using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class Transaction
{
    public int TransactionId { get; set; }
    public Guid UserId { get; set; }
    public int UserPremiumPackageId { get; set; }
    public string TransactionCode { get; set; } = string.Empty;
    public string? PaymentLinkId { get; set; } 
    public decimal PaymentAmount { get; set; }
    public string TransactionStatus { get; set; } = null!;
    public DateTime? TransactionDate { get; set; }
    public DateTime CreateAt { get; set; }
    public string? CancellationReason { get; set; } 
    public DateTime? CancelledAt { get; set; }
    public int PaymentTypeId { get; set; }
    public virtual PaymentType PaymentType { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual UserPremiumPackage UserPremiumPackage { get; set; } = null!;
}
