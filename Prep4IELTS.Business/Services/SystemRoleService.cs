using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Enum;

namespace Prep4IELTS.Business.Services;

public class SystemRoleService(UnitOfWork unitOfWork) : ISystemRoleService
{
    public async Task<SystemRoleDto> FindByRoleNameAsync(SystemRole systemRole)
    {
        var systemRoleEntity = await unitOfWork.SystemRoleRepository.FindByRoleNameAsync(systemRole);
        return systemRoleEntity.Adapt<SystemRoleDto>();
    }

    public async Task<IList<SystemRoleDto>> FindAllAsync()
    {
        var roleEntities = await unitOfWork.SystemRoleRepository.FindAllAsync();
        return roleEntities.Adapt<List<SystemRoleDto>>();
    }

    public async Task<bool> IsExistRoleAsync(int roleId)
    {
        return await unitOfWork.SystemRoleRepository.IsExistRoleAsync(roleId);
    }
}