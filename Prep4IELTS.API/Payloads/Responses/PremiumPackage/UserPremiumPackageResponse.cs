namespace EXE202_Prep4IELTS.Payloads.Responses.PremiumPackage;

public class 
    UserPremiumPackageResponse
{
    public int PremiumPackageId { get; set; }
    public string PremiumPackageName { get; set; } = null!;
    public DateTime CreateDate { get; set; }
    public string? Description { get; set; }
    public DateTime ExpireDate { get; set; }
    public bool IsPremiumActive { get; set; }
    public int? TotalTrials { get; set; }
}