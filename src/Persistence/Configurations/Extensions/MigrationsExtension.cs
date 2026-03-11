using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contracts.Migrations;
using Persistence.Migrations;

namespace Persistence.Configurations.Extensions;

public static class MigrationsExtension
{
    public static IServiceCollection AddMigrations(this IServiceCollection services)
    {
        //Register migrations
        var migrationTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IMigration).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

        foreach (var type in migrationTypes) services.AddTransient(typeof(IMigration), type);

        // Register Migration Manager
        services.AddSingleton<IMigrationManager, MigrationManager>();

        return services;
    }
}