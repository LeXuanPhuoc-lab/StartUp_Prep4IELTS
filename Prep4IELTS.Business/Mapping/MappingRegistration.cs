using Mapster;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using FlashcardDetail = Prep4IELTS.Data.Dtos.FlashcardDetail;

namespace EXE202_Prep4IELTS.Mapping;

public class MappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Test, TestDto>();
        config.NewConfig<TestSection, TestSectionDto>();
        config.NewConfig<TestSectionPartition, TestSectionPartitionDto>();
        config.NewConfig<TestHistory, TestHistoryDto>();
        config.NewConfig<TestCategory, TestCategoryDto>();
        config.NewConfig<TestGrade, TestGradeDto>();
        config.NewConfig<Tag, TagDto>();
        config.NewConfig<PartitionTag, PartitionTagDto>();
        config.NewConfig<Comment, CommentDto>();
        config.NewConfig<SystemRole, SystemRoleDto>();
        config.NewConfig<CloudResource, CloudResourceDto>();
        config.NewConfig<Question, QuestionDto>();
        config.NewConfig<QuestionAnswer, QuestionAnswerDto>();
        config.NewConfig<ScoreCalculation, ScoreCalculationDto>();
        config.NewConfig<SystemRole, SystemRoleDto>();
        config.NewConfig<User, UserDto>();
        config.NewConfig<Flashcard, FlashcardDto>();
        // config.NewConfig<FlashcardDetail, FlashcardDetailDto>();
    }
}