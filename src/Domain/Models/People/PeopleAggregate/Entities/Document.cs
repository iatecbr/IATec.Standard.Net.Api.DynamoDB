using IATec.Shared.Domain.SeedWorks;

namespace Domain.Models.People.PeopleAggregate.Entities;

public class Document : EntityUlidInt32
{
    public int PersonId { get; private set; }
    public Person? Person { get; private init; }
    public string Value { get; private set; }
    public string Issuer { get; private set; }

    private Document(int personId, string value, string issuer)
    {
        PersonId = personId;
        Value = value;
        Issuer = issuer;
    }

    public static Document Create(int personId, string value, string issuer)
    {
        return new Document(personId, value, issuer);
    }

    public Document()
    {
    }
}