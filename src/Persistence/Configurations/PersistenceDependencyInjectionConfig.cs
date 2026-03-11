using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Configurations.Extensions;
using Persistence.Configurations.Options;

namespace Persistence.Configurations;

public static class PersistenceDependencyInjectionConfig
{
    public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AwsOption>(configuration.GetSection(AwsOption.Key).Bind);
        var options = configuration.GetSection(AwsOption.Key).Get<AwsOption>();

        services.AddData(options!)
            .AddMigrations()
            .AddMappings(options!);
    }
}