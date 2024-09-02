using System.ComponentModel.DataAnnotations;
using System.Text;
using EXE202_Prep4IELTS.Payloads.Responses;
using EXE202_Prep4IELTS.Payloads.Responses.Payments;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Utils;

namespace EXE202_Prep4IELTS.Payloads.Requests.Payments;

public class MomoOneTimePaymentRequest
{
    [Required]
    [MaxLength(length: 50, ErrorMessage = "PartnerCode must smaller than 50 chars")]
    public string PartnerCode { get; set; } = string.Empty;   
    
    
    public string? PartnerName { get; set; } = string.Empty;   
    
    public string? SubPartnerCode { get; set; } = string.Empty;   
    
    
    public string? StoreId { get; set; } = string.Empty;   
    
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
    
    public string? OrderGroupId { get; set; } = string.Empty;   
    
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

    public List<MomoPaymentItem> Items = new();
    
    [MaxLength(length: 1000, ErrorMessage = "ReferenceId must smaller than 1000 chars")]
    public string? ReferenceId { get; set; } = null!;

    public bool? AutoCapture { get; set; }

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
        req.Signature = HashHelper.HmacSha256(rawSignature, secretKey);
        await Task.CompletedTask;
        // await Task.FromResult(req.Signature = HashHelper.HmacSHA256(rawSignature, secretKey));
    }

    public static async Task<(bool, string?, MomoOneTimePaymentResponse?)> GetUrlAsync(this MomoOneTimePaymentRequest req, string paymentUrl)
    {
        // Initiate HttpClient
        using HttpClient httpClient = new();
        // Convert request data to type of JSON
        var requestData = JsonConvert.SerializeObject(req, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        });
        // Initiate string content with serialized request data, encoding and media type
        var requestContent = new StringContent(
            content: requestData,
            encoding: Encoding.UTF8,
            mediaType:"application/json");
        // Execute POST request with uri and request content
        var createPaymentUrlRes = await httpClient.PostAsync(
            requestUri: paymentUrl, 
            content: requestContent);
        
        // Response content
        var content = createPaymentUrlRes.Content.ReadAsStringAsync().Result;
        var responseData = JsonConvert.DeserializeObject<MomoOneTimePaymentResponse>(content);
        // Check for response content not found 
        if (responseData == null) return (false, "Request to server failed. Not found any response data", null!);
        
        // [MomoResultCode](https://developers.momo.vn/v3/docs/payment/api/result-handling/resultcode/)
        if (createPaymentUrlRes.IsSuccessStatusCode)
        {
            // Check result code status 
            if (responseData.ResultCode.ToString()
                    .Equals(MomoResultCodeConstants.Successful)) 
            {
                return (true, string.Empty, responseData);
            }
            
            return (false, responseData.Message, responseData);
        }
        
        return (false, responseData.Message, null!);
    }
}