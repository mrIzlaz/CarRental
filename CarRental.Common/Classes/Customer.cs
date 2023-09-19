using CarRental.Common.Interfaces;
namespace CarRental.Common.Classes;

public class Customer : IPerson

{
    private int _id;
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string SocialSecurityNumber { get; init; }
    public string SE_SSN { get; init; }
    public DateOnly RegistryDate { get; init; }
    public int CustomerId { get => _id; init => _id = value; }

    public Customer(string firstName, string lastName, string socialSecurityNumber, string se_ssn, DateOnly registryDate, int customerId)
    {
        FirstName = firstName;
        LastName = lastName;
        SE_SSN = se_ssn;
        SocialSecurityNumber = socialSecurityNumber;
        RegistryDate = registryDate;
        CustomerId = customerId;
    }

    public string GetSecurityNumber() => SocialSecurityNumber;
    public string GetSecurityNumberUSFormated() => String.Format("{0:000-00-0000}", SocialSecurityNumber); //This is not working
    public string GetSecurityNumberSEFormated() => String.Format("{0:000000-0000}", SE_SSN); 

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
        var ssn = GetSecurityNumberSEFormated();
        return $"{GetFirstname()}, {GetLastname()} ({ssn})";
    }

    public string GetSecurityFormated(bool USFormat) => USFormat ? GetSecurityNumberUSFormated() : GetSecurityNumberSEFormated();
}
