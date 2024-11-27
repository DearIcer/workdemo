using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.DataModel.Encryption;
using Minio.Exceptions;

namespace WebApplication1;

[Route("api/[controller]/[action]")]
[ApiController]
public class DemoController : Controller
{
    private readonly IMinioClient _minioClient;
    public DemoController(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetBuckets()
    {
        var getListBucketsTask = await _minioClient.ListBucketsAsync().ConfigureAwait(false);
        foreach (var bucket in getListBucketsTask.Buckets)
        {
            Console.WriteLine(bucket.Name + " " + bucket.CreationDateDateTime);
        }
        return Ok();
    }
    
    
    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var bucketName = "icemusic";
        var objectName = file.FileName; // 使用上传文件的名称
        var contentType = file.ContentType; // 使用上传文件的内容类型

        try
        {
            // Check if bucket exists.
            var beArgs = new BucketExistsArgs()
                .WithBucket(bucketName);
            bool found = await _minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);
                await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
            }

            // Upload a file to bucket.
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName) // 使用上传文件的名称
                .WithStreamData(file.OpenReadStream())
                .WithContentType(contentType)
                .WithObjectSize(file.Length);
            await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

            Console.WriteLine("Successfully uploaded " + objectName);
        }
        catch (MinioException e)
        {
            Console.WriteLine("File Upload Error: {0}", e.Message);
            return StatusCode(500, $"Internal server error: {e.Message}");
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> UploadBigFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }
        var bucketName = "icemusic";
        var objectName = file.FileName; // 使用上传文件的名称
        var contentType = file.ContentType; // 使用上传文件的内容类型
        try
        {
            Aes aesEncryption = Aes.Create();
            aesEncryption.KeySize = 256;
            aesEncryption.GenerateKey();
            var ssec = new SSEC(aesEncryption.Key);
            var progress = new Progress<ProgressReport>(progressReport =>
            {
                // Progress events are delivered asynchronously (see remark below)
                Console.WriteLine(
                    $"Percentage: {progressReport.Percentage}% TotalBytesTransferred: {progressReport.TotalBytesTransferred} bytes");
                if (progressReport.Percentage != 100)
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                else Console.WriteLine();
            });
            PutObjectArgs putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName) // 使用上传文件的名称
                .WithStreamData(file.OpenReadStream())
                .WithContentType(contentType)
                .WithObjectSize(file.Length)
                .WithServerSideEncryption(ssec)
                .WithProgress(progress);
            await _minioClient.PutObjectAsync(putObjectArgs);
            Console.WriteLine($"{objectName} is uploaded successfully");
        }
        catch(MinioException e)
        {
            Console.WriteLine("Error occurred: " + e);
        }
        return Ok();
    }
}