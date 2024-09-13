namespace EXE202_Prep4IELTS.Payloads.Responses.Payments.Momo;

public class MomoInitiateTransactionResponse
{
    public string PartnerCode { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public long Amount { get; set; } 
    public long ResponseTime { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ResultCode { get; set; }
    public string PartnerClientId { get; set; } = string.Empty;
    public string MToken { get; set; } = string.Empty;
}