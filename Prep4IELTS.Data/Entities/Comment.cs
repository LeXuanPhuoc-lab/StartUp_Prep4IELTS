using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class Comment
{
    public int CommentId { get; set; }

    public DateTime CommentDate { get; set; }

    public string Content { get; set; } = null!;

    public int Level { get; set; }

    public int TotalChildNode { get; set; }

    public Guid UserId { get; set; }

    public Guid TestId { get; set; }

    public int? ParentCommentId { get; set; }

    public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

    public virtual Comment? ParentComment { get; set; }

    public virtual Test Test { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
