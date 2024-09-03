using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Prep4IELTS.Business.Validations;

public class ImageFileAttribute : ValidationAttribute
{
    private readonly string[] _validImageExtensions = [".jpg", ".jpeg", ".png", ".svg", ".gif", ".bmp", ".ico"];

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var file = value as IFormFile;

        if (file != null)
        {
            var fileExtension = Path.GetExtension(file.FileName.ToLower());

            if (!_validImageExtensions.Contains(fileExtension))
            {
                return new ValidationResult("Invalid image file type. Allowed type are: " +
                                            string.Join(", ", _validImageExtensions));
            }
        }
        
        return ValidationResult.Success;
    }
}