using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EXE202_Prep4IELTS.Payloads.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Utils;
using Serilog;

namespace EXE202_Prep4IELTS.Payloads.Requests;

public class MomoOneTimePaymentRequest
{
    [Required]
    [MaxLength(length: 50, ErrorMessage = "PartnerCode must smaller than 50 chars")]
    public string PartnerCode { get; set; } = string.Empty;   
    
    // public string? SubPartnerCode { get; set; } = string.Empty;   
    
    // public string? StoreName { get; set; } = string.Empty;   
    
    // public string? StoreId { get; set; } = string.Empty;   
    
    [Required]
    [MaxLength(length: 50, ErrorMessage = "RequestId must smaller than 50 chars")]
    public string RequestId { get; set; } = string.Empty;   
    
    [Required]
    public long Amount { get; set; }  
    
    [Required]
    public string OrderId { get; set; } = string.Empty;   
    
    [Required]
    [MaxLength(length: 200, ErrorMessage = "RequestId must smaller than 200 chars")]
    public string OrderInfo { get; set; } = string.Empty;   
    
    // public string? OrderGroupId { get; set; } = string.Empty;   
    
    [Required]
    public string RedirectUrl { get; set; } = string.Empty;   
    
    [Required]
    public string IpnUrl { get; set; } = string.Empty;   
    
    [Required]
    public string RequestType { get; set; } = string.Empty;   
    
    [Required]
    [MaxLength(length: 1000, ErrorMessage = "RequestId must smaller than 1000 chars")]
    public string ExtraData { get; set; } = string.Empty;

    // public object? DeliveryInfo { get; set; } = null!;
    
    public object? UserInfo { get; set; } = null!;
    
    // [MaxLength(length: 1000, ErrorMessage = "ReferenceId must smaller than 1000 chars")]
    // public string? ReferenceId { get; set; } = null!;

    // public bool? AutoCapture { get; set; }

    [Required] public string Lang { get; set; } = string.Empty;

    [Required] public string Signature { get; set; } = string.Empty;

}

public static class MomoOneTimePaymentRequestExtension
{
    public static async Task GenerateSignatureAsync(this MomoOneTimePaymentRequest req, string accessKey, string secretKey)
    {
        var rawSignature = 
            $"accessKey={accessKey}&amount={req.Amount}&extraData={req.ExtraData}" +
            $"&ipnUrl={req.IpnUrl}&orderId={req.OrderId}&orderInfo={req.OrderInfo}" +
            $"&partnerCode={req.PartnerCode}&redirectUrl={req.RedirectUrl}&requestId={req.RequestId}&requestType={req.RequestType}";
        req.Signature = HashHelper.HmacSHA256(rawSignature, secretKey);
        await Task.CompletedTask;
    }

    public static async Task<(bool, string?, MomoOneTimePaymentWithUrlResponse?)> GetUrlAsync(this MomoOneTimePaymentRequest req, string paymentUrl)
    {
        using HttpClient httpClient = new();
        var reqData = JsonConvert.SerializeObject(req, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        });
        var reqContent = new StringContent(reqData, Encoding.UTF8, "application/json");

        var createPaymentUrlRes = await httpClient.PostAsync(paymentUrl, reqContent);
        var contents = createPaymentUrlRes.Content.ReadAsStringAsync().Result;
        Log.Information(contents + "");

        // [MomoResultCode](https://developers.momo.vn/v3/docs/payment/api/result-handling/resultcode/)
        if (createPaymentUrlRes.IsSuccessStatusCode)
        {
            var responseContext = await createPaymentUrlRes.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<MomoOneTimePaymentWithUrlResponse>(responseContext);

            if (responseData == null) return (false, "Request to server failed. Not found any response data", responseData);
            
            // Check result code status 
            if (responseData.ResultCode.ToString()
                    .Equals(MomoResultCodeConstants.Successful))
            {
                return (true, responseData.PayUrl, responseData);
            }
            else
            {
                return (false, responseData.Message, responseData);
            }
        }
        else
        {
            return (false, createPaymentUrlRes.ReasonPhrase, null!);
        }
    }
}