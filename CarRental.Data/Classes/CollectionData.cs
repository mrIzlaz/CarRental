namespace CarRental.Data.Classes;

using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Common.Enums;
using CarRental.Common.Interfaces;
using Interfaces;
using System.Collections.Generic;
using CarRental.Common.Classes;

public class CollectionData : IData
{
    private readonly DataFactory _producer;
    private readonly List<Customer> _persons = new();
    private readonly List<Vehicle> _vehicles = new();
    private readonly List<IBooking> _bookings = new();
    public int NextPersonId => _persons.Count + 1;
    public int NextVehicleId => _vehicles.Count + 1;
    public int NextBookingId => _bookings.Count + 1;

    public CollectionData()
    {
        _producer = new ();
        SeedData();
    }

    public IBooking? RentVehicle(int vehicleId, int customerId)
    {
        var customer = Single<Customer>(c => c.CustomerId == customerId);
        var vehicle = Single<Vehicle>(v => v.Id == vehicleId);
        if (customer == null || vehicle == null) return null;
        return new Booking(NextBookingId, vehicle, customer, DateTime.Today);
    }

    public IBooking? ReturnVehicle(int vehicleId) =>
        Single<IBooking>(b => b.BookingStatus == VehicleStatus.Booked && b.Vehicle.Id.Equals(vehicleId));


    public T? Single<T>(Expression<Func<T, bool>>? expression)
    {
        var func = expression?.Compile();
        var list = GetListFrom<T>();
        if (list == null) return default;
        return func != null ? list.SingleOrDefault(func) : default;
    }
    //Söka igenom listorna som stödjer ISearchable

    /*
    public IEnumerable<string> SearchResult<T>(string searchPrompt) where T : ISearchable
    {
        List<string> test = new();
        FieldInfo[] fieldInfo = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        var iSearchableFieldInfo = fieldInfo.Where(fi => fi.FieldType.IsAssignableFrom(typeof(T)));

        
        
        if (fieldInfo.FirstOrDefault(f => f.FieldType.IsAssignableFrom(typeof(T)))?.GetValue(this) is List<T> one)
        {
            one.Where(c => c.MatchingThis(searchPrompt)).ToList().ForEach(c => test.Add(c.ToString()));
        }

        var ie = fieldInfo.Where(fi => fi.FieldType.IsAssignableFrom(typeof(List<ISearchable>)));

        foreach (var field in iSearchableFieldInfo)
        {
            var temp = field?.GetValue(this) as List<T>;
            temp?.ForEach(v =>
            {
                if (v.MatchingThis(searchPrompt))
                    test.Add(v.ToString());
            });
        }


        return test;
    }
    public IEnumerable<string> SearchResult<T>(string searchPrompt) where T : ISearchable //This works
    {
        List<string> searchResults = new List<string>();
        FieldInfo[] fieldInfo = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (FieldInfo field in fieldInfo)
        {
            // Check if the field type is a generic List<>
            if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
            {
                // Get the generic type argument of the List<>
                Type listType = field.FieldType.GetGenericArguments()[0];

                // Check if the generic type implements the ISearchable interface
                if (typeof(ISearchable).IsAssignableFrom(listType))
                {
                    // Retrieve the value of the field (the List) and cast it to IEnumerable<ISearchable>
                    var list = (IEnumerable<ISearchable>)field.GetValue(this);

                    // Filter items of type T from the list and apply the search
                    // Then select the string representation of matching items
                    searchResults.AddRange(list.OfType<T>().Where(item => item.MatchingThis(searchPrompt))
                        .Select(item => item.ToString()));
                }
            }
        }
        return searchResults;
    }
*/

    /* public IEnumerable<string> SearchResult<T>(string searchPrompt) where T : ISearchable 
     {
         List<string> searchResults = new List<string>();
         FieldInfo[] fieldInfo = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
 
         foreach (FieldInfo field in fieldInfo)
         {
             // Check if the field type is a generic List<>
             if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
             {
                 // Get the generic type argument of the List<>
                 Type listType = field.FieldType.GetGenericArguments()[0];
 
                 // Check if the generic type implements the ISearchable interface
                 if (typeof(ISearchable).IsAssignableFrom(listType))
                 {
                     // Retrieve the value of the field (the List) and cast it to IEnumerable<ISearchable>
                     var list = (IEnumerable<ISearchable>)field.GetValue(this);
 
                     // Filter items of type T from the list and apply the search
                     // Then select the string representation of matching items
                     searchResults.AddRange(list.OfType<T>().Where(item => item.MatchingThis(searchPrompt))
                         .Select(item => item.ToString()));
                 }
             }
         }
         return searchResults;
     }*/

    public IEnumerable<string> SearchResult<T>(string searchPrompt) where T : ISearchable
    {
        List<string> searchResults = new List<string>();

        FieldInfo[] fieldInfo = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (FieldInfo field in fieldInfo)
        {
            if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
            {
                // Get the generic type argument of the List<>
                Type listType = field.FieldType.GetGenericArguments()[0];

                // Check if the generic type (listType) or any of its interfaces implement ISearchable
                if (typeof(ISearchable).IsAssignableFrom(listType) ||
                    listType.GetInterfaces().Any(i => i == typeof(ISearchable)))
                {
                    var list = (IEnumerable)field.GetValue(this);

                    // Filter items that implement ISearchable and match the search criteria
                    searchResults.AddRange(list.Cast<ISearchable>().Where(item => item.MatchingThis(searchPrompt))
                        .Select(item => item.ToString()));
                }
            }
        }

        return searchResults;
    }


    public IEnumerable<T> Get<T>(Expression<Func<T, bool>>? expression)
    {
        var func = expression?.Compile();
        var list = GetListFrom<T>();
        if (list == null) return new List<T>();
        return func == null ? list : list.Where(func).ToList();
    }

    public void Add<T>(T item)
    {
        var result = GetListFrom<T>();
        if (result == null)
            throw new Exception($"Can't handle {typeof(T)}");
        result.Add(item);
    }

    private List<T>? GetListFrom<T>()
    {
        FieldInfo[] fieldInfo = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        var list = fieldInfo.FirstOrDefault(f => f.FieldType == typeof(List<T>))?.GetValue(this) as List<T>;
        return list;
    }

    private void SeedData()
    {
        try
        {
            _persons.AddRange(_producer.GetPersons());
            _vehicles.AddRange(_producer.GetVehicles());
            _bookings.AddRange(_producer.GetBookings());
        }
        catch (Exception)
        {
            // ignored
        }
    }
}