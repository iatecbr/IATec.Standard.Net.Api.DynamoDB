using Microsoft.Extensions.Options;
using Persistence.Configurations.Options;
using Persistence.Contracts.Migrations;

namespace Api.Configurations.Extensions;

public static class MigrationExtensions
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var awsOptions = scope.ServiceProvider.GetRequiredService<IOptions<AwsOption>>();

        if (!awsOptions.Value.UseLocalStack)
            return app;

        var migrationManager = scope.ServiceProvider.GetRequiredService<IMigrationManager>();

        migrationManager.ApplyMigrationsAsync().WaitAsync(CancellationToken.None)
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        return app;
    }
}