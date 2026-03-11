using Amazon.DynamoDBv2.DataModel;
using Domain.Models.People.PeopleAggregate.Entities;
using Domain.Models.People.PeopleAggregate.ValueObjects.Person;
using Persistence.Converters.People;

namespace Persistence.Models;

public class PersonPersistence
{
    [DynamoDBHashKey] public string Pk { get; private set; } = "PERSON#";

    [DynamoDBRangeKey] public string HashKey { get; private set; } = "PERFIL#";

    [DynamoDBProperty(typeof(FirstNameValueConverter))]
    public FirstNameValue FirstName { get; private set; }

    [DynamoDBProperty(typeof(LastNameValueConverter))]
    public LastNameValue LastName { get; private set; }

    [DynamoDBProperty(typeof(MiddleNameValueConverter))]
    public MiddleNameValue MiddleName { get; private set; }

    /// <summary>
    /// DynamoDb context maps lists of complex types automatically.
    /// </summary>
    public List<Document> Documents { get; private set; }

    /// <summary>
    /// DynamoDb map simple types automatically.
    /// </summary>
    public int Age { get; private set; }

    public PersonPersistence(int personId, string externalId, FirstNameValue firstName, LastNameValue lastName,
        MiddleNameValue middleName, List<Document> documents, int age)
    {
        Pk = $"{Pk}{personId}";
        HashKey = $"{HashKey}{externalId}";
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        Documents = documents;
        Age = age;
    }

    public PersonPersistence()
    {
    }
}