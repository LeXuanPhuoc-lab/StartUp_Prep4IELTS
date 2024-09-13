using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class UserPremiumPackage
{
    public int UserPremiumPackageId { get; set; }

    public Guid UserId { get; set; }

    public int PremiumPackageId { get; set; }

    public DateTime ExpireDate { get; set; }

    public bool IsActive { get; set; }

    public virtual PremiumPackage PremiumPackage { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
