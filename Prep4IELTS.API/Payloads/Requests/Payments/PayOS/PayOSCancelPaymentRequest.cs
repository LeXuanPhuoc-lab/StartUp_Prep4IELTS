using System.Net;
using System.Text;
using EXE202_Prep4IELTS.Payloads.Responses.Payments.PayOS;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Prep4IELTS.Business.Models;

namespace EXE202_Prep4IELTS.Payloads.Requests.Payments.PayOS;

public class PayOSCancelPaymentRequest
{
    public string CancellationReason { get; set; } = string.Empty;
}

public static class PayOSCancelPaymentRequestExtension
{
    public static async Task<(bool, string?, PayOSPaymentLinkInformationResponse)> CancelAsync(
        this PayOSCancelPaymentRequest req, 
        string paymentLinkId,
        PayOSConfiguration payOsConfig)
    {
        // Initiate HttpClient
        using HttpClient httpClient = new();
        
        // Add header parameters
        httpClient.DefaultRequestHeaders.Add("x-client-id", payOsConfig.ClientId);
        httpClient.DefaultRequestHeaders.Add("x-api-key", payOsConfig.ApiKey);
        
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
        // Add params to Url by formating string
        var cancelPaymentUrl = string.Format(payOsConfig.CancelPaymentUrl, paymentLinkId);
        // Execute POST request with uri and request content
        var cancelPaymentUrlRes = await httpClient.PostAsync(
            requestUri: cancelPaymentUrl, 
            content: requestContent);
        
        // Response content
        var content = cancelPaymentUrlRes.Content.ReadAsStringAsync().Result;
        var responseData = JsonConvert.DeserializeObject<PayOSPaymentLinkInformationResponse>(content);
        // Check for response content not found 
        if (responseData == null) return (false, "Request to server failed. Not found any response data", null!);
        
        if (cancelPaymentUrlRes.IsSuccessStatusCode)
        {
            return (true, string.Empty, responseData);
        }
        
        // 400: Webhook Url invalid
        // 401: Missing API Key & Client Key
        // 5xx: Sever error
        return (false, responseData.Desc, responseData);
    }
}