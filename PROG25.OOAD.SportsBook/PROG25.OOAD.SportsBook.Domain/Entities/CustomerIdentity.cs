using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Domain.Entities;

public class CustomerIdentity
{
    public CustomerIdentity
    (
        PersonId personId,
        Name name,
        EmailAddress email
    )
    {

        if (personId.Type == PersonIdType.Anonymized)
        {
            throw new ArgumentException("PersonId cannot be of type Anonymized.", nameof(personId));
        }

        if (name == Name.Anonymized)
        {
            throw new ArgumentException("Name cannot be anonymized");
        }

        if (email == EmailAddress.Anonymized)
        {
            throw new ArgumentException("Email cannot be anonymized");
        }

        PersonId = personId;
        Name = name;
        Email = email;
    }

    public PersonId PersonId { get; private set; }
    public Name Name { get; private set; }
    public EmailAddress Email { get; private set; }
    public bool IsAnonymized { get; private set; }

    public void ChangeEmail(EmailAddress newEmail)
    {
        EnsureNotAnonymized();

        if (newEmail == EmailAddress.Anonymized)
        {
            throw new ArgumentException("Email cannot be changed to an anonymized value.", nameof(newEmail));
        }

        Email = newEmail;
    }

    public void Rename(Name newName)
    {
        EnsureNotAnonymized();
        EnsureIsValidName(newName);
        Name = newName;
    }

    internal void Anonymize()
    {
        EnsureNotAnonymized();

        PersonId = AnonymizedPersonId.Instance;
        Name = Name.Anonymized;
        Email = EmailAddress.Anonymized;
        IsAnonymized = true;

    }

    private void EnsureNotAnonymized()
    {
        if (IsAnonymized)
        {
            throw new InvalidOperationException("Operation not allowed for anonymized customers.");
        }
    }

    private static void EnsureIsValidName(Name name)
    {
        if (name == Name.Anonymized)
        {
            throw new InvalidOperationException("Name cannot be anonymized");
        }
    }

}