using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Options;
using Persistence.Configurations.Options;
using Persistence.Contracts.Migrations;
using Persistence.Models;

namespace Persistence.Migrations.Tables;

public class PeopleTableMigration(IOptions<AwsOption> awsOptions) : IMigration
{
    public async Task<CreateTableRequest> ConfigureMigrationAsync()
    {
        var request = new CreateTableRequest
        {
            TableName = awsOptions.Value.DynamoDb?.TableNames?.Person ?? "Person",
            AttributeDefinitions =
            [
                new AttributeDefinition(nameof(PersonPersistence.Pk), ScalarAttributeType.S),
                new AttributeDefinition(nameof(PersonPersistence.HashKey), ScalarAttributeType.S)
            ],
            KeySchema =
            [
                new KeySchemaElement(nameof(PersonPersistence.Pk), KeyType.HASH),
                new KeySchemaElement(nameof(PersonPersistence.HashKey), KeyType.RANGE)
            ],
            ProvisionedThroughput = new ProvisionedThroughput(25, 25)
        };

        return await Task.FromResult(request);
    }
}