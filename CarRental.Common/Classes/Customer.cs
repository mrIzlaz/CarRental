using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Customer : IPerson
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string SecurityNumber { get; init; }
    public DateOnly RegistryDate { get; init; }
    public int CustomerId { get; private set; }
    public string FullInfo => $"{FirstName}, {LastName} ({SecurityNumber})";

    public Customer(string firstName, string lastName, long socialSecurityNumber, DateOnly registryDate, int customerId)
        : this(firstName, lastName, socialSecurityNumber, registryDate)
    {
        CustomerId = customerId;
    }

    public Customer(string firstName, string lastName, long socialSecurityNumber, DateOnly registryDate)
    {
        FirstName = firstName;
        LastName = lastName;
        SecurityNumber = $"{socialSecurityNumber:000-00-0000}";
        RegistryDate = registryDate;
    }
    
    public Customer(Customer customer, int customerId)
    {
        FirstName = customer.FirstName;
        LastName = customer.LastName;
        SecurityNumber = customer.SecurityNumber;
        RegistryDate = customer.RegistryDate;
        CustomerId = customerId;
    }

}