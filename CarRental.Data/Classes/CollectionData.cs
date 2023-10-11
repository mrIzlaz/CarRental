using System.Linq.Expressions;
using System.Reflection;

namespace CarRental.Data.Classes;

using Common.Enums;
using CarRental.Common.Interfaces;
using Interfaces;
using System.Collections.Generic;
using CarRental.Common.Classes;

public class CollectionData : IData
{
    public int NextPersonId => _persons.Count + 1;
    public int NextVehicleId => _vehicles.Count + 1;
    public int NextBookingId => _bookings.Count + 1;

    private readonly DataFactory _producer = new();
    public CollectionData() => SeedData();

    private readonly List<IPerson> _persons = new();
    private readonly List<Vehicle> _vehicles = new();
    private readonly List<IBooking> _bookings = new();
    public IEnumerable<IBooking> GetBookings() => _bookings.OrderByDescending(x => x.BookingStatus).Reverse();
    public IEnumerable<IPerson> GetPersons() => _persons;

    public IEnumerable<Vehicle> GetVehicles(VehicleStatus status = default) =>
        status == default ? _vehicles : _vehicles.Where(x => x.VehicleStatus == status);

    public T? Single<T>(Expression<Func<T, bool>>? expression)
    {
        var func = expression?.Compile();
        GetListFrom<T>(out var list);
        if (list == null) return default;
        return func != null ? list.SingleOrDefault(func) : default;
    }

    public List<T> Get<T>(Expression<Func<T, bool>>? expression)
    {
        var func = expression?.Compile();
        GetListFrom<T>(out var list);
        if (list == null) return new List<T>();
        return func == null ? list : list.Where(func).ToList();
    }

    public void Add<T>(T item)
    {
        GetListFrom<T>(out var list);
        list?.Add(item);
    }

    private void GetListFrom<T>(out List<T>? list)
    {
        FieldInfo[] fieldInfo = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        list = fieldInfo.FirstOrDefault(f => f.FieldType == typeof(List<T>))?.GetValue(this) as List<T>;
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