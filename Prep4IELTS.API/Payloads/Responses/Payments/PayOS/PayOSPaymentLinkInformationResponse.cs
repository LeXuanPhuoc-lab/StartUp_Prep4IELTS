using Prep4IELTS.Business.Models;

namespace EXE202_Prep4IELTS.Payloads.Responses.Payments.PayOS;

public class PayOSPaymentLinkInformationResponse
{
    public string Code { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public PayOSPaymentLinkInformationData Data { get; set; } = null!;
    public string Signature { get; set; } = string.Empty;
}