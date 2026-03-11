using Amazon;
using Amazon.Util;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Configurations.Options;
using Persistence.Models;

namespace Persistence.Configurations.Extensions;

public static class MappingExtension
{
    public static IServiceCollection AddMappings(this IServiceCollection services, AwsOption options)
    {
        AWSConfigsDynamoDB.Context.AddMapping(new TypeMapping(typeof(PersonPersistence),
            options.DynamoDb?.TableNames?.Person));

        return services;
    }
}