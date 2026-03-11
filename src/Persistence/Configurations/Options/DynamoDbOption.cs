namespace Persistence.Configurations.Options;

public class DynamoDbOption
{
    public string Region { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// This property holds the names of the DynamoDB tables used in the application on local environment with localstack.
    /// The migration tool uses these names to create the necessary tables in the local DynamoDB instance.
    /// </summary>
    public DynamoDbTableNames? TableNames { get; set; }
}