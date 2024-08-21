using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record CommentDto(
    int CommentId,
    DateTime CommentDate,
    string Content,
    int Level,
    int TotalChildNode,
    Guid UserId, UserDto User,
    ICollection<CommentDto> InverseParentComment)
{
    public int? ParentCommentId { get; set; }
    [JsonIgnore] public CommentDto ParentComment { get; set; } = null!;
    
    public Guid TestId { get; set; }
    [JsonIgnore] public TestDto Test { get; set; } = null!;
};
