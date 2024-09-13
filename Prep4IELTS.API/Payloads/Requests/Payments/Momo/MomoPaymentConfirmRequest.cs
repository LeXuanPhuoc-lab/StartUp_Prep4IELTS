using System.Text;
using EXE202_Prep4IELTS.Payloads.Responses.Payments;
using EXE202_Prep4IELTS.Payloads.Responses.Payments.Momo;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Utils;

namespace EXE202_Prep4IELTS.Payloads.Requests.Payments.Momo;

public class MomoPaymentConfirmRequest
{
    public string PartnerCode { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public long Amount { get; set; }
    public string Lang { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
}

public static class PaymentConfirmRequestExtension
{
    public static async Task GenerateSignatureAsync(this MomoPaymentConfirmRequest req, 
        string accessKey, string secretKey)
    {
        var rawSignature = $"accessKey={accessKey}&amount={req.Amount}&description={req.Description}&orderId={req.OrderId}" +
                           $"&partnerCode={req.PartnerCode}&requestId={req.RequestId}&requestType={req.RequestType}";
        await Task.FromResult(req.Signature = HashHelper.HmacSha256(rawSignature, secretKey));
    }

    public static async Task<(bool, string?, MomoPaymentConfirmResponse)> ConfirmPaymentAsync(this MomoPaymentConfirmRequest req, 
        string paymentConfirmUrl)
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
        var paymentConfirmRes = await httpClient.PostAsync(
            requestUri: paymentConfirmUrl, 
            content: requestContent);
        
        // Response content
        var content = paymentConfirmRes.Content.ReadAsStringAsync().Result;
        var responseData = JsonConvert.DeserializeObject<MomoPaymentConfirmResponse>(content);
        // Check for response content not found 
        if (responseData == null) return (false, "Request to server failed. Not found any response data", null!);
        
        if (paymentConfirmRes.IsSuccessStatusCode)
        {
            // Check result code status 
            if (responseData.ResultCode.ToString()
                .Equals(MomoResultCodeConstants.Successful)) // Transfer to respective partner successfully
            {
                return (true, string.Empty, responseData);
            }
            
            // Failed Status (Cancel)
            return (false, responseData.Message, responseData);
        }
        
        // Invoke problem
        return (false, responseData?.Message, null!);
    }
}