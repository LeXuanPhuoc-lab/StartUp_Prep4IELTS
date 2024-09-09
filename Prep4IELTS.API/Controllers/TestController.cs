using EXE202_Prep4IELTS.Attributes;
using EXE202_Prep4IELTS.Payloads;
using EXE202_Prep4IELTS.Payloads.Filters;
using EXE202_Prep4IELTS.Payloads.Requests.Tests;
using EXE202_Prep4IELTS.Payloads.Responses;
using EXE202_Prep4IELTS.Payloads.Responses.Tests;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;
using Serilog;

namespace EXE202_Prep4IELTS.Controllers;

[ApiController]
public class TestController(
    ITestService testService,
    ITestCategoryService testCategoryService,
    IUserService userService,
    ITagService tagService,
    IPartitionTagService partitionTagService,
    IMapper mapper,
    IOptionsMonitor<AppSettings> monitor) : ControllerBase
{
    private readonly AppSettings _appSettings = monitor.CurrentValue;

    //  Summary:
    //      Get all test categories that use to filter test by category
    [HttpGet(ApiRoute.TestCategory.GetAll, Name = nameof(GetAllTestCategoryAsync))]
    public async Task<IActionResult> GetAllTestCategoryAsync()
    {
        // Get all test category
        var testCategoryDtos = await testCategoryService.FindAllAsync();

        return !testCategoryDtos.Any() // Not exist any test category
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any tests."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = testCategoryDtos
            });
    }

    //  Summary:
    //      Get all existing tests 
    [HttpGet(ApiRoute.Test.GetAll, Name = nameof(GetAllTestAsync))]
    public async Task<IActionResult> GetAllTestAsync([FromQuery] TestFilterRequest req)
    {
        // Get all test
        var testDtos = await testService.FindAllWithConditionAndPagingAsync(
            // With conditions
            // Search with test title
            filter: x => x.TestTitle.Contains(req.Term ?? "") &&
                         // Is not draft
                         !x.IsDraft && 
                         // filter with test category name
                         (string.IsNullOrEmpty(req.Category) ||
                          x.TestCategory.TestCategoryName!.Equals(req.Category.Replace("%", " "))),
            orderBy: null,
            // Include Tags
            includeProperties: "Tags",
            // With page index
            pageIndex: req.Page,
            // With page size
            pageSize: req.PageSize ?? _appSettings.PageSize,
            // Include user test histories (if any)
            userId: req.UserId);

        // Total actual tests
        var actualTotal = await testService.CountTotalAsync();

        // Sorting 
        if (!string.IsNullOrEmpty(req.OrderBy))
        {
            var sortingEnumerable = await SortHelper.SortTestByColumnAsync(testDtos, req.OrderBy);
            testDtos = sortingEnumerable.ToList();
        }

        // Create paginated detail list 
        var paginatedDetail = PaginatedDetailList<TestDto>.CreateInstance(testDtos,
            pageIndex: req.Page ?? 1,
            req.PageSize ?? _appSettings.PageSize,
            actualItem: actualTotal);

        return !testDtos.Any() // Not exist any test
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any tests."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Tests = paginatedDetail,
                    Page = paginatedDetail.PageIndex,
                    TotalPage = paginatedDetail.TotalPage
                }
            });
    }

    //  Summary:
    //      Get all existing tests 
    [HttpGet(ApiRoute.Test.GetAllDraft, Name = nameof(GetAllTestDraftAsync))]
    // [ClerkAuthorize(Roles = [SystemRoleConstants.Staff])]
    public async Task<IActionResult> GetAllTestDraftAsync([FromQuery] TestFilterRequest req)
    {
        // Get staff information
        var staffDto = HttpContext.Items["User"] as User;
        // if (staffDto == null) return Unauthorized();
        
        // Get all test (draft) for particular staff
        var testDtos = await testService.FindAllWithConditionAndPagingAsync(
            // With conditions
            // Search with test title
            filter: x => x.TestTitle.Contains(req.Term ?? "") &&
                         // Is not draft
                         x.IsDraft && 
                         // With particular staff id 
                         // x.UserId == staffDto.UserId &&
                         x.UserId == Guid.Parse("EE3DA1EC-E2D1-458C-B6C9-23E76727DA8D") &&
                         // filter with test category name
                         (string.IsNullOrEmpty(req.Category) ||
                          x.TestCategory.TestCategoryName!.Equals(req.Category.Replace("%", " "))),
            orderBy: null,
            // Include Tags
            includeProperties: "Tags",
            // With page index
            pageIndex: req.Page,
            // With page size
            pageSize: req.PageSize ?? _appSettings.PageSize,
            // Include user test histories (if any)
            userId: req.UserId);

        // Total actual tests
        var actualTotal = await testService.CountTotalAsync();

        // Sorting 
        if (!string.IsNullOrEmpty(req.OrderBy))
        {
            var sortingEnumerable = await SortHelper.SortTestByColumnAsync(testDtos, req.OrderBy);
            testDtos = sortingEnumerable.ToList();
        }

        // Create paginated detail list 
        var paginatedDetail = PaginatedDetailList<TestDto>.CreateInstance(testDtos,
            pageIndex: req.Page ?? 1,
            req.PageSize ?? _appSettings.PageSize,
            actualItem: actualTotal);

        return !testDtos.Any() // Not exist any test
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any tests."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    Tests = paginatedDetail,
                    Page = paginatedDetail.PageIndex,
                    TotalPage = paginatedDetail.TotalPage
                }
            });
    }
    
    //  Summary:
    //      Get test by id 
    [HttpGet(ApiRoute.Test.GetById, Name = nameof(GetTestByIdAsync))]
    public async Task<IActionResult> GetTestByIdAsync([FromRoute] int id, [FromQuery] Guid? userId)
    {
        // Get by id 
        var testDto = await testService.FindByIdAsync(id,
            userId != null! ? userId : null!);

        return testDto == null!
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any test match id {id}"
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = testDto
            });
    }

    //  Summary:
    //      Practice test
    [HttpGet(ApiRoute.Test.PracticeById, Name = nameof(PracticeTestByIdAsync))]
    public async Task<IActionResult> PracticeTestByIdAsync([FromRoute] int id, [FromQuery] int[] section)
    {
        var testDto = await testService.FindByIdForPracticeAsync(id, section);

        return testDto == null! // Not exist any test
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any tests."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = testDto
            });
    }

    //  Summary:
    //      Start test
    [HttpGet(ApiRoute.Test.StartTest, Name = nameof(StartTestByIdAsync))]
    public async Task<IActionResult> StartTestByIdAsync([FromRoute] int id)
    {
        var testDto = await testService.FindByIdForTestSimulationAsync(id);

        return testDto == null! // Not exist any test
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any tests."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = testDto
            });
    }

    //  Summary:
    //      Get all answers by test id
    [HttpGet(ApiRoute.Test.GetAllAnswer, Name = nameof(GetAllAnswerByTestIdAsync))]
    public async Task<IActionResult> GetAllAnswerByTestIdAsync([FromRoute] int id, [FromQuery] int[] section)
    {
        // Get test by id and its answers
        var testDto = await testService.FindByIdAndGetAllAnswerAsync(id);

        // Initiate collection of section with questions response
        var sectionAnswerRespList = new List<SectionSolutionResponse>();
        // Loop all section in test to add section name, transcript, questions to section solution resp 
        foreach (var st in testDto.TestSections)
        {
            // Check whether getting answer with particular section
            if (section.Any() && !section.Contains(st.TestSectionId))
                continue; // Skip if section != null && not found in section required

            // Get all answer for each section
            var questionAnswers = st.TestSectionPartitions
                .SelectMany(x => x.Questions.Select(q => new QuestionAnswerDisplayResponse
                {
                    QuestionNumber = q.QuestionNumber,
                    AnswerDisplay = q.QuestionAnswers.First().AnswerDisplay.ToUpper()
                }))
                .ToList();

            // Add to section answer response list
            sectionAnswerRespList.Add(new SectionSolutionResponse()
            {
                SectionName = st.TestSectionName,
                Transcript = st.SectionTranscript ?? string.Empty,
                QuestionAnswers = questionAnswers
            });
        }

        // Initiate list of question answers for test 
        var testAnsResp = new TestSolutionResponse()
        {
            TestTitle = testDto.TestTitle,
            Sections = sectionAnswerRespList
        };

        return testDto == null! // Not exist any test
            ? NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Not found any tests."
            })
            : Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = testAnsResp
            });
    }

    //  Summary:
    //      Test submission
    [HttpPost(ApiRoute.Test.Submission, Name = nameof(TestSubmissionAsync))]
    public async Task<IActionResult> TestSubmissionAsync([FromBody] TestSubmissionRequest req)
    {
        // Check validation errors
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Check exist test 
        var isExistTest = await testService.IsExistTestAsync(req.TestId);
        if (!isExistTest)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any test match id {req.TestId}"
            });
        }

        // Check exist user
        var isExistUser = await userService.IsExistUserAsync(req.UserId);
        if (!isExistUser)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any user match id {req.UserId}"
            });
        }

        // Create history for submission
        var isCreated = await testService.SubmitTestAsync(
            req.TotalCompletionTime, req.TakenDatetime, req.IsFull,
            req.UserId, req.TestId,
            mapper.Map<List<QuestionAnswerSubmissionModel>>(req.QuestionAnswers));

        return isCreated
            ? Created()
            : StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Failed to create the test submission due to a server error."
            });
    }

    //  Summary:
    //      Create Test Detail
    [HttpGet(ApiRoute.Test.GetCreateTestDetail, Name = nameof(GetCreateTestDetailAsync))]
    [ClerkAuthorize(Roles = [SystemRoleConstants.Staff])]
    public async Task<IActionResult> GetCreateTestDetailAsync()
    {
        var testCategories = await testCategoryService.FindAllAsync();
        var tags = await tagService.FindAllAsync();
        var partitionTags = await partitionTagService.FindAllAsync();

        List<string> testTypes = new()
        {
            TestType.Listening.GetDescription(),
            TestType.Reading.GetDescription(),
            TestType.Writing.GetDescription(),
            TestType.Speaking.GetDescription(),
        };

        return Ok(new BaseResponse()
        {
            StatusCode = StatusCodes.Status200OK,
            Data = new
            {
                TestCategories = testCategories,
                Tags = tags,
                PartitionTags = partitionTags,
                TestTypes = testTypes
            }
        });
    }
    
    //  Summary:
    //      Create Test
    [HttpPost(ApiRoute.Test.Create, Name = nameof(CreateTestAsync))]
    [ClerkAuthorize(Roles = [SystemRoleConstants.Staff])]
    public async Task<IActionResult> CreateTestAsync([FromBody] CreateTestRequest req)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        // Get staff information
        var staffDto = HttpContext.Items["User"] as UserDto;
        // if (staffDto == null) return Unauthorized();
        
        // Map request to test entity
        var testEntity = mapper.Map<Test>(req);
        
        // Update created user
        // testEntity.UserId = staffDto.UserId;
        // testEntity.CreateBy = $"{staffDto.FirstName} {staffDto.LastName}";
        testEntity.UserId = Guid.Parse("EE3DA1EC-E2D1-458C-B6C9-23E76727DA8D");
        
        // Mark as Draft
        testEntity.IsDraft = true;
        
        // Get all test sections
        var testSections = testEntity.TestSections.ToList();
        
        // Check valid total question
        var totalQuestionReq = testSections
            .SelectMany(ts => ts.TestSectionPartitions)
            .SelectMany(tsp => tsp.Questions)
            .Count();
        if (totalQuestionReq != req.TotalQuestion)
        {
            ModelState.AddModelError("TotalQuestion", 
                $"Total question is not valid. Number of question input: {req.TotalQuestion}; Actual total: {totalQuestionReq}");
            return BadRequest(ModelState);
        }
        
        // Check valid total section
        var totalSectionReq = testSections.Count;
        if (totalSectionReq != req.TotalSection)
        {
            ModelState.AddModelError("TotalSection", 
                $"Total section is not valid. Number of section input: {req.TotalSection}; Actual total: {totalSectionReq}");
            return BadRequest(ModelState);
        }
        
        // Check requirement for each TestType
        switch (req.TestType)
        {
            // LISTENING TEST
            case TestTypeConstants.Listening:
                // Check for audio resource in each sections
                var totalAudioResource = testSections.Count(ts => 
                    ts.CloudResource != null!);
                
                // Invoke error
                if (totalAudioResource != testSections.Count)
                {
                    ModelState.AddModelError("TestSections", 
                        "Each section of listening test must be include audio resource." +
                        " Please add audio resource for each section");
                    return BadRequest(ModelState);
                }
                break;
            // READING TEST
            case TestTypeConstants.Reading:
                
                // Check for audio resource in each sections
                var totalReadingDesc = testSections.Count(ts => 
                    string.IsNullOrEmpty(ts.ReadingDesc));
                
                // Invoke error
                if (totalReadingDesc != testSections.Count)
                {
                    ModelState.AddModelError("TestSections", 
                        "Each section of reading test must be include reading paragraphs. " +
                        "Please add reading paragraphs for each section");
                    return BadRequest(ModelState);
                }
                break;
        }
        
        // Update Test properties
        testEntity.CreateDate = DateTime.Now;
        testEntity.TotalEngaged = 0;
        
        // Check for resource exists
        var testSectionReq = req.TestSections;
        for (int i = 0; i < testSections.Count; ++i)
        {
            if (testSectionReq[i].CloudResource != null && !string.IsNullOrEmpty(testSectionReq[i].CloudResource?.Url))
            {
                testSections[i].CloudResource = new CloudResource()
                {
                    PublicId = testSectionReq[i].CloudResource!.PublicId,
                    Url = testSectionReq[i].CloudResource?.Url!,
                    CreateDate = DateTime.Now
                };
            }
            
            var testSectionPartitionReq = testSectionReq[i].TestSectionPartitions;
            var testSectionPartitions = testSections[i].TestSectionPartitions.ToList();
            for (int j = 0; j < testSectionPartitions.Count; ++j)
            {
                if (testSectionPartitionReq[j].CloudResource != null && !string.IsNullOrEmpty(testSectionPartitionReq[j].CloudResource?.Url))
                {
                    testSectionPartitions[j].CloudResource = new CloudResource()
                    {
                        PublicId = testSectionPartitionReq[j].CloudResource!.PublicId,
                        Url = testSectionPartitionReq[j].CloudResource?.Url!,
                        CreateDate = DateTime.Now
                    };
                }
            }
        }
        
        // Clear tags inside test
        testEntity.Tags.Clear();
        
        // Insert listening test
        var isCreateSuccess = await testService.InsertAsync(mapper.Map<TestDto>(testEntity), req.Tags);

        return isCreateSuccess
            ? Created()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }
    
    //  Summary:
    //      Update Test
    [HttpPut(ApiRoute.Test.Update, Name = nameof(UpdateTestAsync))]
    [ClerkAuthorize(Roles = [SystemRoleConstants.Staff])]
    public async Task<IActionResult> UpdateTestAsync([FromRoute] Guid id,[FromBody] UpdateTestRequest req)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        // Check exist test 
        var isExistTest = await testService.IsExistTestAsync(id);
        if (!isExistTest)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any test match id {id}"
            });
        }
        
        // Check test status
        var isPublished = await testService.IsPublishedAsync(id);
        if (isPublished)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Test must be hidden to progress update"
            });
        }
        
        // Mapping to testDto
        var testToUpdate = mapper.Map<Test>(req);
        // Test id
        testToUpdate.TestId = id;
        
        // Progress update test
        var isUpdatedSuccess = await testService.UpdateAsync(testToUpdate.Adapt<TestDto>(), req.Tags);
        
        return isUpdatedSuccess
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }
    
    //  Summary:
    //      Delete Test
    [HttpDelete(ApiRoute.Test.Delete, Name = nameof(DeleteTestAsync))]
    [ClerkAuthorize(Roles = [SystemRoleConstants.Staff])]
    public async Task<IActionResult> DeleteTestAsync([FromRoute] Guid id)
    {
        // Check exist test 
        var isExistTest = await testService.IsExistTestAsync(id);
        if (!isExistTest)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any test match id {id}"
            });
        }
        
        // Check test status
        var isPublished = await testService.IsPublishedAsync(id);
        if (isPublished)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Test must be hidden to progress delete"
            });
        }
        
        // Progress delete test 
        var isDeleted = await testService.RemoveAsync(id);

        return isDeleted 
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }
    
    //  Summary:
    //      Publish test
    [HttpPatch(ApiRoute.Test.Publish, Name = nameof(PublishTestAsync))]
    [ClerkAuthorize(Roles = [SystemRoleConstants.Staff])]
    public async Task<IActionResult> PublishTestAsync([FromRoute] Guid id)
    {
        // Check existing test
        var isExistTest = await testService.IsExistTestAsync(id);
        if (!isExistTest)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any test match id {id}"
            });
        }
        
        // Check test status
        var isPublished = await testService.IsPublishedAsync(id);
        if (isPublished)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Test already published"
            });
        }
        
        // Progress publish test
        var isPublishedSuccess = await testService.PublishTestAsync(id);
        
        return isPublishedSuccess
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }
    
    //  Summary:
    //      Hidden test
    [HttpPatch(ApiRoute.Test.Hidden, Name = nameof(HideTestAsync))]
    [ClerkAuthorize(Roles = [SystemRoleConstants.Staff])]
    public async Task<IActionResult> HideTestAsync([FromRoute] Guid id)
    {
        // Check existing test
        var isExistTest = await testService.IsExistTestAsync(id);
        if (!isExistTest)
        {
            return NotFound(new BaseResponse()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = $"Not found any test match id {id}"
            });
        }
        
        // Check test status
        var isPublished = await testService.IsPublishedAsync(id);
        if (!isPublished)
        {
            return BadRequest(new BaseResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Test already hidden"
            });
        }
        
        // Progress publish test
        var isHideSuccess = await testService.HideTestAsync(id);
        
        return isHideSuccess
            ? NoContent()
            : StatusCode(StatusCodes.Status500InternalServerError);
    }
}