using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;

namespace DoAnCuoiKy.Service
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using DoAnCuoiKy.Model.Response;

    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration config)
        {
            var acc = new Account(
                config["CloudinarySettings:CloudName"],
                config["CloudinarySettings:ApiKey"],
                config["CloudinarySettings:ApiSecret"]
            );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<UploadResultResponse> UploadFileAsync(IFormFile file, string folder)
        {
            await using var stream = file.OpenReadStream();
            string extension = Path.GetExtension(file.FileName).ToLower();

            // Kiểm tra có phải ảnh không
            bool isImage = extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".webp";

            // Nếu là ảnh: dùng ImageUploadParams để resize + nén
            if (isImage)
            {
                var imageParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder,
                    UseFilename = true,
                    UniqueFilename = true,
                    Overwrite = true,
                    Transformation = new Transformation()
                        .Width(1000).Height(1000).Crop("limit")
                        .Quality("auto").FetchFormat("auto")
                };

                var imageResult = await _cloudinary.UploadAsync(imageParams);
                if (imageResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new UploadResultResponse
                    {
                        Url = imageResult.SecureUrl.ToString(),
                        PublicId = imageResult.PublicId
                    };
                }
                else
                {
                    throw new Exception($"Upload image failed: {imageResult.Error?.Message}");
                }
            }
            else
            {
                // Nếu không phải ảnh: dùng RawUploadParams
                var fileParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder,
                    UseFilename = true,
                    UniqueFilename = true,
                    Overwrite = true
                };

                var fileResult = await _cloudinary.UploadAsync(fileParams);

                if (fileResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new UploadResultResponse
                    {
                        Url = fileResult.SecureUrl.ToString(),
                        PublicId = fileResult.PublicId
                    };
                }
                else
                {
                    throw new Exception($"Upload file failed: {fileResult.Error?.Message}");
                }
            }
        }

    }
}
