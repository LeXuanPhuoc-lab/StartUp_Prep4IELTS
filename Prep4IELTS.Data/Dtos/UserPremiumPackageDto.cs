
using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record UserPremiumPackageDto(
    int UserPremiumPackageId,
    DateTime ExpireDate,
    bool IsActive,
    Guid UserId, UserDto User,
    int PremiumPackageId,
    PremiumPackageDto PremiumPackage)
{
    [JsonIgnore] public ICollection<TransactionDto> Transactions = new List<TransactionDto>();
};
