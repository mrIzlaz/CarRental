using System.Reflection;

namespace CarRental.Common.Interfaces;

public interface ISearchable
{
    public bool IsMatchingThis(string prompt);
    public abstract string ToString();
}