using Mapster;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

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
        config.NewConfig<FlashcardDetail, FlashcardDetailDto>();
        config.NewConfig<UserFlashcard, UserFlashcardDto>();
        config.NewConfig<UserFlashcardProgress, UserFlashcardProgressDto>();
        config.NewConfig<Transaction, TransactionDto>();
        config.NewConfig<PaymentType, PaymentTypeDto>();
        config.NewConfig<PremiumPackage, PremiumPackageDto>();
        config.NewConfig<UserPremiumPackage, UserPremiumPackageDto>();
        config.NewConfig<SpeakingSample, SpeakingSampleDto>();
        config.NewConfig<SpeakingPart, SpeakingPartDto>();
        config.NewConfig<UserSpeakingSampleHistory, UserSpeakingSampleHistoryDto>();
        config.NewConfig<FlashcardDetailTag, FlashcardDetailTagDto>();
        config.NewConfig<FlashcardExamHistory, FlashcardExamHistoryDto>();
        config.NewConfig<FlashcardExamGrade, FlashcardExamGradeDto>();
        config.NewConfig<VocabularyUnitSchedule, VocabularyUnitScheduleDto>();
        // config.NewConfig<FlashcardDetail, FlashcardDetailDto>();
    }
}