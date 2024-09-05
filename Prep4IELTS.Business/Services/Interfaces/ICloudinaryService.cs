using Microsoft.AspNetCore.Http;
using Prep4IELTS.Data.Enum;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ICloudinaryService
{
    Task<(string? secureUrl, string? publicId, string? messageErr)> UploadAsync(string publicId, IFormFile file, FileType fileType);
    Task<(string? secureUrl, string? publicId, string? messageErr)> UpdateAsync(string publicId, IFormFile file, FileType fileType);
    Task<(bool? isDeleteSucess, string? messageErr)> DeleteAsync(string publicId, string fileType);
}