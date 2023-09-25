using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using System.Text;

namespace CarRental.Data.Classes;

public sealed class DataFactory
{
    private int _customerId = 0;
    private int _carId = 0;
    readonly char[] _charData = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z' };
    private Dictionary<string, IVehicle> _carLib = new Dictionary<string, IVehicle>();
    private Dictionary<string, IBooking> _bookings = new Dictionary<string, IBooking>();
    private List<VehicleManufacturer> _motoMakers = new List<VehicleManufacturer>() { VehicleManufacturer.Toyota, VehicleManufacturer.Bmw, VehicleManufacturer.Honda, VehicleManufacturer.Suzuki };

    /// <summary>
    /// Returns a generated List of IBookings from parameters. Verify there's enough entities in <paramref name="vehiclesForRent"/> for it to succeed.<br></br>
    /// <paramref name="vehicleStatus"/>Defines the booking status for the List of Bookings it will return.
    /// </summary>
    /// <param name="customers">List of Customers elegible for renting a car</param>
    /// <param name="vehiclesForRent">List of IVehicles in vehicle-pool</param>
    /// <param name="vehicleStatus">VehicleStatus for bookings</param>
    /// <param name="numberOfBookings">Number of bookings it will return. If left at 0 it will return a random number between 0 and <paramref name="customers"/>.Count</param>
    /// <returns></returns>
    /// <exception cref="Exception">Throws an exception if it can't pick a car that's not already booked, after 8 trials</exception>
    public List<IBooking> GenerateIBookingsList(List<Customer> customers, List<IVehicle> vehiclesForRent, VehicleStatus vehicleStatus, int numberOfBookings = 0)
    {
        var rnd = new Random();
        if (numberOfBookings <= 0) numberOfBookings = rnd.Next(customers.Count);

        Booking newBooking;
        for (int i = 0; i < numberOfBookings; i++)
        {
            int trials = 8;
            do
            {
                newBooking = GetNewBooking(customers, vehiclesForRent, vehicleStatus);
                if (_bookings.TryAdd(newBooking.LicensePlate(), newBooking))
                    break;
                trials--;
            } while (trials >= 0);

            if (trials <= 0)
            {
                throw new Exception("Bookings failed to complete properly (booking trials exceeded)");
            }
        }

        return GetBookings(vehicleStatus);
    }


    public List<IPerson> GenerateIPersonList(int numberOfPersons = 4)
    {
        if (numberOfPersons < 1) throw new Exception($"numberOfPersons needs to have at least 1, had {numberOfPersons}");

        var rnd = new Random();
        List<string> firstNames = new List<string>() { "Margot", "Astrid", "Charles", "Sean", "Crow", "Welsh", "Tim", "Bob", "Clarence", "Eva", "Lena", "Thomas", "Kent", "Sam", "Jonas", "Rikard", "Kalle", "Frank", "Tina", "Alberg", "Robert", "Titti", "Hubertius" };
        List<string> lastNames = new List<string>() { "Andersson", "Karlsson", "Rayden", "Russel", "Taylor", "Birdie", "Hitchcock", "Penn", "Bacon", "Smith", "Kimchi", "Clarkson", "Edelblomberg", "Booker", "Crook", "Smoker", "Webber", "Ramsey" };

        List<IPerson> list = new List<IPerson>();

        try
        {
            for (int i = 0; i < numberOfPersons; i++)
            {
                string firtName = firstNames[rnd.Next(firstNames.Count - 1)];
                string lastName = lastNames[rnd.Next(lastNames.Count - 1)];
                long seSsn = GenerateSE_SSN();
                long ssn = GenerateSsn();
                DateOnly date = GenerateDate(GeneratedDateVariants.RegistryDate);
                var cus = new Customer(firtName, lastName, ssn, seSsn, date, _customerId);
                _customerId++;
                list.Add(cus);
            }
        }
        catch (ArgumentException e)
        {
            throw e;
        }
        catch
        {
            throw new Exception("Unhandled exception");
        }
        return list;
    }

    public List<IVehicle> GenerateIVehicleList(int numberOfVehicles = 8)
    {
        var list = new List<IVehicle>();
        var plates = GetLicencePlateList(numberOfVehicles);
        int fails = 0;
        int trials = numberOfVehicles * 2;
        foreach (var plate in plates)
        {
            var vehicle = GetVehicle(plate, _carId);
            if (_carLib.TryAdd(plate, vehicle))
            {
                _carId++;
                list.Add(vehicle);
            }
            else { fails++; }
        }
        while (fails > 0 && trials > 0)
        {
            trials--;
            var plate = GetSingleLicencePlate();
            var vehicle = GetVehicle(plate, _carId);
            if (_carLib.TryAdd(plate, vehicle))
            {
                _carId++;
                fails--;
                list.Add(vehicle);
            }
        }

        return list;

    }

    #region Helper Methods

    private IVehicle GetVehicle(string licencePlate, int id)
    {
        var rnd = new Random();
        bool car = rnd.Next(10) <= 5;
        VehicleManufacturer manu = car ? (VehicleManufacturer)rnd.Next(0, Manufacturer.GetTotalCarManufacturers()) : _motoMakers[rnd.Next(_motoMakers.Count)];
        VehicleTypes type = (VehicleTypes)rnd.Next(0, 3);
        int odo = rnd.Next(1000, 20000);

        IVehicle vehicle = car ? new Car(id, licencePlate, manu.ToString(), type, odo, (int)GetVehicleCost(manu), GetVehicleCost(manu, true), "") : new Motorcycle(id, licencePlate, manu.ToString(), odo, (int)GetVehicleCost(manu), GetVehicleCost(manu, true), "");
        return vehicle;
    }

    private Booking GetNewBooking(List<Customer> customers, List<IVehicle> vehiclesForRent, VehicleStatus bookingStatus)
    {
        var rnd = new Random();
        var vehicle = GetUnbookedVehicle(vehiclesForRent);
        var customer = TryGetAvailableCustomer(customers);
        var startDate = GenerateDate(GeneratedDateVariants.BookingsDate).ToDateTime(new TimeOnly());
        Booking newBooking = new Booking(vehicle, customer, startDate);
        switch (bookingStatus)
        {
            case VehicleStatus.Available:
                newBooking.TryCloseBooking(GetReturnDate(startDate), newBooking.GetOdometerStart() + rnd.Next(100, 1200));
                return newBooking;
            case VehicleStatus.Booked:
                return newBooking;
            case VehicleStatus.Unavailable:
                var note = "At repairshop til return";
                return new Booking(vehicle, customer, startDate, GetReturnDate(startDate), bookingStatus, note);
            default:
                return newBooking;
        }

    }

    private Customer TryGetAvailableCustomer(List<Customer> customers)
    {
        var rnd = new Random();
        var customer = customers[rnd.Next(customers.Count - 1)];
        foreach (var booking in _bookings)
        {
            if (booking.Value.CustomerId() == customer.CustomerId)
            {
                customer = customers[rnd.Next(customers.Count - 1)]; //TODO: CHECK THIS CODE. IS NOT TESTED!!!!!!
            }
        }
        return customer;
    }

    private IVehicle GetUnbookedVehicle(List<IVehicle> vehiclesForRent)
    {
        var rnd = new Random();
        var vehicle = vehiclesForRent[rnd.Next(vehiclesForRent.Count - 1)];
        while (_bookings.ContainsKey(vehicle.GetLicencePlate()))
            vehicle = vehiclesForRent[rnd.Next(vehiclesForRent.Count - 1)];

        return vehicle;
    }
    private List<IBooking> GetBookings(VehicleStatus vehicleStatus)
    {
        var list = new List<IBooking>();
        foreach (var booking in _bookings)
        {
            if (booking.Value.GetBookingStatus() == vehicleStatus)
                list.Add(booking.Value);
        }
        return list;
    }

    private DateTime GetReturnDate(DateTime startDate, bool resetToToday = true)
    {
        var returnDate = startDate.AddDays(new Random().Next(5, 90));
        if (resetToToday && returnDate.DayOfYear > DateTime.Today.DayOfYear)
            return DateTime.Today;
        else
            return returnDate;
    }

    private double GetVehicleCost(VehicleManufacturer manufacturer, bool milageCost = false)
    {
        double cost = 0;
        switch (manufacturer)
        {
            case VehicleManufacturer.Volvo:
                cost = milageCost ? 1.25d : 210;
                break;
            case VehicleManufacturer.Kia:
                cost = milageCost ? 0.88d : 200;
                break;
            case VehicleManufacturer.Jeep:
                cost = milageCost ? 2.12d : 230;
                break;
            case VehicleManufacturer.Ford:
                cost = milageCost ? 1.2d : 200;
                break;
            case VehicleManufacturer.Toyota:
                cost = milageCost ? 1.5d : 110;
                break;
            case VehicleManufacturer.Škoda:
                cost = milageCost ? 1.22d : 140;
                break;
            case VehicleManufacturer.Rivian:
                cost = milageCost ? 0.85d : 300;
                break;
            case VehicleManufacturer.Bmw:
                cost = milageCost ? 2.1d : 230;
                break;
            case VehicleManufacturer.Honda:
                cost = milageCost ? 1.52d : 100;
                break;
            case VehicleManufacturer.Suzuki:
                cost = milageCost ? 1.12d : 190;
                break;
        }
        return cost;
    }

    private string GetSingleLicencePlate()
    {
        var rnd = new Random();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 3; i++)
            sb.Append(_charData[rnd.Next(0, _charData.Length - 1)]);
        sb.Append(" " + String.Format("{0:000}", rnd.Next(0, 999)));
        return sb.ToString();
    }

    private long GenerateSE_SSN()
    {
        var rnd = new Random();
        StringBuilder sb = new StringBuilder();
        var date = GenerateDate(GeneratedDateVariants.DateOfBirth);
        var month = date.Month.ToString("D2");
        var day = date.Day.ToString("D2");
        var digits = rnd.Next(1, 9999).ToString("D4");
        sb.Append(date.Year.ToString().Substring(2, 2));
        sb.Append(month);
        sb.Append(day);
        sb.Append(digits);
        var text = sb.ToString();
        _ = long.TryParse(text, out long result);
        return result;
    }
    private long GenerateSsn()
    {
        var rnd = new Random();
        return rnd.NextInt64(100000000, 999999999);
    }

    private DateOnly GenerateDate(GeneratedDateVariants dateVariant)
    {
        var rnd = new Random();
        var date = new DateOnly();
        switch (dateVariant)
        {
            case GeneratedDateVariants.DateOfBirth:
                date = new DateOnly(rnd.Next(1940, 2003), rnd.Next(1, 12), rnd.Next(1, 31));
                break;
            case GeneratedDateVariants.RegistryDate:
                date = new DateOnly(rnd.Next(2020, 2023), rnd.Next(1, 12), rnd.Next(1, 31));
                break;
            case GeneratedDateVariants.BookingsDate:
                date = new DateOnly(2023, DateTime.Now.Month, rnd.Next(1, DateTime.Now.Day));
                break;
        }
        //var date = new DateOnly(bool ? rnd.Next(1940, 2003) : rnd.Next(2020, 2023), rnd.Next(1, 12), rnd.Next(1, 31));
        return date;
    }

    private List<string> GetLicencePlateList(int nr = 1)
    {
        var list = new List<string>();
        for (int i = 0; i < nr; i++) list.Add(GetSingleLicencePlate());
        return list;
    }

    #endregion

    enum GeneratedDateVariants
    {
        DateOfBirth,
        RegistryDate,
        BookingsDate
    }
}
