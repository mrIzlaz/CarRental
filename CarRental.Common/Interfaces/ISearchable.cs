using System.Reflection;

namespace CarRental.Common.Interfaces;

public interface ISearchable
{
    public bool IsMatchingThis(string prompt);

    public bool IsMatchingThis<T>(string prompt) where T : ISearchable
    {
        // Get the type of the class T
        Type type = typeof(T);

        var debug1 = type.ToString();
        var fieldinfo = type.GetFields(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
        
        
        
        // Get all the properties of the class T that implement ISearchable
        var searchableProperties = type.GetProperties()
            .Where(prop =>
                prop.PropertyType == typeof(string) && prop.GetCustomAttributes(typeof(ISearchable), false).Any());
        // Iterate through the searchable properties
        foreach (var property in searchableProperties)
        {
            // Get the value of the property
            var propertyValue = (string)property.GetValue(this);

            // Check if the property value contains the prompt
            if (propertyValue != null && propertyValue.Contains(prompt))
                return true; // Return true if a match is found
        }

        // If no match is found in any property, return false
        return false;
    }


    public string ToString();
}