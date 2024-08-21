using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record SystemRoleDto(int RoleId, string? RoleName)
{
    [JsonIgnore] public ICollection<UserDto> Users { get; set; } = new List<UserDto>();
};