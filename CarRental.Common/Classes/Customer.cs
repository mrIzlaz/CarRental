using CarRental.Common.Interfaces;
namespace CarRental.Common.Classes;

public class Customer : IPerson

{
    private string FirstName { get; init; }
    private string LastName { get; init; }
    private long SocialSecurityNumber { get; init; }
    private long SeSsn { get; init; }
    public DateOnly RegistryDate { get; init; }
    public int CustomerId { get; init; }

    public Customer(string firstName, string lastName, long socialSecurityNumber, long seSsn, DateOnly registryDate, int customerId)
    {
        FirstName = firstName;
        LastName = lastName;
        SeSsn = seSsn;
        SocialSecurityNumber = socialSecurityNumber;
        RegistryDate = registryDate;
        CustomerId = customerId;
    }

    public string GetSecurityNumber() => GetSecurityNumberUsFormat();
    private string GetSecurityNumberUsFormat() => $"{SocialSecurityNumber:000-00-0000}";
    private string GetSecurityNumberSeFormat() => $"{SeSsn:000000-0000}"; 

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
        var ssn = GetSecurityNumberSeFormat();
        return $"{GetFirstname()}, {GetLastname()} ({ssn})";
    }

    public string GetSecurityFormat(bool usFormat) => usFormat ? GetSecurityNumberUsFormat() : GetSecurityNumberSeFormat();
}
