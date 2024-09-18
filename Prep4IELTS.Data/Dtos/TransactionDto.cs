namespace Prep4IELTS.Data.Dtos;

public record TransactionDto(
    int TransactionId, 
    string TransactionCode, 
    string? PaymentLinkId, 
    decimal PaymentAmount, 
    string TransactionStatus, 
    DateTime CreateAt,
    DateTime? TransactionDate, 
    string? CancellationReason,
    DateTime? CancelledAt,
    Guid UserId, UserDto User,
    int UserPremiumPackageId, UserPremiumPackageDto UserPremiumPackage,
    int PaymentTypeId, PaymentTypeDto PaymentType);
