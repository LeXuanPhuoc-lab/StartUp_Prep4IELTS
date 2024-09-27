using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Repositories;

namespace Prep4IELTS.Data;

public class UnitOfWork(Prep4IeltsContext unitOfWorkContext) : IDisposable
{
    #region User Management
    private UserRepository _userRepository = null!;
    private SystemRoleRepository _systemRoleRepository = null!;
    #endregion
    
    # region Test Related
    private TestRepository _testRepository = null!;
    private TestSectionRepository _testSectionRepository = null!;
    private TestSectionPartitionRepository _testSectionPartitionRepository = null!;
    private TestCategoryRepository _testCategoryRepository = null!;
    #endregion

    #region History Related
    private TestHistoryRepository _testHistoryRepository = null!;
    private PartitionHistoryRepository _partitionHistoryRepository = null!;
    private TestGradeRepository _testGradeRepository = null!;
    #endregion
    
    #region Question Related
    private QuestionRepository _questionRepository = null!;
    private QuestionAnswerRepository _questionAnswerRepository = null!;
    #endregion

    #region Tag Related
    private TagRepository _tagRepository = null!;
    private PartitionTagRepository _partitionTagRepository = null!;
    #endregion

    #region Payment Related
    private PremiumPackageRepository _premiumPackageRepository = null!;
    private TransactionRepository _transactionRepository = null!;
    private UserPremiumPackageRepository _userPremiumPackageRepository = null!;
    private PaymentTypeRepository _paymentTypeRepository = null!;
    #endregion

    #region Speaking

    private SpeakingSampleRepository _speakingSampleRepository = null!;
    #endregion
    
    #region Others
    private CommentRepository _commentRepository = null!;
    private FlashcardRepository _flashcardRepository = null!;
    private FlashcardDetailRepository _flashcardDetailRepository = null!;
    private UserFlashcardRepository _userFlashcardRepository = null!;
    private ScoreCalculationRepository _scoreCalculationRepostiroy = null!;
    #endregion

    #region Repositories
    
    public UserRepository UserRepository
        => _userRepository ??= new (unitOfWorkContext);

    public SystemRoleRepository SystemRoleRepository
        => _systemRoleRepository ??= new(unitOfWorkContext);
    
    public TestRepository TestRepository
        => _testRepository ??= new(unitOfWorkContext);
    
    public TestSectionRepository TestSectionRepository
        => _testSectionRepository ??= new(unitOfWorkContext);

    public TestSectionPartitionRepository TestSectionPartitionRepository
        => _testSectionPartitionRepository ??= new(unitOfWorkContext);

    public TestCategoryRepository TestCategoryRepository
        => _testCategoryRepository ??= new(unitOfWorkContext);

    public TestHistoryRepository TestHistoryRepository
        => _testHistoryRepository ??= new(unitOfWorkContext);

    public PartitionHistoryRepository PartitionHistoryRepository
        => _partitionHistoryRepository ??= new(unitOfWorkContext);

    public TestGradeRepository TestGradeRepository
        => _testGradeRepository ??= new(unitOfWorkContext);

    public QuestionRepository QuestionRepository
        => _questionRepository ??= new(unitOfWorkContext);
    
    public QuestionAnswerRepository QuestionAnswerRepository
        => _questionAnswerRepository ??= new(unitOfWorkContext);

    public TagRepository TagRepository
        => _tagRepository ??= new(unitOfWorkContext);

    public PartitionTagRepository PartitionTagRepository
        => _partitionTagRepository ??= new(unitOfWorkContext);
        
    public CommentRepository CommentRepository
        => _commentRepository ??= new(unitOfWorkContext);
    
    public FlashcardRepository FlashcardRepository
        => _flashcardRepository ??= new(unitOfWorkContext);
    
    public FlashcardDetailRepository FlashcardDetailRepository
        => _flashcardDetailRepository ??= new(unitOfWorkContext);
    
    public ScoreCalculationRepository ScoreCalculationRepository
        => _scoreCalculationRepostiroy ??= new(unitOfWorkContext);
    
    public PremiumPackageRepository PremiumPackageRepository
        => _premiumPackageRepository ??= new(unitOfWorkContext);
    
    public TransactionRepository TransactionRepository
        => _transactionRepository ??= new(unitOfWorkContext);
    
    public UserPremiumPackageRepository UserPremiumPackageRepository
        => _userPremiumPackageRepository ??= new(unitOfWorkContext);
    
    public PaymentTypeRepository PaymentTypeRepository
        => _paymentTypeRepository ??= new(unitOfWorkContext);
    
    public SpeakingSampleRepository SpeakingSampleRepository
        => _speakingSampleRepository ??= new(unitOfWorkContext);
    
    public UserFlashcardRepository UserFlashcardRepository
        => _userFlashcardRepository ??= new(unitOfWorkContext);
    #endregion
    
    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                unitOfWorkContext.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}