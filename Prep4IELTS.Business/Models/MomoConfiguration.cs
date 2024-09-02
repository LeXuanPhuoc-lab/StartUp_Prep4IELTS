namespace Prep4IELTS.Business.Models;

public class MomoConfiguration
{
    public string PaymentMethodName { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    // public string ReturnUrl { get; set; } = string.Empty;
    public string IpnUrl { get; set; } = string.Empty;
    public string PaymentUrl { get; set; } = string.Empty;
    public string CheckTransactionUrl { get; set; } = string.Empty;
    public string PaymentConfirmUrl { get; set; } = string.Empty;
    public string InitiateTransactionUrl { get; set; } = string.Empty;
    public string PartnerCode { get; set; } = string.Empty;
}