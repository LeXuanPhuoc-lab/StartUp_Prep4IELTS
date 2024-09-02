using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Prep4IELTS.Business.Validations;

public class AudioFileAttribute : ValidationAttribute
{
    private readonly string[] _validAudioExtensions = [ ".mp3", ".wav", ".flac", ".aac", ".ogg", ".wma", ".m4a" ];

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var file = value as IFormFile;

        if (file != null)
        {
            var fileExtension = Path.GetExtension(file.FileName.ToLower());

            if (!_validAudioExtensions.Contains(fileExtension))
            {
                return new ValidationResult("Invalid audio file type. Allowed types are: " +
                                            string.Join(", ", _validAudioExtensions));
            }
        }

        return ValidationResult.Success;
    }
}