using Prep4IELTS.Business.Validations;

namespace EXE202_Prep4IELTS.Payloads.Requests.Resources;

public class UpdateImageRequest
{
    [ImageFile(ErrorMessage = "Please upload a valid image file. Allowed types are: .jpg, .jpeg, .png, .gif, .svg, .bmp, .tiff, .ico")]
    public IFormFile File { get; set; } = null!;

    public string PublicId { get; set; } = string.Empty;    
}