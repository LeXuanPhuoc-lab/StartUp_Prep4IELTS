using System.Text;
using EXE202_Prep4IELTS.Payloads.Responses.Payments;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Utils;

namespace EXE202_Prep4IELTS.Payloads.Requests.Payments;

public class MomoInitiateTransactionRequest
{
    public string PartnerCode { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;
    public long Amount { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public string OrderInfo { get; set; } = string.Empty;
    public string RedirectUrl { get; set; } = string.Empty;
    public string IpnUrl { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public string ExtraData { get; set; } = string.Empty;
    public string PartnerClientId { get; set; } = string.Empty;
    public string Lang { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
    public object? UserInfo { get; set; }
}

public static class MomoInitiateTransactionRequestExtension
{
    public static async Task GenerateSignatureAsync(this MomoInitiateTransactionRequest req, string accessKey, string secretKey)
    {
        var rawSignature = 
            $"accessKey={accessKey}&amount={req.Amount}&extraData={req.ExtraData}" +
            $"&ipnUrl={req.IpnUrl}&orderId={req.OrderId}&orderInfo={req.OrderInfo}&partnerClientId={req.PartnerClientId}" +
            $"&partnerCode={req.PartnerCode}&redirectUrl={req.RedirectUrl}&requestId={req.RequestId}&requestType={req.RequestType}";
        await Task.FromResult(req.Signature = HashHelper.HmacSha256(rawSignature, secretKey));
    }

    public static async Task<(bool, string?, MomoInitiateTransactionResponse?)> InitiateTransactionAsync(this MomoInitiateTransactionRequest req,
        string initiateTransactionUrl)
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
        var initiatePaymentRes = await httpClient.PostAsync(
            requestUri: initiateTransactionUrl, 
            content: requestContent);
        
        // Response content
        var content = initiatePaymentRes.Content.ReadAsStringAsync().Result;
        var responseData = JsonConvert.DeserializeObject<MomoInitiateTransactionResponse>(content);
        // Check for response content not found 
        if (responseData == null) return (false, "Request to server failed. Not found any response data", null!);
        
        // [MomoResultCode](https://developers.momo.vn/v3/docs/payment/api/result-handling/resultcode/)
        if (initiatePaymentRes.IsSuccessStatusCode)
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