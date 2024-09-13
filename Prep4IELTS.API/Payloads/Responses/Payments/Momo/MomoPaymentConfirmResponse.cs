namespace EXE202_Prep4IELTS.Payloads.Responses.Payments.Momo;

public class MomoPaymentConfirmResponse
{
    public string PartnerCode { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public long Amount { get; set; }
    public long TransId { get; set; }
    public int ResultCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public long ResponseTime { get; set; } 
}