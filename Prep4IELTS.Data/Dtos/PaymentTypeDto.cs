using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record PaymentTypeDto(
    int PaymentTypeId,
    string PaymentMethod)
{
    [JsonIgnore] public ICollection<TransactionDto> Transactions = new List<TransactionDto>();
}