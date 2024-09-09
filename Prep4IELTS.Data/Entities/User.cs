using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
    
    [JsonIgnore]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [JsonIgnore]
    public virtual ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();

    public virtual SystemRole? Role { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();
    
    [JsonIgnore]
    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
