using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class User
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string ClerkId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string? Phone { get; set; }

    public string? AvatarImage { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? TestTakenDate { get; set; }

    public string? TargetScore { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual SystemRole? Role { get; set; }

    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<UserFlashcard> UserFlashcards { get; set; } = new List<UserFlashcard>();

    public virtual UserPremiumPackage? UserPremiumPackage { get; set; }

    public virtual ICollection<UserSpeakingSampleHistory> UserSpeakingSampleHistories { get; set; } = new List<UserSpeakingSampleHistory>();
}
