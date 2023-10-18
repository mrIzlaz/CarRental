using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarRental.Common.Extensions;
using Microsoft.VisualBasic;

namespace CarRental.Data.Classes;

using Common.Enums;
using CarRental.Common.Interfaces;
using Interfaces;
using System.Collections.Generic;
using CarRental.Common.Classes;

public class CollectionData : IData
{
    private readonly DataFactory _producer;
    private readonly List<IPerson> _persons = new();
    private readonly List<Vehicle> _vehicles = new();
    private readonly List<IBooking> _bookings = new();
    public int NextPersonId => _persons.Count + 1;
    public int NextVehicleId => _vehicles.Count + 1;
    public int NextBookingId => _bookings.Count + 1;

    public CollectionData()
    {
        _producer = new DataFactory(this);
        SeedData();
    }

    public IBooking? RentVehicle(int vehicleId, int customerId)
    {
        var customer = Get<IPerson>(null).Cast<Customer>().Single(c => c.CustomerId == customerId);
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

    public IEnumerable<string> SearchResult<T>(string searchPrompt) where T : ISearchable
    {
        List<string> test = new();
        FieldInfo[] fieldInfo = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        var listResult = fieldInfo.Where(f => f.FieldType is T) as List<T>;

        foreach (var VARIABLE in listResult)
        {
            test.Add(VARIABLE.ToString());
        }
        /* var list = fieldInfo.ForEach(x => 
         {
             
         });
         //if (listResult.Count > 1)
         //    listResult.ForEach().GetValue(this);
 */

        return test;
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