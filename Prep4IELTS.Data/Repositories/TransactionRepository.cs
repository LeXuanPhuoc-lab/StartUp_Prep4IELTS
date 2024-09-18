using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;

namespace Prep4IELTS.Data.Repositories;

public class TransactionRepository : GenericRepository<Transaction>
{
    public TransactionRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }

    public async Task<UserPremiumPackage?> FindUserPremiumPackageByUserIdAsync(Guid userId)
    {
        var userPremiumPackageEntity =
            await DbContext.UserPremiumPackages
                .FirstOrDefaultAsync(t => t.UserId == userId);
    
        // if(userPremiumPackageEntity != null) userPremiumPackageEntity.User = null!;
        
        return userPremiumPackageEntity;
    }

    public async Task<bool> CheckIsExpiredTransactionByCodeAsync(string transactionCode)
    {
        var transactionEntity = await _dbSet
            .OrderBy(t => t.TransactionId)
            .FirstOrDefaultAsync(t => t.TransactionCode == transactionCode);
        return transactionEntity != null && transactionEntity.TransactionStatus.Equals(TransactionStatus.Expired.GetDescription());
    }
    
    public async Task<bool> UpdateTransactionStatusAsync(string transactionCode, DateTime? transactionDate, decimal? paymentAmount,
        string? cancellationReason, DateTime? cancelledAt, TransactionStatus transactionStatus)
    {
        var transactionEntity = await _dbSet
            .Include(t => t.UserPremiumPackage)
            .FirstOrDefaultAsync(t => t.TransactionCode == transactionCode);

        if (transactionEntity == null) return false;
        
        // Progress update transaction entity
        switch (transactionStatus)
        {
            case TransactionStatus.Pending:
                transactionEntity.TransactionStatus = TransactionStatus.Pending.GetDescription();
                break;
            case TransactionStatus.Expired:
                transactionEntity.TransactionStatus = TransactionStatus.Expired.GetDescription();
                // Remove transaction
                break;
            case TransactionStatus.Paid:
                // Update transaction status
                transactionEntity.TransactionStatus = TransactionStatus.Paid.GetDescription();
                // Update transaction payment datetime
                transactionEntity.TransactionDate = transactionDate;
                // Update payment amount
                transactionEntity.PaymentAmount = paymentAmount ?? 0;
                // Update user premium package status
                transactionEntity.UserPremiumPackage.IsActive = true;
                break;
            case TransactionStatus.Cancelled:
                transactionEntity.TransactionStatus = TransactionStatus.Cancelled.GetDescription();
                transactionEntity.CancellationReason = cancellationReason;
                transactionEntity.CancelledAt = cancelledAt;
                break;
        }

        return await SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> IsExistTransactionByCodeAsync(string transactionCode)
    {
        return await _dbSet.AnyAsync(t => t.TransactionCode.Equals(transactionCode));
    }

    public async Task<Transaction?> FindTransactionByCodeAsync(string transactionCode)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.TransactionCode.Equals(transactionCode));
    }
}