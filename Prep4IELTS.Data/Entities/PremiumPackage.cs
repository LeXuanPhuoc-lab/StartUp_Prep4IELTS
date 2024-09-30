using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class PremiumPackage
{
    public int PremiumPackageId { get; set; }

    public string PremiumPackageName { get; set; } = null!;

    public decimal Price { get; set; }

    public int DurationInMonths { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreateDate { get; set; }

    public string? Description { get; set; }

    public string PackageType { get; set; } = null!;

    public virtual ICollection<UserPremiumPackage> UserPremiumPackages { get; set; } = new List<UserPremiumPackage>();
}
