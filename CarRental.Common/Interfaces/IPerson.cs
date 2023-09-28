namespace CarRental.Common.Interfaces;

public interface IPerson
{
    public string SecurityNumber { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string FullInfo { get; }
}
