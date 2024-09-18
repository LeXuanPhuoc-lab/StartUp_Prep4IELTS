using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;

namespace Prep4IELTS.Business.Services;

public class TransactionService(UnitOfWork unitOfWork) : ITransactionService
{
    public async Task<UserPremiumPackageDto?> FindUserPremiumPackageByUserIdAsync(Guid userId)
    {
        var userPremiumPackageEntity =
            await unitOfWork.TransactionRepository.FindUserPremiumPackageByUserIdAsync(userId);
        return userPremiumPackageEntity.Adapt<UserPremiumPackageDto>();
    }

    public async Task<bool> CreateAsync(TransactionDto transactionDto)
    {
        await unitOfWork.TransactionRepository.InsertAsync(transactionDto.Adapt<Transaction>());
        return await unitOfWork.TransactionRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> UpdateTransactionStatusAsync(string transactionCode, DateTime? transactionDate, decimal? paymentAmount,
        string? cancellationReason, DateTime? cancelledAt, TransactionStatus transactionStatus)
    {
        return await unitOfWork.TransactionRepository.UpdateTransactionStatusAsync(
            transactionCode, transactionDate, paymentAmount, cancellationReason, cancelledAt, transactionStatus);
    }

    public async Task<bool> IsExistTransactionByCodeAsync(string transactionCode)
    {
        return await unitOfWork.TransactionRepository.IsExistTransactionByCodeAsync(transactionCode);
    }

    public async Task<TransactionDto?> FindTransactionByCodeAsync(string transactionCode)
    {
        var transactionEntity =
            await unitOfWork.TransactionRepository.FindTransactionByCodeAsync(transactionCode);
        return transactionEntity.Adapt<TransactionDto>();
    }
}