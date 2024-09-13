namespace EXE202_Prep4IELTS.Payloads.Responses.Payments.Momo;

public class MomoCheckTransactionResponse
{
    public string PartnerCode { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string ExtraData { get; set; } = string.Empty;
    public long Amount { get; set; } 
    public long TransId { get; set; }
    public string PayType { get; set; } = string.Empty;
    public int ResultCode { get; set; }
    public List<string> RefundTrans { get; set; } = new();
    public string Message { get; set; } = string.Empty;
    public long ResponseTime { get; set; }
    public string? PaymentOption { get; set; } = string.Empty;
}