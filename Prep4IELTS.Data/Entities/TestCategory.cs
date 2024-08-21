using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class TestCategory
{
    public int TestCategoryId { get; set; }

    public string? TestCategoryName { get; set; }

    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
