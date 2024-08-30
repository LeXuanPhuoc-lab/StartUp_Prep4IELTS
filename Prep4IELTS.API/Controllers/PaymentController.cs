using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Requests;
using EXE202_Prep4IELTS.Payloads.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Models;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class PaymentController(
    IOptionsMonitor<MomoConfiguration> momoMonitor) : ControllerBase
{
    private readonly MomoConfiguration _momoConfig = momoMonitor.CurrentValue;
    
    
    [HttpPost(ApiRoute.Payment.CreatePaymentWithMethod, Name = nameof(CreatePaymentWithMethodAsync))]
    public async Task<IActionResult> CreatePaymentWithMethodAsync([FromQuery] string? method)
    {
        switch (method) // Choose payment method
        {
            // MOMO
            case PaymentMethodConstants.Momo:
                // [Momo Test Account](https://developers.momo.vn/v3/docs/payment/onboarding/test-instructions/#e-wallet-test-details)
                // Initiate Momo one time payment request 
                MomoOneTimePaymentRequest momoOneTimeRequest = new()
                {
                    PartnerCode = _momoConfig.PartnerCode,
                    RequestId = Guid.NewGuid().ToString(),
                    // RequestType = "payWithATM",
                    // RequestType = "captureWallet",
                    RequestType = "payWithCC",
                    Amount = 100_000,
                    OrderId = Guid.NewGuid().ToString(),
                    OrderInfo = "Test payment with momo",
                    RedirectUrl = string.Empty,
                    IpnUrl = _momoConfig.IpnUrl,
                    ExtraData = string.Empty,
                    Lang = "vi"
                };
                await momoOneTimeRequest.GenerateSignatureAsync(_momoConfig.AccessKey, _momoConfig.SecretKey);
                var paymentResp = await momoOneTimeRequest.GetUrlAsync(_momoConfig.PaymentUrl);

                if (paymentResp.Item1) return Ok(paymentResp.Item3);
                else return BadRequest(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = paymentResp.Item2
                });
            break;
        }
        
        return BadRequest();
    }
    
    
    [HttpGet(ApiRoute.Payment.MomoReturn)]
    public async Task<IActionResult> MomoPaymentReturnAsync()
    {
        await Task.CompletedTask;
        return Ok();
    }
    
    [HttpGet(ApiRoute.Payment.MomoIpn)]
    public async Task<IActionResult> MomoIpnReturnAsync()
    {
        await Task.CompletedTask;
        return Ok();
    }
}