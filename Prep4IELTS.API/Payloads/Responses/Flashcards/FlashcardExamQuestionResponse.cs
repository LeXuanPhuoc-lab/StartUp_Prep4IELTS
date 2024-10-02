using Prep4IELTS.Business.Constants;
using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Responses.Flashcards;

public class FlashcardExamQuestionResponse
{
    public int FlashcardDetailId { get; set; }

    public int QuestionNumber { get; set; }
    
    public string QuestionTitle { get; set; } = string.Empty;

    public string? QuestionDesc { get; set; } = null!;

    public List<FlashcardExamQuestionAnswerResponse> QuestionAnswers { get; set; } = new();

    public List<FlashcardExamQuestionResponse> MatchingQuestions { get; set; } = new();
    
    // TRUE/FALSE - MULTIPLE CHOICE - WRITTEN - MATCHING QUESTION
    public string QuestionType { get; set; } = string.Empty;
}


public static class FlashcardExamQuestionResponseExtensions
{
    public static List<FlashcardExamQuestionResponse> GenerateFlashcardExamQuestions(
        this List<FlashcardDetailDto> flashcardDetails, int totalQuestion, bool isTermPattern,
        List<string> questionTypes)
    {
        // Check if question types is empty
        if(!questionTypes.Any()) questionTypes = new ()
        {
            // Include all question types when user not select
            FlashcardExamTypeConstants.MultipleChoice,
            FlashcardExamTypeConstants.MatchingQuestion,
            FlashcardExamTypeConstants.Written,
            FlashcardExamTypeConstants.TrueFalse,
        };
     
        // Initiate random variable
        var rnd = new Random();
        // Initiate list of question exam
        var flashcardExamQuestions = new List<FlashcardExamQuestionResponse>();
        
        // Select randomly from flashcard details with specific total question 
        var flashcardDetailRandomList = flashcardDetails
             // Shuffle the list
            .OrderBy(x => rnd.Next()) 
            // With particular length
            .Take(totalQuestion)
            .ToList();
        // Initiate question answers for all type (TF, Matching, Multi, Written)
        var questionAnswers = flashcardDetails.Select(fd => 
                new FlashcardExamQuestionAnswerResponse
            {
                AnswerText = isTermPattern 
                    // Check if question is term pattern ? Definition is answer : Word text is answer
                    ? fd.Definition : fd.WordText, 
                ImageUrl = fd.CloudResource?.Url
            })
            .ToList();
        
        // Handle [Matching Question] type only
        if (questionTypes.Count == 1 
            && questionTypes.First() == FlashcardExamTypeConstants.MatchingQuestion)
        {
            // Get all answer from random question list   
            var answersForMatchingExam = flashcardDetailRandomList.Select(fd =>
                    new
                    {
                        FlashcardDetailId = fd.FlashcardDetailId,
                        Answer = new FlashcardExamQuestionAnswerResponse
                        {
                            AnswerText = isTermPattern 
                                // Check if question is term pattern ? Definition is answer : Word text is answer
                                ? fd.Definition : fd.WordText, 
                            ImageUrl = fd.CloudResource?.Url
                        }
                    })
                .ToList();
         
            var matchingQuestionExam = new FlashcardExamQuestionResponse();
            var matchingQuesIndex = 0;
            // Iterate random list, generate all matching questions
            for(int i = 0; i < flashcardDetailRandomList.Count; ++i)
            {
                var fd = flashcardDetailRandomList[i];
                // Question desc
                var questionDesc = isTermPattern ? fd.WordText : fd.Description;
                // Correct answer text
                var correctAnswerText = isTermPattern ? fd.Definition : fd.WordText;
                
                // Get randomly answer from random question list
                // var rndAnswer = answersForMatchingExam
                //     .FirstOrDefault(ans => !ans.Answer.AnswerText.Equals(correctAnswerText));
                
                // Add question answer pair 
                // matchingQuestionExam.QuestionAnswerPairs.Add(new ()
                // {
                //     Question = new()
                //     {
                //         FlashcardDetailId = fd.FlashcardDetailId,
                //         QuestionTitle = questionDesc ?? string.Empty,
                //         QuestionNumber = ++matchingQuesIndex,
                //         QuestionType = FlashcardExamTypeConstants.MatchingQuestion,
                //     },
                //     Answer = rndAnswer?.Answer ?? new(),
                // });
                matchingQuestionExam.MatchingQuestions.Add(new()
                {
                    FlashcardDetailId = fd.FlashcardDetailId,
                    QuestionTitle = questionDesc ?? string.Empty,
                    QuestionNumber = ++matchingQuesIndex,
                    QuestionType = FlashcardExamTypeConstants.MatchingQuestion,
                });
            }
            
            // Add answer list
            matchingQuestionExam.QuestionAnswers = answersForMatchingExam
                // Shuffle the list 
                .OrderBy(ans => rnd.Next())
                .Select(ans => ans.Answer).ToList();
            // Set question type
            matchingQuestionExam.QuestionType = FlashcardExamTypeConstants.MatchingQuestion;
            // Add single matching question consisting of all other questions
            flashcardExamQuestions.Add(matchingQuestionExam);
            // Return only 1 element using for matching question type only
            return flashcardExamQuestions;
        }
        
        // [True/False]-[Matching question]-[Multiple choice]-[Written]
        // Iterate each flashcard detail to generate flashcard exam question
        var questionIndex = 0;
        // foreach (var fd in flashcardDetailRandomList)
        for (int i = 0; i < flashcardDetailRandomList.Count; ++i)
        {
            var fd = flashcardDetailRandomList[i];
            
            // Random question type
            var questionType = questionTypes[rnd.Next(questionTypes.Count)];
            // Check if question type is Matching, just allow only 1 this question type per exam
            if (flashcardExamQuestions.Any(q => 
                    q.QuestionType.Equals(FlashcardExamTypeConstants.MatchingQuestion)))
            {
                questionType = questionTypes
                    .OrderBy(x => rnd.Next())
                    .First(x => !x.Equals(FlashcardExamTypeConstants.MatchingQuestion));
            }
            // Question desc
            var questionTitle = isTermPattern ? fd.WordText : fd.Description;
            // Correct answer text
            var correctAnswerText = isTermPattern ? fd.Definition : fd.WordText;
            
            switch (questionType)
            {
                // TRUE/FALSE
                case FlashcardExamTypeConstants.TrueFalse:
                    // Random question is true or false
                    // var isTrueQuestion = rnd.NextDouble() >= 0.5;
                    
                    // Random answer for false context
                    var falseAnswer = questionAnswers
                        .OrderBy(ans => rnd.Next())
                        .FirstOrDefault(e => !e.AnswerText.Equals(correctAnswerText));
                    
                    var trueFalseQuestion = new FlashcardExamQuestionResponse()
                    {
                        FlashcardDetailId = fd.FlashcardDetailId,
                        QuestionNumber = ++questionIndex,
                        QuestionTitle = questionTitle ?? string.Empty,
                        QuestionAnswers = new()
                        {
                            falseAnswer ?? null!,
                            new()
                            {
                                AnswerText = correctAnswerText,
                                ImageUrl = fd.CloudResource?.Url
                            }
                        },
                        QuestionType = FlashcardExamTypeConstants.TrueFalse,
                    };
                    
                    // Shuffle answer kust
                    trueFalseQuestion.QuestionAnswers = 
                        trueFalseQuestion.QuestionAnswers.OrderBy(qa => rnd.Next()).ToList();
                    
                    // Set question desc
                    var questionDesc = trueFalseQuestion.QuestionAnswers.First().AnswerText;
                    trueFalseQuestion.QuestionDesc = questionDesc;
                    
                    // Add new true false question type
                    flashcardExamQuestions.Add(trueFalseQuestion);
                    break;
                // MATCHING QUESTION
                case FlashcardExamTypeConstants.MatchingQuestion:
                    // Take 5 question from rnd list
                    var fiveQuestionRow = flashcardDetailRandomList
                        .Skip(questionIndex) // Skip handled question
                        .Take(5).ToList();
                    // Initiate list of question in matching question
                    var matchingQuestions = fiveQuestionRow
                        .Select(x =>
                            new FlashcardExamQuestionResponse()
                            {
                                FlashcardDetailId = x.FlashcardDetailId,
                                QuestionTitle = isTermPattern ? x.WordText : x.Definition,
                                QuestionNumber = ++questionIndex,
                            })
                        .ToList();
                        
                    // Select list of flashcardDetailIds
                    var flashcardDetailIds = matchingQuestions.Select(e =>
                        e.FlashcardDetailId).ToList();
                    
                    // Get all answer from random question list   
                    var answersForMatchingExam = flashcardDetailRandomList
                        .Where(x => flashcardDetailIds.Contains(x.FlashcardDetailId))
                        // Shuffle the list
                        .OrderBy(x => rnd.Next())
                        .Select(x => new FlashcardExamQuestionAnswerResponse
                        {
                            AnswerText = isTermPattern 
                                // Check if question is term pattern ? Definition is answer : Word text is answer
                                ? x.Definition : x.WordText, 
                            ImageUrl = x.CloudResource?.Url
                        })
                        .ToList();
                    
                    // Add new flashcard exam question
                    flashcardExamQuestions.Add(new()
                    {
                        FlashcardDetailId = 0,
                        QuestionTitle = string.Empty,
                        QuestionNumber = 0,
                        QuestionAnswers = answersForMatchingExam,
                        MatchingQuestions = matchingQuestions,
                        QuestionType = FlashcardExamTypeConstants.MatchingQuestion
                    });
                    
                    // Move to current total ques index, as taking a row of 5 questions for matching type
                    i = questionIndex - 1;
                    break;
                // MULTIPLE CHOICE
                case FlashcardExamTypeConstants.MultipleChoice:
                    // Get 3 random answer from other flashcard detail
                    var multipleChoiceAnswers = questionAnswers
                        .Where(e => !e.AnswerText.Equals(correctAnswerText))
                        .OrderBy(x => rnd.Next())
                        .Take(3)
                        .ToList();
                    
                    // Add correct answer
                    multipleChoiceAnswers.Add(new FlashcardExamQuestionAnswerResponse
                    {
                        AnswerText = correctAnswerText,
                        ImageUrl = fd.CloudResource?.Url
                    });
                    
                    // Add new flashcard exam question
                    flashcardExamQuestions.Add(new()
                    {
                        QuestionTitle = questionTitle ?? string.Empty,
                        // Shuffle the list
                        QuestionAnswers = multipleChoiceAnswers.OrderBy(qa =>
                            rnd.Next()).ToList(),
                        QuestionNumber = ++questionIndex,
                        QuestionType = FlashcardExamTypeConstants.MultipleChoice,
                        FlashcardDetailId = fd.FlashcardDetailId
                    });
                    
                    // Remove flashcard detail in random list preventing replicate question
                    // flashcardDetailRandomList.Remove(fd);
                    break;
                // WRITTEN
                case FlashcardExamTypeConstants.Written:
                    var writtenQuestion = new FlashcardExamQuestionResponse()
                    {
                        FlashcardDetailId = fd.FlashcardDetailId,
                        QuestionNumber = ++questionIndex,
                        QuestionTitle = questionTitle ?? null!,
                        QuestionAnswers = new()
                        {
                            new()
                            {
                                AnswerText = string.Empty,
                                ImageUrl = fd.CloudResource?.Url
                            }
                        },
                        QuestionType = FlashcardExamTypeConstants.Written
                    };
                    // Add new written question type
                    flashcardExamQuestions.Add(writtenQuestion);
                    break;
            }
        }

        return flashcardExamQuestions;
    }
}