using CarRental.Common.Interfaces;
namespace CarRental.Common.Classes;

public class Customer : IPerson

{
    private int _id;
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required int SocialSecurityNumber { get; init; }
    public DateOnly RegistryDate { get; init; }
    public required int CustomerId { get => _id; init => _id = value; }


    public int GetSecurityNumber() => SocialSecurityNumber;
    public string GetSecurityNumberFormated() => String.Format("{0:000-00-0000}", SocialSecurityNumber);



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
        var ssn = String.Format("{0:000-00-0000}", SocialSecurityNumber);
        return $"{GetFirstname()} {GetLastname} ({ssn})";
    }
}
