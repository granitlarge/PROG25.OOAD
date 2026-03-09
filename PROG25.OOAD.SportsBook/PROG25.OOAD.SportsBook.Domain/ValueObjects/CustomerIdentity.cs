namespace PROG25.OOAD.SportsBook.Domain.ValueObjects;

public record CustomerIdentity
{
    public static CustomerIdentity Anonymized => new(AnonymizedPersonId.Instance, Name.Anonymized, EmailAddress.Anonymized, true);

    private CustomerIdentity
    (
        PersonId personId,
        Name name,
        EmailAddress email,
        bool isAnonymized
    )
    {
        PersonId = personId;
        Name = name;
        Email = email;
        IsAnonymized = isAnonymized;
    }

    public PersonId PersonId { get; private set; }
    public Name Name { get; private set; }
    public EmailAddress Email { get; private set; }
    public bool IsAnonymized { get; private set; }

    private static void EnsureIsValidPersonId(PersonId personId)
    {
        if (personId.Type == PersonIdType.Anonymized)
        {
            throw new ArgumentException("PersonId cannot be of type Anonymized.", nameof(personId));
        }
    }

    private static void EnsureIsValidName(Name name)
    {
        if (name == Name.Anonymized)
        {
            throw new InvalidOperationException("Name cannot be anonymized");
        }
    }

    private static void EnsureIsValidEmail(EmailAddress email)
    {
        if (email == EmailAddress.Anonymized)
        {
            throw new InvalidOperationException("Email cannot be anonymized");
        }
    }

    public static CustomerIdentity Create(PersonId personId, Name name, EmailAddress email)
    {
        EnsureIsValidPersonId(personId);
        EnsureIsValidName(name);
        EnsureIsValidEmail(email);
        return new CustomerIdentity(personId, name, email, false);
    }

}