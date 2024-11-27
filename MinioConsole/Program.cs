using Minio;

var endpoint = "play.min.io:9000";
var accessKey = "minioadmin";
var secretKey = "minioadmin";
var secure = true;
IMinioClient minio = new MinioClient()
    .WithEndpoint(endpoint)
    .WithCredentials(accessKey, secretKey)
    .WithSSL(secure)
    .Build();

// Create an async task for listing buckets.
var getListBucketsTask = await minio.ListBucketsAsync().ConfigureAwait(false);
foreach (var bucket in getListBucketsTask.Buckets)
{
    Console.WriteLine(bucket.Name + " " + bucket.CreationDateDateTime);
}
Console.WriteLine("List of buckets");
