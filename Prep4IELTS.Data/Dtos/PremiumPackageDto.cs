using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record PremiumPackageDto(
    int PremiumPackageId,
    string PremiumPackageName,
    decimal Price,
    int DurationInMonths,
    bool IsActive,
    DateTime CreateDate,
    string? Description,
    string PackageType)
{
    [JsonIgnore] public ICollection<UserPremiumPackageDto>? UserPremiumPackages = new List<UserPremiumPackageDto>();
};