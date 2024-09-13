using System.Net;
using System.Text;
using EXE202_Prep4IELTS.Payloads.Responses.Payments.Momo;
using EXE202_Prep4IELTS.Payloads.Responses.Payments.PayOS;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Utils;

namespace EXE202_Prep4IELTS.Payloads.Requests.Payments.PayOS;

public class PayOSPaymentRequest
{
    public int OrderCode { get; set; } 
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string BuyerName { get; set; } = string.Empty;
    public string BuyerEmail { get; set; } = string.Empty;
    public string BuyerPhone { get; set; } = string.Empty;
    public string BuyerAddress { get; set; } = string.Empty;
    public List<object> Items { get; set; } = new();
    public string CancelUrl { get; set; } = string.Empty;
    public string ReturnUrl { get; set; } = string.Empty;
    public int ExpiredAt { get; set; }
    public string Signature { get; set; } = string.Empty;
}

public static class PayOsPaymentRequestExtensions
{
    public static async Task GenerateSignatureAsync(this PayOSPaymentRequest req, int orderCode,
        PayOSConfiguration payOsConfig)
    {
        var rawSignature = $"amount={req.Amount}&cancelUrl={payOsConfig.CancelUrl}&description={req.Description}" +
                           $"&orderCode={orderCode}&returnUrl={payOsConfig.ReturnUrl}";
        req.Signature = HashHelper.HmacSha256(rawSignature, payOsConfig.ChecksumKey);
        await Task.CompletedTask;
    }
    
    public static async Task<(bool, string?, PayOSPaymentResponse?)> GetUrlAsync(this PayOSPaymentRequest req, 
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
        // Execute POST request with uri and request content
        var createPaymentUrlRes = await httpClient.PostAsync(
            requestUri: payOsConfig.PaymentUrl, 
            content: requestContent);
        
        // Response content
        var content = createPaymentUrlRes.Content.ReadAsStringAsync().Result;
        var responseData = JsonConvert.DeserializeObject<PayOSPaymentResponse>(content);
        // Check for response content not found 
        if (responseData == null) return (false, "Request to server failed. Not found any response data", null!);
        
        if (createPaymentUrlRes.IsSuccessStatusCode)
        {
            return (true, string.Empty, responseData);
        }
        
        // 409: Too many request to PayOS server 
        return (false, "Too many request", responseData);
    }

    public static async Task<(bool, string?, PayOSPaymentLinkInformationResponse?)> GetLinkInformationAsync(string paymentLinkId,
        PayOSConfiguration payOsConfig)
    {
        // Initiate HttpClient
        using HttpClient httpClient = new();
        
        // Add header parameters
        httpClient.DefaultRequestHeaders.Add("x-client-id", payOsConfig.ClientId);
        httpClient.DefaultRequestHeaders.Add("x-api-key", payOsConfig.ApiKey);
        
        // Add params to Url by formating string
        var getPaymentLinkInformationUrl = string.Format(payOsConfig.GetPaymentLinkInformationUrl, paymentLinkId);
        // Execute GET request with uri 
        var createPaymentUrlRes = await httpClient.GetAsync(
            requestUri: getPaymentLinkInformationUrl);
        
        // Response content
        var content = createPaymentUrlRes.Content.ReadAsStringAsync().Result;
        var responseData = JsonConvert.DeserializeObject<PayOSPaymentLinkInformationResponse>(content);
        // Check for response content not found 
        if (responseData == null) return (false, "Request to server failed. Not found any response data", null!);
        
        if (createPaymentUrlRes.IsSuccessStatusCode)
        {
            return (true, string.Empty, responseData);
        }
        
        // 409: Too many request to PayOS server 
        return (false, "Too many request", responseData);
    }
}