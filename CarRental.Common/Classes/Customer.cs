using CarRental.Common.Interfaces;
namespace CarRental.Common.Classes;

public class Customer : IPerson

{
    private int _id;
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string SocialSecurityNumber { get; init; }
    public DateOnly RegistryDate { get; init; }
    public int CustomerId { get => _id; init => _id = value; }

    public Customer(string firstName, string lastName, string socialSecurityNumber, DateOnly registryDate, int customerId)
    {
        FirstName = firstName;
        LastName = lastName;
        SocialSecurityNumber = socialSecurityNumber;
        RegistryDate = registryDate;
        CustomerId = customerId;
    }

    public string GetSecurityNumber() => SocialSecurityNumber;
    public string GetSecurityNumberUSFormated() => String.Format("{0:000-00-0000}", SocialSecurityNumber);
    public string GetSecurityNumberSEFormated() => String.Format("{0:000000-0000}", SocialSecurityNumber);

    public string GetFirstname()
    {
        return FirstName;
    }

    public string GetLastname()
    {
        return LastName;
    }

    public string GetFullInfo()
    {
        var ssn = GetSecurityNumberUSFormated();
        return $"{GetFirstname()} {GetLastname} ({ssn})";
    }

    public string GetSecurityFormated(bool USFormat) => USFormat ? GetSecurityNumberUSFormated() : GetSecurityNumberSEFormated();
}
