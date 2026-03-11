using Amazon.DynamoDBv2.Model;

namespace Persistence.Contracts.Migrations;

public interface IMigration
{
    public Task<CreateTableRequest> ConfigureMigrationAsync();
}