using System.ComponentModel.DataAnnotations;

namespace EXE202_Prep4IELTS.Payloads.Requests.PremiumPackage;

public class UpdatePremiumPackageRequest
{
    [Required]
    [StringLength(maximumLength: 100, MinimumLength = 3, ErrorMessage = "Premium package name must be between 3 and 100 characters")]
    [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = "Premium package name cannot contain special characters")]
    public string PremiumPackageName { get; set; } = null!;

    public int DurationInMonths { get; set; }

    [Required] 
    [MaxLength(length: 1000, ErrorMessage = "Premium package description must be less than 1000 characters")]
    public string Description { get; set; } = null!;
    
    public decimal Price { get; set; } = 0;
}