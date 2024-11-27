using Minio;

namespace WebApplication1;

public static class AddMinio
{
    public static IServiceCollection AddMinioClient(this IServiceCollection services, IConfiguration configuration)
    {
        var minioSettings = new MinioSettings();
        configuration.GetSection("Minio").Bind(minioSettings);

        // services.AddSingleton<IMinioClient>(new MinioClient()
        //     .WithEndpoint(minioSettings.Endpoint)
        //     .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey)
        //     .WithSSL(true).Build()
        // );
        services.AddMinio(configureClient =>
        {
            configureClient
                .WithEndpoint(minioSettings.Endpoint)
                .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey)
                .WithSSL(true); // 如果需要 SSL，可以设置为 true
        }, ServiceLifetime.Singleton);
        return services;
    }

}