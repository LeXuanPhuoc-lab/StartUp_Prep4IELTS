using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public partial class PaymentType
{
    public int PaymentTypeId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
