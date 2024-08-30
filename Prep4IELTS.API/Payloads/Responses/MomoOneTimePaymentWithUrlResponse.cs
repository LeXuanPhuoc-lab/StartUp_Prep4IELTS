using System.ComponentModel.DataAnnotations;

namespace EXE202_Prep4IELTS.Payloads.Responses;

public class MomoOneTimePaymentWithUrlResponse
{
    public string PartnerCode { get; set; } = string.Empty;   
    public string RequestId { get; set; } = string.Empty;   
    public long Amount { get; set; }  
    public string OrderId { get; set; } = string.Empty;
    public long ResponseTime { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ResultCode { get; set; }
    public string PayUrl { get; set; } = string.Empty;
    public string DeepLink { get; set; } = string.Empty;
    public string QrCodeUrl { get; set; } = string.Empty;
}