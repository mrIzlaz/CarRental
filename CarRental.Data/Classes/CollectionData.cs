using System.Linq.Expressions;
using System.Reflection;
using CarRental.Common.Extensions;

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

        var temp = 0;
        int? intT = 0;
        intT = null;
        if (intT.IsNullOrEmpty())
            intT = 44;
        if (intT.IsNullOrEmpty())
            intT = 41;
        
        List<int> list = new();
        if (list.IsNullOrEmpty())
            temp++;
        list.Add(temp);
        if (list.IsNullOrEmpty())
            temp++;
        list.Clear();
        if (list.IsNullOrEmpty())
            temp++;
        list = null;
        if (list.IsNullOrEmpty())
            temp++;
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