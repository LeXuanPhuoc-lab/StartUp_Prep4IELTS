using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public partial class User
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string ClerkId { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

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

    [JsonIgnore]
    public virtual UserPremiumPackage? UserPremiumPackage { get; set; }

    public virtual ICollection<UserSpeakingSampleHistory> UserSpeakingSampleHistories { get; set; } = new List<UserSpeakingSampleHistory>();
}
