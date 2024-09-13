using System.ComponentModel.DataAnnotations;
using CloudinaryDotNet;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Requests.Payments;
using EXE202_Prep4IELTS.Payloads.Requests.Payments.Momo;
using EXE202_Prep4IELTS.Payloads.Requests.Payments.PayOS;
using EXE202_Prep4IELTS.Payloads.Responses;
using EXE202_Prep4IELTS.Payloads.Responses.Payments.PayOS;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Utils;
using Serilog;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class PaymentController(
    IOptionsMonitor<MomoConfiguration> momoMonitor,
    IOptionsMonitor<PayOSConfiguration> payOsMonitor) : ControllerBase
{
    private readonly MomoConfiguration _momoConfig = momoMonitor.CurrentValue;
    private readonly PayOSConfiguration _payOsConfig = payOsMonitor.CurrentValue;

    [HttpPost(ApiRoute.Payment.CreatePaymentWithMethod, Name = nameof(CreatePaymentWithMethodAsync))]
    public async Task<IActionResult> CreatePaymentWithMethodAsync([FromBody] CreatePaymentRequest req)
    {
        // Check validation errors
        if (!ModelState.IsValid) return BadRequest(ModelState);

        switch (req.PaymentIssuer) // Choose payment issuers
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
                    Amount = req.Amount,
                    OrderId = orderId,
                    OrderInfo = "en".Equals(req.Lang) 
                        ? $"Payment order {orderId}" 
                        : $"Thanh toan hoa don {orderId}",
                    OrderGroupId = string.Empty,
                    RedirectUrl = "http://localhost:7000/api/payment/momo-return",
                    IpnUrl = _momoConfig.IpnUrl,
                    ExtraData = string.Empty,
                    Lang = req.Lang,
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
                            ImageUrl = "https://t4.ftcdn.net/jpg/01/25/36/71/360_F_125367167_JnrCHTqtZhAbWS3doG4tt631usPHiPnr.jpg",
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
                // Generate requestId and orderCode
                var orderCode =  PaymentHelper.GenerateRandomOrderCodeDigits(5);

                // Initiate PayOS payment request
                PayOSPaymentRequest payOsPaymentRequest = new()
                {
                    OrderCode = orderCode,
                    Amount = req.Amount,
                    Description = "Test payment with PayOS",
                    BuyerName = req.Name ?? string.Empty,
                    BuyerEmail = req.Email,
                    BuyerPhone = req.PhoneNumber,
                    BuyerAddress = "Address of merchant",
                    Items = new List<object>()
                    {
                        new
                        {
                            Name = "Iphone",
                            Quantity = 2,
                            Price = 28000000
                        }
                    },
                    CancelUrl = _payOsConfig.CancelUrl,
                    ReturnUrl = _payOsConfig.ReturnUrl,
                    ExpiredAt = (int)((DateTimeOffset)DateTime.Now.AddHours(2)).ToUnixTimeSeconds()
                };
                
                // Generate signature
                await payOsPaymentRequest.GenerateSignatureAsync(orderCode, _payOsConfig);
                var payOsPaymentResp = await payOsPaymentRequest.GetUrlAsync(_payOsConfig);

                return payOsPaymentResp.Item1
                    ? Ok(payOsPaymentResp.Item3)
                    : BadRequest(new BaseResponse()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = payOsPaymentResp.Item2
                    });
            #endregion
        }

        return BadRequest();
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
        [FromQuery] 
        [MaxLength(length: 2, ErrorMessage = "Language of message must be (vi or en).")] string lang)
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
    #endregion
}