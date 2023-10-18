namespace CarRental.Common.Interfaces;

public interface ISearchable
{
    public bool MatchingThis(string prompt);

    public string StringValue();
}