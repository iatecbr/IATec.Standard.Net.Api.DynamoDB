using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Domain.Models.People.PeopleAggregate.ValueObjects.Person;

namespace Persistence.Converters.People;

public class LastNameValueConverter : IPropertyConverter
{
    public DynamoDBEntry? ToEntry(object value)
    {
        return value is not LastNameValue myValueObject ? null : new Primitive { Value = myValueObject.Value };
    }

    public object? FromEntry(DynamoDBEntry entry)
    {
        if (entry is not Primitive primitive || string.IsNullOrEmpty(primitive.AsString()))
            return null;

        return new LastNameValue(primitive.AsString());
    }
}