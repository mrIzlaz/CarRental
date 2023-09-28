using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using System.Text;

namespace CarRental.Data.Classes;

public sealed class DataFactory
{
    private static int NumberOfVehicleStatus => Enum.GetNames(typeof(VehicleStatus)).Length;

    private int _customerId;
    private int _carId;

    private readonly char[] _charData =
    {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'W', 'X',
        'Y', 'Z'
    };

    private readonly Dictionary<string, Vehicle> _carLib = new();
    private readonly Dictionary<string, IBooking> _bookings = new();


    private readonly IEnumerable<Vehicle> _iVehicles;
    private readonly IEnumerable<IPerson> _iPersons;
    private readonly IEnumerable<IBooking> _iBookings;

    public DataFactory(int vehicleCount = 8, int customerCount = 4, int bookingCount = 3)
    {
        _iVehicles = GenerateIVehicleList(vehicleCount);
        _iPersons = GenerateIPersonList(customerCount);
        _iBookings = GenerateIBookingsList(_iPersons.Cast<Customer>().ToList(), _iVehicles.ToList(), bookingCount);
    }

    public IEnumerable<Vehicle> GetVehicles() => _iVehicles;
    public IEnumerable<IPerson> GetPersons() => _iPersons;
    public IEnumerable<IBooking> GetBookings() => _iBookings;


    #region ListGenerators

    private IEnumerable<IBooking> GenerateIBookingsList(IReadOnlyList<Customer> customers,
        IReadOnlyList<Vehicle> vehiclesForRent,
        int numberOfBookings = 0)
    {
        var rnd = new Random();
        var enumerable = customers.ToList();
        if (numberOfBookings <= 0) numberOfBookings = rnd.Next(enumerable.Count);

        for (var i = 0; i < numberOfBookings; i++)
        {
            var trials = 8;
            do
            {
                var typeOfBooking = (VehicleStatus)rnd.Next(0, NumberOfVehicleStatus);
                var newBooking = GetNewBooking(enumerable, vehiclesForRent, typeOfBooking);
                if (_bookings.TryAdd(newBooking.Vehicle.LicencePlate, newBooking))
                    break;
                trials--;
            } while (trials >= 0);

            if (trials <= 0)
            {
                throw new Exception("Bookings failed to complete properly (booking trials exceeded)");
            }
        }

        return _bookings.Values.ToList();
    }


    private IEnumerable<IPerson> GenerateIPersonList(int numberOfPersons = 4)
    {
        if (numberOfPersons < 1)
            throw new Exception($"numberOfPersons needs to have at least 1, had {numberOfPersons}");

        var rnd = new Random();
        var firstNames = new List<string>()
        {
            "Margot", "Astrid", "Charles", "Sean", "Crow", "Welsh", "Tim", "Bob", "Clarence", "Eva", "Lena", "Thomas",
            "Kent", "Sam", "Jonas", "Rikard", "Kalle", "Frank", "Tina", "Albert", "Robert", "Titti", "Hubertius"
        };
        var lastNames = new List<string>()
        {
            "Andersson", "Karlsson", "Rayden", "Russel", "Taylor", "Birdie", "Hitchcock", "Penn", "Bacon", "Smith",
            "Kimi", "Clarkson", "Edelblomberg", "Booker", "Crook", "Smoker", "Webber", "Ramsey"
        };

        var list = new List<IPerson>();

        try
        {
            for (var i = 0; i < numberOfPersons; i++)
            {
                var firstName = firstNames[rnd.Next(firstNames.Count - 1)];
                var lastName = lastNames[rnd.Next(lastNames.Count - 1)];
                var ssn = GenerateSsn();
                var date = GenerateDate(GeneratedDateVariants.RegistryDate);
                var cus = new Customer(firstName, lastName, ssn, date, _customerId);
                _customerId++;
                list.Add(cus);
            }
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch
        {
            throw new Exception("Unhandled exception");
        }

        return list;
    }

    private IEnumerable<Vehicle> GenerateIVehicleList(int numberOfVehicles = 8)
    {
        var list = new List<Vehicle>();
        var plates = GetLicencePlateList(numberOfVehicles);
        var fails = 0;
        var trials = numberOfVehicles * 2;
        foreach (var plate in plates)
        {
            var vehicle = GetVehicle(plate, _carId);
            if (_carLib.TryAdd(plate, vehicle))
            {
                _carId++;
                list.Add(vehicle);
            }
            else
                fails++;
        }

        while (fails > 0 && trials > 0)
        {
            trials--;
            var plate = GetSingleLicencePlate();
            var vehicle = GetVehicle(plate, _carId);
            if (!_carLib.TryAdd(plate, vehicle)) continue;
            _carId++;
            fails--;
            list.Add(vehicle);
        }

        return list;
    }

    #endregion

    #region Helper Methods

    private static Vehicle GetVehicle(string licencePlate, int id)
    {
        var rnd = new Random();
        var car = rnd.Next(10) <= 5;
        var manu = car
            ? (VehicleManufacturer)rnd.Next(0, TotalCarManu)
            : Manufacturer.MotoMakers[rnd.Next(Manufacturer.MotoMakers.Count)];
        var type = (VehicleType)rnd.Next(0, 3);
        var odo = rnd.Next(1000, 20000);

        Vehicle vehicle = car
            ? new Car(id, licencePlate, manu.ToString(), odo, type, (int)GetVehicleCost(manu),
                GetVehicleCost(manu, true))
            : new Motorcycle(id, licencePlate, manu.ToString(), odo, (int)GetVehicleCost(manu),
                GetVehicleCost(manu, true));
        return vehicle;
    }

    private Booking GetNewBooking(IEnumerable<IPerson> customers, IReadOnlyList<Vehicle> vehiclesForRent,
        VehicleStatus bookingStatus)
    {
        var customerList = customers.Cast<Customer>().ToList();
        var rnd = new Random();
        var vehicle = GetUnbookedVehicle(vehiclesForRent);
        var customer = TryGetAvailableCustomer(customerList);
        var startDate = GenerateDate(GeneratedDateVariants.BookingsDate).ToDateTime(new TimeOnly());
        var newBooking = new Booking(vehicle, customer, startDate);
        switch (bookingStatus)
        {
            case VehicleStatus.Available:
                newBooking.TryCloseBooking(GetReturnDate(startDate),
                    newBooking.OdometerStart + rnd.Next(100, 1200));
                return newBooking;
            case VehicleStatus.Booked:
                return newBooking;
            case VehicleStatus.Unavailable:
                return new Booking(vehicle, customer, startDate, GetReturnDate(startDate), bookingStatus);
            default:
                return newBooking;
        }
    }

    private Customer TryGetAvailableCustomer(IReadOnlyList<Customer> customers)
    {
        var rnd = new Random();
        var customer = customers[rnd.Next(customers.Count - 1)];
        foreach (var booking in _bookings.Where(booking => booking.Value.Customer.CustomerId == customer.CustomerId))
        {
            customer = customers[rnd.Next(customers.Count - 1)];
        }

        return customer;
    }

    private Vehicle GetUnbookedVehicle(IReadOnlyList<Vehicle> vehiclesForRent)
    {
        var rnd = new Random();
        var vehicle = vehiclesForRent[rnd.Next(vehiclesForRent.Count - 1)];
        while (_bookings.ContainsKey(vehicle.LicencePlate))
            vehicle = vehiclesForRent[rnd.Next(vehiclesForRent.Count - 1)];

        return vehicle;
    }

    private static DateTime GetReturnDate(DateTime startDate, bool resetToToday = true)
    {
        var returnDate = startDate.AddDays(new Random().Next(5, 90));
        if (resetToToday && returnDate.DayOfYear > DateTime.Today.DayOfYear)
            return DateTime.Today;
        else
            return returnDate;
    }

    private static double GetVehicleCost(VehicleManufacturer manufacturer, bool mileageCost = false)
    {
        var cost = manufacturer switch
        {
            VehicleManufacturer.Volvo => mileageCost ? 1.25d : 210,
            VehicleManufacturer.Kia => mileageCost ? 0.88d : 200,
            VehicleManufacturer.Jeep => mileageCost ? 2.12d : 230,
            VehicleManufacturer.Ford => mileageCost ? 1.2d : 200,
            VehicleManufacturer.Toyota => mileageCost ? 1.5d : 110,
            VehicleManufacturer.Škoda => mileageCost ? 1.22d : 140,
            VehicleManufacturer.Rivian => mileageCost ? 0.85d : 300,
            VehicleManufacturer.Bmw => mileageCost ? 2.1d : 230,
            VehicleManufacturer.Honda => mileageCost ? 1.52d : 100,
            VehicleManufacturer.Suzuki => mileageCost ? 1.12d : 190,
            _ => 0
        };
        return cost;
    }

    private string GetSingleLicencePlate()
    {
        var rnd = new Random();
        var sb = new StringBuilder();
        for (var i = 0; i < 3; i++)
            sb.Append(_charData[rnd.Next(0, _charData.Length - 1)]);
        sb.Append(" " + $"{rnd.Next(0, 999):000}");
        return sb.ToString();
    }

    private static long GenerateSsn()
    {
        var rnd = new Random();
        return rnd.NextInt64(100000000, 999999999);
    }

    private static DateOnly GenerateDate(GeneratedDateVariants dateVariant)
    {
        var rnd = new Random();
        var date = dateVariant switch
        {
            GeneratedDateVariants.DateOfBirth => new DateOnly(rnd.Next(1940, 2003), rnd.Next(1, 12), rnd.Next(1, 31)),
            GeneratedDateVariants.RegistryDate => new DateOnly(rnd.Next(2020, 2023), rnd.Next(1, DateTime.Today.Month),
                rnd.Next(1, DateTime.Today.Day)),
            GeneratedDateVariants.BookingsDate => new DateOnly(2023, DateTime.Now.Month, rnd.Next(1, DateTime.Now.Day)),
            _ => new DateOnly()
        };
        return date;
    }

    private List<string> GetLicencePlateList(int nr = 1)
    {
        var list = new List<string>();
        for (var i = 0; i < nr; i++) list.Add(GetSingleLicencePlate());
        return list;
    }

    #endregion

    private static int TotalCarManu => Enum.GetNames(typeof(VehicleManufacturer)).Length;

    private enum GeneratedDateVariants
    {
        DateOfBirth,
        RegistryDate,
        BookingsDate
    }
}