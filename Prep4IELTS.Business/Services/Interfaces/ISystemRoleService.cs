using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Enum;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ISystemRoleService
{
    Task<SystemRoleDto> FindByRoleNameAsync(SystemRole systemRole);
    Task<IList<SystemRoleDto>> FindAllAsync();
    Task<bool> IsExistRoleAsync(int roleId);
}