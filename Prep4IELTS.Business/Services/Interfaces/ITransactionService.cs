using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ITransactionService
{
    Task<UserPremiumPackageDto?> FindUserPremiumPackageByUserIdAsync(Guid userId);
    Task<bool> CreateAsync(TransactionDto transactionDto);
    Task<bool> UpdateTransactionStatusAsync(
        string transactionCode, DateTime? transactionDate, decimal? paymentAmount,
        string? cancellationReason, DateTime? cancelledAt, TransactionStatus transactionStatus);
    Task<bool> IsExistTransactionByCodeAsync(string transactionCode);
    Task<TransactionDto?> FindTransactionByCodeAsync(string transactionCode);
    Task<List<TransactionDto>> FindAllAsync();
}