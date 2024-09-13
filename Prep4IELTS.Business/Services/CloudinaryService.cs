using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;

namespace Prep4IELTS.Business.Services;

public class CloudinaryService(
    Cloudinary cloudinary, 
    IOptionsMonitor<AppSettings> monitor) : ICloudinaryService
{
    private readonly AppSettings _appSettings = monitor.CurrentValue;

    public async Task<(string? secureUrl, string? publicId, string? messageErr)> UploadAsync(string publicId, IFormFile file, FileType fileType)
    {
        // Custom public Id, end with random digits
        var publicIdCustom = $"{publicId}/{PaymentHelper.GenerateRandomDigits(7)}";
    
        switch (fileType)
        {
            // IMAGE
            case FileType.Image:
                // Image upload params
                var imageUploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(publicIdCustom, file.OpenReadStream()),
                    PublicId = publicIdCustom
                };
                var imageResult = await cloudinary.UploadAsync(imageUploadParams);
                
                // Success
                if(imageResult.StatusCode == HttpStatusCode.OK) return (imageResult.SecureUrl.ToString(), imageResult.PublicId ,null);
                // Error
                return (null, null, imageResult.Error.Message);
                
            // VIDEO
            case FileType.Video:
                // Video upload params
                var videoUploadParams = new VideoUploadParams()
                {
                    File = new FileDescription(publicIdCustom, file.OpenReadStream()),
                    PublicId = publicIdCustom
                };
                var videoResult = await cloudinary.UploadAsync(videoUploadParams);
                
                // Success
                if(videoResult.StatusCode == HttpStatusCode.OK) return (videoResult.SecureUrl.ToString(), videoResult.PublicId ,null);
                // Error
                return (null, null, videoResult.Error.Message);
        }
        
        return (null, null, "Error at Server. Failed to upload image/video file");
    }

    public async Task<(string? secureUrl, string? publicId, string? messageErr)> UpdateAsync(string publicId, IFormFile file,
        FileType fileType)
    {
        switch (fileType)
        {
            // IMAGE
            case FileType.Image:
                // Image upload params
                var imageUploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(publicId, file.OpenReadStream()),
                    PublicId = publicId,
                    // Invalidate = true,
                    Overwrite = true
                };
                var imageResult = await cloudinary.UploadAsync(imageUploadParams);

                // Success
                if (imageResult.StatusCode == HttpStatusCode.OK) return (imageResult.SecureUrl.ToString(), imageResult.PublicId ,null);
                // Error
                return (null, null, imageResult.Error.Message);

            // VIDEO
            case FileType.Video:
                // Video upload params
                var videoUploadParams = new VideoUploadParams()
                {
                    File = new FileDescription(publicId, file.OpenReadStream()),
                    PublicId = publicId,
                    // Invalidate = true,
                    Overwrite = true
                };
                var videoResult = await cloudinary.UploadAsync(videoUploadParams);

                // Success
                if (videoResult.StatusCode == HttpStatusCode.OK) return (videoResult.SecureUrl.ToString(), videoResult.PublicId ,null);
                // Error
                return (null, null, videoResult.Error.Message);
        }
        
        return (null, null, $"Error at Server. Failed to update file: {publicId}");
    }

    public async Task<(bool? isDeleteSucess, string? messageErr)> DeleteAsync(string publicId, string fileType)
    {
        var existResource = await cloudinary.GetResourceAsync(publicId);
        if (existResource == null) return (false, "Not found any resource match");
        
        var deleteParams = new DeletionParams(publicId)
        {
            Invalidate = true,
            ResourceType = fileType.Equals(FileType.Image.GetDescription()) ? ResourceType.Image : ResourceType.Video
        };
        var deleteResult = await cloudinary.DestroyAsync(deleteParams);
        
        // Not found
        if (deleteResult.Result.Contains("not found")) return (false, "Not found any resource match.");
        // Success
        if (deleteResult.StatusCode == HttpStatusCode.OK) return (true, null!);
        
        return (false, $"Error at Server. Failed to delete file: {publicId}");
    }
}