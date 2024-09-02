using Prep4IELTS.Business.Validations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Resources;

public class UploadImageRequest
{
    [ImageFile(ErrorMessage = "Please upload a valid image file. Allowed types are: .jpg, .jpeg, .png, .gif, .svg, .bmp, .tiff, .ico")]
    public IFormFile File { get; set; } = null!;

    public string TestType { get; set; } = string.Empty;
    
    public string TestTitle { get; set; } = string.Empty;
    
    public string TestSectionName { get; set; } = string.Empty;
    
    public int PartitionNumber { get; set; } 
}