using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public Guid UserId { get; set; }

    public int UserPremiumPackageId { get; set; }

    public decimal PaymentAmount { get; set; }

    public string TransactionStatus { get; set; } = null!;

    public DateTime TransactionDate { get; set; }

    public int PaymentTypeId { get; set; }

    public virtual PaymentType PaymentType { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual UserPremiumPackage UserPremiumPackage { get; set; } = null!;
}
