namespace Persistence.Configurations.Options;

public class AwsOption
{
    public const string Key = "AWS";
    public string? ServiceUrl { get; set; } = string.Empty;
    public bool UseLocalStack { get; set; }
    public DynamoDbOption? DynamoDb { get; set; }
}