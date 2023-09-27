using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Customer : IPerson

{
    private string FirstName { get; init; }
    private string LastName { get; init; }
    private long SocialSecurityNumber { get; init; }
    public DateOnly RegistryDate { get; init; }
    public int CustomerId { get; init; }
    public Customer(string firstName, string lastName, long socialSecurityNumber, DateOnly registryDate, int customerId)
    {
        FirstName = firstName;
        LastName = lastName;
        SocialSecurityNumber = socialSecurityNumber;
        RegistryDate = registryDate;
        CustomerId = customerId;
    }
    public string GetSecurityNumber() => $"{SocialSecurityNumber:000-00-0000}";
    public string GetFirstname() => FirstName;
    public string GetLastname() => LastName;
    public string GetFullInfo() => $"{GetFirstname()}, {GetLastname()} ({GetSecurityNumber()})";
}