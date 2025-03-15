namespace Infrastructure.Services;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

using Domain.Interfaces.Services;
using Domain.Shared.Configurations;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

	public ImageService(IOptionsMonitor<CloudinarySettings> cloudinarySettings)
	{
		var acc = new Account(
			cloudinarySettings.CurrentValue.CloudName,
			cloudinarySettings.CurrentValue.ApiKey,
			cloudinarySettings.CurrentValue.ApiSecret
		);

		_cloudinary = new Cloudinary(acc);
	}

    public async Task<ImageUploadResult> AddImageAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();
        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream)
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        return uploadResult;
    }

    public async Task<DeletionResult> DeleteImageAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result;
    }


}