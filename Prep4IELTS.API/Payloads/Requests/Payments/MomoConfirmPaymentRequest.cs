namespace EXE202_Prep4IELTS.Payloads.Requests.Payments;

public class MomoConfirmPaymentRequest
{
    public string OrderId { get; set; } = String.Empty;
    // Capture / Cancel
    public string RequestType { get; set; } = string.Empty;
    public long Amount { get; set; }
    public string Description { get; set; } = string.Empty;
}