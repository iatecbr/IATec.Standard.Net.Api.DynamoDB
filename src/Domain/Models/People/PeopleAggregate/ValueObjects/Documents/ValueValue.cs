namespace Domain.Models.People.PeopleAggregate.ValueObjects.Documents;

public class ValueValue(string value)
{
    public const int FieldMinLength = 1;
    public const int FieldMaxLength = 150;

    public string Value { get; } = value;
}