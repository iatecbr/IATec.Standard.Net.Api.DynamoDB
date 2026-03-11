using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Configurations.Options;

namespace Persistence.Configurations.Extensions;

public static class DatabaseExtension
{
    public static IServiceCollection AddData(this IServiceCollection services, AwsOption options)
    {
        if (options.DynamoDb is null)
            throw new ArgumentNullException($"{nameof(AwsOption)} cannot be null!");

        // Configuration for low-level API
        services.AddSingleton<IAmazonDynamoDB>(_ =>
        {
            var clientConfig = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(options.DynamoDb?.Region)
            };

            if (options.UseLocalStack)
                clientConfig.ServiceURL = options.ServiceUrl;

            return new AmazonDynamoDBClient(options.DynamoDb?.AccessKey, options.DynamoDb?.SecretKey, clientConfig);
        });

        //Configuration for high-level API, similar to DbContext in EF Core
        services.AddScoped<IDynamoDBContext>(sp =>
        {
            var client = sp.GetRequiredService<IAmazonDynamoDB>();

            var contextBuilder = new DynamoDBContextBuilder()
                .WithDynamoDBClient(() => client)
                .ConfigureContext(cfg =>
                {
                    cfg.ConsistentRead = false;
                    cfg.SkipVersionCheck = true;
                })
                .Build();

            return contextBuilder;
        });

        return services;
    }
}