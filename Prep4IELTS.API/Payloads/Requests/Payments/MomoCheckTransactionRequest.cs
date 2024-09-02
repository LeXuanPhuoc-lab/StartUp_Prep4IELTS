using System.Text;
using EXE202_Prep4IELTS.Payloads.Responses;
using EXE202_Prep4IELTS.Payloads.Responses.Payments;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Utils;

namespace EXE202_Prep4IELTS.Payloads.Requests.Payments;

public class MomoCheckTransactionRequest
{
    public string PartnerCode { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string Lang { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
}

public static class MomoCheckTransactionRequestExtension
{
    public static async Task GenerateSignatureAsync(this MomoCheckTransactionRequest req, 
        string accessKey, string secretKey)
    {
        var rawSignature = $"accessKey={accessKey}&orderId={req.OrderId}" +
                           $"&partnerCode={req.PartnerCode}&requestId={req.RequestId}";
        await Task.FromResult(req.Signature = HashHelper.HmacSha256(rawSignature, secretKey));
    }
    
    public static async Task<(bool, string?, MomoCheckTransactionResponse)> CheckTransactionStatusAsync(this MomoCheckTransactionRequest req,
        string checkTransactionRequestUrl)
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
        var checkTransactionStatusResp = await httpClient.PostAsync(
            requestUri: checkTransactionRequestUrl, 
            content: requestContent);
        
        // Response content
        var content = checkTransactionStatusResp.Content.ReadAsStringAsync().Result;
        var responseData = JsonConvert.DeserializeObject<MomoCheckTransactionResponse>(content);
        // Check for response content not found 
        if (responseData == null) return (false, "Request to server failed. Not found any response data", null!);
        
        // Check whether success
        if (checkTransactionStatusResp.IsSuccessStatusCode)
        {
            // Get result code from request data
            var resultCodeStr = responseData.ResultCode;
            
            // List of transaction status result code
            List<int> statusResultCodes = new()
            {
                int.Parse(MomoResultCodeConstants.Successful),
                int.Parse(MomoResultCodeConstants.WaitingForConfirm),
                int.Parse(MomoResultCodeConstants.InSufficientFunds),
                int.Parse(MomoResultCodeConstants.RejectedByIssuers),
                int.Parse(MomoResultCodeConstants.CancelledByUser),
                int.Parse(MomoResultCodeConstants.OverTransactionTimeLimit),
                int.Parse(MomoResultCodeConstants.QrCodeExpired),
                int.Parse(MomoResultCodeConstants.UserDeniedConfirmPayment),
                int.Parse(MomoResultCodeConstants.TransactionIsAuthorizedSuccessfully),
            };
            
            // Check existing result code
            if (statusResultCodes.Contains(resultCodeStr))
            {
                return (true, string.Empty, responseData);
            }

            // For bad request message
            return (false, responseData.Message, responseData);
        }
        
        // Invoke problem
        return (false, responseData?.Message, null!);
    } 
}