namespace Prep4IELTS.Business.Models;

public class PayOSPaymentLinkInformationData
{
    public string Id { get; set; } = string.Empty;
    public string OrderCode { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal AmountRemaining { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? CreatedAt { get; set; }
    public List<PayOSTransaction> Transactions { get; set; } = new();
    public string? CancellationReason { get; set; } = string.Empty;
    public string? CanceledAt { get; set; } 
}