using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Repositories;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Prep4IELTS.Data;

public class UnitOfWork : IDisposable
{
    private Prep4IeltsContext _unitOfWorkContext;
    
    public UnitOfWork(Prep4IeltsContext unitOfWorkContext)
    {
        _unitOfWorkContext = unitOfWorkContext;
    }
    
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

    #region Others
    private CommentRepository _commentRepository = null!;
    private FlashcardRepository _flashcardRepository = null!;
    private FlashcardDetailRepository _flashcardDetailRepository = null!;
    #endregion

    #region Repositories
    
    public UserRepository UserRepository
        => _userRepository ??= new (_unitOfWorkContext);

    public SystemRoleRepository SystemRoleRepository
        => _systemRoleRepository ??= new(_unitOfWorkContext);
    
    public TestRepository TestRepository
        => _testRepository ??= new(_unitOfWorkContext);
    
    public TestSectionRepository TestSectionRepository
        => _testSectionRepository ??= new(_unitOfWorkContext);

    public TestSectionPartitionRepository TestSectionPartitionRepository
        => _testSectionPartitionRepository ??= new(_unitOfWorkContext);

    public TestCategoryRepository TestCategoryRepository
        => _testCategoryRepository ??= new(_unitOfWorkContext);

    public TestHistoryRepository TestHistoryRepository
        => _testHistoryRepository ??= new(_unitOfWorkContext);

    public PartitionHistoryRepository PartitionHistoryRepository
        => _partitionHistoryRepository ??= new(_unitOfWorkContext);

    public TestGradeRepository TestGradeRepository
        => _testGradeRepository ??= new(_unitOfWorkContext);

    public QuestionRepository QuestionRepository
        => _questionRepository ??= new(_unitOfWorkContext);
    
    public QuestionAnswerRepository QuestionAnswerRepository
        => _questionAnswerRepository ??= new(_unitOfWorkContext);

    public TagRepository TagRepository
        => _tagRepository ??= new(_unitOfWorkContext);

    public PartitionTagRepository PartitionTagRepository
        => _partitionTagRepository ??= new(_unitOfWorkContext);
        
    public CommentRepository CommentRepository
        => _commentRepository ??= new(_unitOfWorkContext);
    
    public FlashcardRepository FlashcardRepository
        => _flashcardRepository ??= new(_unitOfWorkContext);
    
    public FlashcardDetailRepository FlashcardDetailRepository
        => _flashcardDetailRepository ??= new(_unitOfWorkContext);
    
    #endregion
    
    public void Dispose()
    {
        if (_unitOfWorkContext == null) return;

        if (_unitOfWorkContext.Database.GetDbConnection().State == ConnectionState.Open)
        {
            _unitOfWorkContext.Database.GetDbConnection().Close();
        }
        _unitOfWorkContext.Dispose();
        _unitOfWorkContext = null!;
    }
}