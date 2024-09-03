using Prep4IELTS.Business.Validations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Resources;

public class UploadAudioRequest
{
    [AudioFile(ErrorMessage = "Please upload a valid audio file. Allowed types are: .mp3, .wav, .flac, .aac, .ogg, .wma, .m4a.")]
    public IFormFile File { get; set; } = null!;

    public string TestType { get; set; } = string.Empty;
    
    public string TestTitle { get; set; } = string.Empty;
    
    public string TestSectionName { get; set; } = string.Empty;
}