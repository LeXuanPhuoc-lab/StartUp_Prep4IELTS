using System.ComponentModel;

namespace Prep4IELTS.Data.Enum;

public enum TransactionStatus
{
    [Description("PAID")]
    Paid,
    [Description("PENDING")]
    Pending,
    [Description("EXPIRED")]
    Expired,
    [Description("CANCELLED")]
    Cancelled
}