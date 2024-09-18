using System.ComponentModel.DataAnnotations;
using CloudinaryDotNet;
using EXE202_Prep4IELTS.Attributes;
using EXE202_Prep4IELTS.Extensions;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Requests.Payments;
using EXE202_Prep4IELTS.Payloads.Requests.Payments.Momo;
using EXE202_Prep4IELTS.Payloads.Requests.Payments.PayOS;
using EXE202_Prep4IELTS.Payloads.Responses;
using EXE202_Prep4IELTS.Payloads.Responses.Payments.PayOS;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;
using Serilog;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class PaymentController(
    IPremiumPackageService premiumPackageService,
    ITransactionService transactionService,
    IUserPremiumPackageService userPremiumPackageService,
    IPaymentTypeService paymentTypeService,
    IOptionsMonitor<MomoConfiguration> momoMonitor,
    IOptionsMonitor<PayOSConfiguration> payOsMonitor) : ControllerBase
{
    private readonly MomoConfiguration _momoConfig = momoMonitor.CurrentValue;
    private readonly PayOSConfiguration _payOsConfig = payOsMonitor.CurrentValue;

    [HttpPost(ApiRoute.Payment.CreatePaymentWithMethod, Name = nameof(CreatePaymentWithMethodAsync))]
    [ClerkAuthorize(Roles = [SystemRoleConstants.Staff, SystemRoleConstants.Student])]
    public async Task<IActionResult> CreatePaymentWithMethodAsync([FromBody] CreatePaymentRequest req)
    {
        // Check validation errors
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        
        // Check exist user 
        var userDto = HttpContext.Items["User"] as UserDto;
        if (userDto == null)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Not found user"
            });
        }
        
        // Check user exist email
        if (string.IsNullOrEmpty(userDto.Email))
        {
            ModelState.AddModelError("email", "Email is required, please update your profile");
            return UnprocessableEntity(ModelState);
        }
        
        // Check exist premium package
        var isExistPremiumPackage = await premiumPackageService.IsExistPremiumPackageAsync(req.PremiumPackageId);
        if (!isExistPremiumPackage)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Not found premium package"
            });
        }
        
        // Check exist payment type
        var paymentTypeDto = await paymentTypeService.FindByIdAsync(req.PaymentTypeId);
        if (paymentTypeDto == null)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Not found payment type"
            });
        }
        
        switch (paymentTypeDto.PaymentMethod) // Choose payment issuers
        {
            #region Momo Issuers

            // MOMO
            // [Momo Test Checkout](https://developers.momo.vn/v3/checkout/)
            case PaymentIssuerConstants.Momo:

                // Check existing request type 
                if (string.IsNullOrEmpty(req.RequestType))
                {
                    return BadRequest(new BaseResponse()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Missing request type for Momo issuer"
                    });
                }

                // Generate requestId and orderId   
                var requestId = PaymentHelper.GenerateRequestId();
                var orderId = PaymentHelper.GenerateOrderId(requestId);

                // [Momo Test Account](https://developers.momo.vn/v3/docs/payment/onboarding/test-instructions/#e-wallet-test-details)
                // Initiate Momo one time payment request 
                MomoOneTimePaymentRequest momoOneTimeRequest = new()
                {
                    PartnerCode = _momoConfig.PartnerCode,
                    RequestId = requestId,
                    RequestType = req.RequestType,
                    Amount = 1000,
                    OrderId = orderId,
                    OrderInfo = "en".Equals(req.Lang)
                        ? $"Payment order {orderId}"
                        : $"Thanh toan hoa don {orderId}",
                    OrderGroupId = string.Empty,
                    RedirectUrl = "http://localhost:7000/api/payment/momo-return",
                    IpnUrl = _momoConfig.IpnUrl,
                    ExtraData = string.Empty,
                    Lang = req.Lang ?? "vn",
                    AutoCapture = false,
                    StoreId = string.Empty,
                    SubPartnerCode = String.Empty,
                    UserInfo = new
                    {
                        Email = req.Email,
                        Name = req.Name,
                        PhoneNumber = req.PhoneNumber
                    },
                    Items = new()
                    {
                        new()
                        {
                            Id = 1,
                            Name = "Prep4Study Premium Package",
                            Description = "Premium monthly",
                            Category = "package",
                            ImageUrl =
                                "https://t4.ftcdn.net/jpg/01/25/36/71/360_F_125367167_JnrCHTqtZhAbWS3doG4tt631usPHiPnr.jpg",
                            Price = 100_000,
                            Manufacturer = string.Empty,
                            Quantity = 1,
                            Unit = "package",
                            TotalPrice = 100_000
                        }
                    }
                };
                await momoOneTimeRequest.GenerateSignatureAsync(_momoConfig.AccessKey, _momoConfig.SecretKey);
                var paymentResp = await momoOneTimeRequest.GetUrlAsync(_momoConfig.PaymentUrl);

                return paymentResp.Item1
                    ? Ok(paymentResp.Item3)
                    : BadRequest(new BaseResponse()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = paymentResp.Item2
                    });

            #endregion

            #region PayOS

            // PayOS
            case PaymentIssuerConstants.PayOs:
                // Get premium package by id 
                var premiumPackageDto = 
                    await premiumPackageService.FindOneWithConditionAsync(
                        pp => pp.PremiumPackageId == req.PremiumPackageId);
                
                // Check exist user premium package in any transaction
                var userPremiumPackageDto =
                    await transactionService.FindUserPremiumPackageByUserIdAsync(userDto.UserId);
                // Initiate user premium package
                var currentTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.UtcNow);
                if (userPremiumPackageDto != null)
                {
                    // Check expired date
                    var isExpired = userPremiumPackageDto.ExpireDate > currentTime;
                    if (isExpired && userPremiumPackageDto.IsActive)
                    {
                        ModelState.AddModelError("expireDate", $"Your premium package '{premiumPackageDto.PremiumPackageName}' is not expired yet.");
                        return UnprocessableEntity(ModelState);
                    }
                }
                
                // Generate requestId and orderCode
                var orderCode = PaymentHelper.GenerateRandomOrderCodeDigits(8);

                var paymentExpireAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddHours(2), 
                    // Vietnam timezone
                    TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
                // Initiate PayOS payment request
                PayOSPaymentRequest payOsPaymentRequest = new()
                {
                    OrderCode = orderCode,
                    Amount = (int) premiumPackageDto.Price,
                    Description = "Prep4IELTS Premium",
                    BuyerName = $"{userDto.FirstName} {userDto.LastName}".ToUpper(),
                    BuyerEmail = userDto.Email,
                    BuyerPhone = req.PhoneNumber ?? string.Empty,
                    BuyerAddress = string.Empty,
                    Items =
                    [
                        new
                        {
                            Name = "Premium Package",
                            Quantity = 1,
                            Price = premiumPackageDto.Price
                        }
                    ],
                    CancelUrl = _payOsConfig.CancelUrl,
                    ReturnUrl = _payOsConfig.ReturnUrl,
                    ExpiredAt = (int)((DateTimeOffset)paymentExpireAt).ToUnixTimeSeconds()
                };
                
                // Generate signature
                await payOsPaymentRequest.GenerateSignatureAsync(orderCode, _payOsConfig);
                var payOsPaymentResp = await payOsPaymentRequest.GetUrlAsync(_payOsConfig);
            
                // Create Payment status
                bool isCreatePaymentSuccess = payOsPaymentResp.Item1;
                bool isTransactionCreated = false;
                
                // Check if create payment success with resp data
                if (isCreatePaymentSuccess && payOsPaymentResp.Item3 != null)
                {
                    // Map user premium package to entity to create new user premium package
                    UserPremiumPackage? userPremiumPackage = userPremiumPackageDto.Adapt<UserPremiumPackage>();
                    // Initiate package expire time by adding months
                    var packageExpiredAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddMonths(premiumPackageDto.DurationInMonths), 
                        // Vietnam timezone
                        TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
                    if (userPremiumPackage != null!) // Exist any package 
                    {
                        // Progress update existing package with new expired time and premium package 
                        userPremiumPackage.ExpireDate = packageExpiredAt;
                        userPremiumPackage.IsActive = false; // Change to not paid yet
                        userPremiumPackage.PremiumPackageId = req.PremiumPackageId;
                        userPremiumPackage.UserId = userDto.UserId;
                        // Set null user 
                        userPremiumPackage.User = null!;
                        
                        // Update user premium package
                        await userPremiumPackageService.UpdateAsync(userPremiumPackage);
                    }
                    else
                    {
                        // Create user premium package 
                        userPremiumPackage = new UserPremiumPackage()
                        {
                            UserId = userDto.UserId,
                            PremiumPackageId = req.PremiumPackageId,
                            ExpireDate = packageExpiredAt,
                            IsActive = false // Not paid yet
                        };
                    }
                    
                    // Create transaction
                    var transaction = new Prep4IELTS.Data.Entities.Transaction
                    {
                        UserId = userDto.UserId,
                        TransactionCode = orderCode.ToString(),
                        PaymentLinkId = payOsPaymentResp.Item3.Data.PaymentLinkId,
                        // PaymentAmount = premiumPackageDto.Price,
                        // Pending status
                        TransactionStatus = TransactionStatus.Pending.GetDescription(), 
                        CreateAt = TimeZoneInfo.ConvertTimeFromUtc(
                            DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")),
                        PaymentTypeId = req.PaymentTypeId
                    };
                    // Add user premium package
                    if (userPremiumPackage.UserPremiumPackageId > 0) // Exist yet
                    {
                        transaction.UserPremiumPackageId = userPremiumPackage.UserPremiumPackageId;
                    }
                    else // Not exist yet -> new instance
                    {
                        transaction.UserPremiumPackage = userPremiumPackage;
                    }
                    
                    // Create transaction
                    isTransactionCreated = await transactionService.CreateAsync(transaction.Adapt<TransactionDto>());
                }
                
                return isCreatePaymentSuccess && isTransactionCreated // Create payment and save transaction success
                    ? Ok(payOsPaymentResp.Item3)
                    : !isCreatePaymentSuccess // Error invoke while request to create payment
                        ? BadRequest(new BaseResponse()
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = payOsPaymentResp.Item2
                        })
                        // Save transaction error
                        : StatusCode(StatusCodes.Status500InternalServerError);

            #endregion
        }

        return BadRequest();
    }

    [HttpGet(ApiRoute.Payment.GetAllPaymentType)]
    public async Task<IActionResult> GetAllPaymentTypesAsync()
    {
        var paymentTypeDtos = await paymentTypeService.FindAllAsync();
        return Ok(new BaseResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Data = paymentTypeDtos.Any() ? paymentTypeDtos : new List<PaymentTypeDto>()
        });
    }
    
    [HttpGet(ApiRoute.Payment.GetPaymentIssuers)]
    public async Task<IActionResult> GetPaymentIssuerAsync()
    {
        List<string> paymentIssuers = new()
        {
            PaymentIssuerConstants.Momo,
            PaymentIssuerConstants.PayOs
        };

        await Task.CompletedTask;

        return paymentIssuers.Any()
            ? Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = paymentIssuers
            })
            : NotFound();
    }

    #region Momo Payment

    [HttpGet(ApiRoute.Payment.GetMomoPaymentMethods)]
    public async Task<IActionResult> GetMomoPaymentMethodsAsync(
        [FromQuery] [MaxLength(length: 2, ErrorMessage = "Language of message must be (vi or en).")]
        string lang)
    {
        // Check validation errors
        if (!ModelState.IsValid) return BadRequest(ModelState);

        List<MomoPaymentMethod> momoPaymentMethods = new()
        {
            new()
            {
                MethodName = "en".Equals(lang) ? "MoMo Wallet" : "Ví MoMo",
                RequestType = MomoPaymentMethodConstants.MomoWallet
            },
            new()
            {
                MethodName = "en".Equals(lang) ? "Domestic ATM Card" : "Thẻ ATM nội địa",
                RequestType = MomoPaymentMethodConstants.PayWithAtm
            },
            new()
            {
                MethodName = "en".Equals(lang) ? "Visa/Mastercard/JCB" : "Thẻ Visa/Mastercard/JCB",
                RequestType = MomoPaymentMethodConstants.PayWithCreditCard
            },
            new()
            {
                MethodName = "en".Equals(lang) ? "Momo Later Payment" : "Ví trả sau",
                RequestType = MomoPaymentMethodConstants.PayWithVts
            }
        };

        return await Task.FromResult(Ok(new BaseResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Data = momoPaymentMethods
        }));
    }

    [HttpPost(ApiRoute.Payment.CheckTransactionStatus)]
    public async Task<IActionResult> CheckTransactionStatusAsync([FromRoute] string orderId)
    {
        // Initiate Momo check transaction request
        MomoCheckTransactionRequest checkTransactionRequestRequest = new()
        {
            PartnerCode = _momoConfig.PartnerCode,
            RequestId = Guid.NewGuid().ToString(),
            OrderId = orderId,
            Lang = "vi"
        };
        // Generate request signature
        await checkTransactionRequestRequest.GenerateSignatureAsync(
            _momoConfig.AccessKey, _momoConfig.SecretKey);
        // Perform check transaction with POST method
        var checkTransactionResp = await checkTransactionRequestRequest.CheckTransactionStatusAsync(
            _momoConfig.CheckTransactionUrl);

        return checkTransactionResp.Item1
            ? Ok(checkTransactionResp.Item3)
            : BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = checkTransactionResp.Item2
            });
    }

    [HttpPost(ApiRoute.Payment.Confirm)]
    public async Task<IActionResult> ConfirmPaymentAsync([FromBody] MomoConfirmPaymentRequest req)
    {
        // Initiate Momo confirm payment request
        MomoPaymentConfirmRequest momoPaymentConfirmReq = new()
        {
            PartnerCode = _momoConfig.PartnerCode,
            RequestId = Guid.NewGuid().ToString(),
            OrderId = req.OrderId,
            Lang = "vi",
            RequestType = req.RequestType,
            Amount = req.Amount,
            Description = req.Description
        };
        // Generate request signature
        await momoPaymentConfirmReq.GenerateSignatureAsync(
            _momoConfig.AccessKey, _momoConfig.SecretKey);
        // Perform payment confirmation req with POST method
        var paymentConfirmResp = await momoPaymentConfirmReq.ConfirmPaymentAsync(
            _momoConfig.PaymentConfirmUrl);

        // Response
        return paymentConfirmResp.Item1
            ? Ok(paymentConfirmResp.Item3)
            : BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = paymentConfirmResp.Item2
            });
    }

    [HttpPost(ApiRoute.Payment.InitiateTransaction)]
    public async Task<IActionResult> ConfirmPaymentAsync()
    {
        MomoInitiateTransactionRequest req = new()
        {
            PartnerCode = _momoConfig.PartnerCode,
            RequestId = Guid.NewGuid().ToString(),
            RequestType = MomoPaymentMethodConstants.Initiate,
            Amount = 100_000,
            OrderId = Guid.NewGuid().ToString(),
            OrderInfo = "Test payment with momo",
            RedirectUrl = string.Empty,
            IpnUrl = _momoConfig.IpnUrl,
            PartnerClientId = "stevenmarks147@gmail.com",
            ExtraData = "email=abc@gmail.com",
            Lang = "vi",
        };
        await req.GenerateSignatureAsync(_momoConfig.AccessKey, _momoConfig.SecretKey);
        var initiateTransactionResp = await req.InitiateTransactionAsync(_momoConfig.InitiateTransactionUrl);

        // Response
        return initiateTransactionResp.Item1
            ? Ok(initiateTransactionResp.Item3)
            : BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = initiateTransactionResp.Item2
            });
    }

    [HttpGet(ApiRoute.Payment.MomoReturn)]
    public async Task<IActionResult> MomoPaymentReturnAsync()
    {
        Log.Information("Momo Return Success");
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost(ApiRoute.Payment.MomoIpn)]
    public async Task<IActionResult> MomoIpnReturnAsync([FromBody] MomoPaymentNotification? notification)
    {
        if (notification == null)
        {
            Log.Error("Momo IPN failed: notification is null");
            return BadRequest("Invalid data");
        }

        Log.Information($"Momo IPN Success: {notification.OrderId} with amount {notification.Amount}");

        await Task.CompletedTask;
        return Ok();
    }

    #endregion

    #region PayOS

    [HttpGet(ApiRoute.Payment.GetPayOsPaymentLinkInformation)]
    public async Task<IActionResult> PayOsPaymentLinkInformationAsync([FromRoute] string paymentLinkId)
    {
        var getLinkInformationResp =
            await PayOsPaymentRequestExtensions.GetLinkInformationAsync(paymentLinkId, _payOsConfig);

        return getLinkInformationResp.Item1
            ? Ok(getLinkInformationResp.Item3)
            : BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = getLinkInformationResp.Item2
            });
    }

    [HttpPost(ApiRoute.Payment.CancelPayOsPayment)]
    public async Task<IActionResult> CancelPayOsPaymentAsync(
        [FromRoute] string paymentLinkId,
        [FromBody] PayOSCancelPaymentRequest req)
    {
        // Process cancel payOS payment
        var cancelResp = await req.CancelAsync(paymentLinkId, _payOsConfig);

        return cancelResp.Item1
            ? Ok(cancelResp.Item3)
            : BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = cancelResp.Item2
            });
    }

    [HttpPost(ApiRoute.Payment.VerifyPaymentWebhookData)]
    public async Task<IActionResult> VerifyPaymentWebhookDataAsync([FromBody] PayOSPaymentLinkInformationResponse req)
    {
        PayOS payOs = new PayOS(_payOsConfig.ClientId, _payOsConfig.ApiKey, _payOsConfig.ChecksumKey);
        
        // Initiate transaction detail
        string transactionCode = req.Data.OrderCode;
        DateTime? transactionDate = null;
        decimal? paymentAmount = null;
        string? cancellationReason = null;
        DateTime? cancelledAt = null;
        TransactionStatus status = TransactionStatus.Pending;
        bool isPremiumPackageActivated = false;
        
        // Get transaction by code 
        var transactionDto = await transactionService.FindTransactionByCodeAsync(transactionCode);
        // Check exist transaction
        if (transactionDto == null)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Not found any transaction valid"
            });
        }

        // Check payment information status
        switch (req.Data.Status)
        {
            case PayOSTransactionStatusConstants.Pending:
                // Update transaction status to DB
                status = TransactionStatus.Pending;
                break;
            case PayOSTransactionStatusConstants.Expired:
                // Update transaction status to DB
                status = TransactionStatus.Expired;
                break;
            case PayOSTransactionStatusConstants.Cancelled:
                // Update transaction status to DB
                status = TransactionStatus.Cancelled;
                cancellationReason = req.Data.CancellationReason;
                cancelledAt = !string.IsNullOrEmpty(req.Data.CanceledAt) ? DateTime.Parse(req.Data.CanceledAt) : null;
                break;
            case PayOSTransactionStatusConstants.Paid:
                try
                {
                    // Initiate Webhook type
                    WebhookType webhookType = new(
                        req.Code,
                        req.Desc,
                        new WebhookData(
                            orderCode: long.Parse(req.Data.OrderCode),
                            amount: (int)req.Data.Transactions[0].Amount,
                            description: req.Data.Transactions[0].Description,
                            accountNumber: req.Data.Transactions[0].AccountNumber,
                            reference: req.Data.Transactions[0].Reference ?? string.Empty,
                            transactionDateTime: req.Data.Transactions[0].TransactionDateTime,
                            currency: "VND",
                            paymentLinkId: req.Data.Id,
                            code: req.Code,
                            desc: req.Desc,
                            counterAccountBankId: req.Data.Transactions[0].CounterAccountBankId,
                            counterAccountBankName: req.Data.Transactions[0].CounterAccountBankName,
                            counterAccountName: req.Data.Transactions[0].CounterAccountName,
                            counterAccountNumber: req.Data.Transactions[0].CounterAccountNumber,
                            virtualAccountName: req.Data.Transactions[0].VirtualAccountName,
                            virtualAccountNumber: req.Data.Transactions[0].VirtualAccountNumber ?? string.Empty
                        ),
                        await req.GenerateWebhookSignatureAsync(req.Data.Id, _payOsConfig.ChecksumKey));

                    // Verify payment webhook data
                    WebhookData webhookData = payOs.verifyPaymentWebhookData(webhookType);
                    
                    // Update transaction status to DB
                    if (webhookData != null!) // Success status
                    {
                        DateTimeOffset parsedDateTimeOffset = DateTimeOffset.ParseExact(
                            webhookData.transactionDateTime, "yyyy-MM-ddTHH:mm:ssK", null);
                        // Transaction datetime
                        transactionDate = parsedDateTimeOffset.DateTime;
                        // Payment amount
                        paymentAmount = webhookData.amount;
                        // Transaction status
                        status = TransactionStatus.Paid;
                    }
                }
                catch (Exception ex) // Invoke exception
                {
                    throw new Exception(ex.Message);
                }
                break;
        }
        
        // Process update transaction status
        var isSuccess = await transactionService.UpdateTransactionStatusAsync(
            transactionCode, transactionDate, paymentAmount, cancellationReason, cancelledAt, status);
        
        // Update activated status
        if (isSuccess && status.Equals(TransactionStatus.Paid)) isPremiumPackageActivated = true;
        
        return isSuccess // Update transaction status success
            ? Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    TransactionStatus = status.GetDescription(),
                    IsPremiumPackageActivated = isPremiumPackageActivated
                }
            })
            // Transaction already has status
            : !string.IsNullOrEmpty(transactionDto.TransactionStatus) 
                ? Ok(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = new
                    {
                        TransactionStatus = transactionDto.TransactionStatus
                    }
                })
                : StatusCode(StatusCodes.Status500InternalServerError);
    }


    [HttpPost(ApiRoute.Payment.WebhookPayOsReturn)]
    public async Task<IActionResult> WebhookPayOsReturnAsync([FromBody] WebhookType req)
    {
        Log.Information("Received data from web hook");
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost(ApiRoute.Payment.WebhookPayOsCancel)]
    public async Task<IActionResult> WebhookPayOsCancelAsync([FromBody] WebhookType req)
    {
        Log.Information("Received data from web hook");
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("Test")]
    public async Task<IActionResult> TestAsync()
    {
        PayOS payOs = new PayOS(_payOsConfig.ClientId, _payOsConfig.ApiKey, _payOsConfig.ChecksumKey);
        // var confirmWebhookUrl = await payOs.confirmWebhook("https://7b3b-2402-800-63b6-b04f-e85b-5329-2528-3e5f.ngrok-free.app/api/payment/pay-os/return");
        var confirmWebhookUrl =
            await payOs.confirmWebhook(
                "https://7b3b-2402-800-63b6-b04f-e85b-5329-2528-3e5f.ngrok-free.app/api/payment/pay-os/cancel?code=00&id=eb776244be6640b19bf62b28e963d3e5&cancel=true&status=CANCELLED&orderCode=59272n");

        return !string.IsNullOrEmpty(confirmWebhookUrl)
            ? Ok()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }

    #endregion
}