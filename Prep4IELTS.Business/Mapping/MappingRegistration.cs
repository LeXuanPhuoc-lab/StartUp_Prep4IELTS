using Mapster;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace EXE202_Prep4IELTS.Mapping;

public class MappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Test, TestDto>();
        config.NewConfig<TestSection, TestSectionDto>();
        config.NewConfig<TestHistory, TestHistoryDto>();
        config.NewConfig<TestCategory, TestCategoryDto>();
        config.NewConfig<TestCategory, TestCategoryDto>();
        config.NewConfig<Tag, TagDto>();
        config.NewConfig<Comment, CommentDto>();
        config.NewConfig<SystemRole, SystemRoleDto>();
    }
}