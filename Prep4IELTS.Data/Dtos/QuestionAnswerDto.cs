using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record QuestionAnswerDto(int QuestionAnswerId, string AnswerDisplay, string AnswerText, bool IsTrue)
{
    public int QuestionId { get; set; }
    [JsonIgnore] public QuestionDto Question { get; set; } = null!;
};