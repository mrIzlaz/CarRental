using System.Linq.Expressions;
using System.Reflection;

namespace CarRental.Data.Classes;

using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using CarRental.Data.Interfaces;
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

    public IEnumerable<Vehicle> GetVehicles(VehicleStatus status = default)
    {
        return status == default ? _vehicles : _vehicles.Where(x => x.VehicleStatus == status);
    }

    public T? Single<T>(Expression<Func<T, bool>>? expression)
    {
        throw new NotImplementedException();
        var comp = expression?.Compile();
    }

    public List<T>? Get<T>(Expression<Func<T, bool>>? expression)
    {
        if (expression == null) return null;
        FieldInfo[] myFieldInfo;
        Type myType = typeof(T);
        myFieldInfo = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        for (int i = 0; i < myFieldInfo.Length; i++)
        {
            Console.WriteLine("\nName            : {0}", myFieldInfo[i].Name);
            Console.WriteLine("Declaring Type  : {0}", myFieldInfo[i].DeclaringType);
            Console.WriteLine("IsPublic        : {0}", myFieldInfo[i].IsPublic);
            Console.WriteLine("MemberType      : {0}", myFieldInfo[i].MemberType);
            Console.WriteLine("FieldType       : {0}", myFieldInfo[i].FieldType);
            Console.WriteLine("IsFamily        : {0}", myFieldInfo[i].IsFamily);
        }

        return null;
        var reflection = expression.Compile();
        //GetFields ska du hämta alla fields som möter ett kriterie 
        var comp = expression?.Compile();
        if (typeof(T) == typeof(Vehicle))
            return comp == null
                ? _vehicles.Cast<T>().ToList()
                : _vehicles.Where(veh => comp((T)(object)veh)).Cast<T>().ToList();
        if (typeof(T) == typeof(Customer))
            return comp == null
                ? _persons.Cast<T>().ToList()
                : _persons.Where(cus => comp((T)cus)).Cast<T>().ToList();
        if (typeof(T) == typeof(Booking))
            return comp == null
                ? _bookings.Cast<T>().ToList()
                : _bookings.Where(boo => comp((T)boo)).Cast<T>().ToList();

        throw new InvalidOperationException("Unsupported type");
    }


    public void Add<T>(T item)
    {
        if (item == null) throw new ArgumentNullException();
        switch (item)
        {
            case Customer customer:
                _persons.Add(new Customer(customer, _persons.Count + 1));
                break;
            case Booking booking:
                _bookings.Add(booking);
                break;
            case Vehicle vehicle:
                Vehicle newVeh = vehicle.VehicleType == VehicleType.Motorcycle
                    ? new Motorcycle(_vehicles.Count + 1, vehicle)
                    : new Car(_vehicles.Count + 1, vehicle);
                _vehicles.Add(newVeh);
                break;
        }
    }

    private void SeedData()
    {
        try
        {
            _persons.AddRange(_producer.GetPersons());
            _vehicles.AddRange(_producer.GetVehicles());
            _bookings.AddRange(_producer.GetBookings());
        }
        catch (Exception ex)
        {
            var mes = ex.Message;
        }
    }
}