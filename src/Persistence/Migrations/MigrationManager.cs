using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using Persistence.Contracts.Migrations;

namespace Persistence.Migrations;

public class MigrationManager(
    IAmazonDynamoDB client,
    IEnumerable<IMigration> migrations,
    ILogger<MigrationManager> logger) : IMigrationManager
{
    public async Task ApplyMigrationsAsync()
    {
        var response = await client.ListTablesAsync();

        logger.LogInformation("Starting migration, found {TableCount} tables", response.TableNames.Count);

        foreach (var migration in migrations)
        {
            var request = await migration.ConfigureMigrationAsync();

            if (response.TableNames.Contains(request.TableName))
            {
                logger.LogInformation("Table {TableName} already exists, skipping migration", request.TableName);
                continue;
            }

            try
            {
                logger.LogInformation("Creating table {TableName}", request.TableName);
                await client.CreateTableAsync(request);
            }
            catch (ContinuousBackupsUnavailableException ex)
            {
                logger.LogError(ex, "Error enabling continuous backup on table {TableName}", request.TableName);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating table {TableName}", request.TableName);
                throw;
            }
        }
    }
}