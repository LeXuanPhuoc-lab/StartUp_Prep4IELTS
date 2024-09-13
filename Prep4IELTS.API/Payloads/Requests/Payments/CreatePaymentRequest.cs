using System.ComponentModel.DataAnnotations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Payments;

public class CreatePaymentRequest
{
    [Required]
    public string PaymentIssuer { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(length: 2, ErrorMessage = "Language of message must be (vi or en).")]
    public string Lang { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string? Name { get; set; } = string.Empty;
    
    [Range(minimum: 1000, maximum: 50_000_000, ErrorMessage = "Amount needs to be paid Min: 1.000 VND or Max: 50.000.000 VND")]
    public long Amount { get; set; } 
    
    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    // [Required]
    public string? RequestType { get; set; } = string.Empty;
    
    // With display method, FE handle this
}