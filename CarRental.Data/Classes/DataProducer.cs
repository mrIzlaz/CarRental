using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using System.Numerics;
using System.Text;

namespace CarRental.Data.Classes;

public sealed class DataProducer
{
    private int customerId = 0;
    private int carID = 0;
    readonly char[] charData = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z' };
    Dictionary<string, IVehicle> CarLib = new Dictionary<string, IVehicle>();
    List<VehicleManufacturer> motoMakers = new List<VehicleManufacturer>() { VehicleManufacturer.Toyota, VehicleManufacturer.BMW, VehicleManufacturer.Honda, VehicleManufacturer.Suzuki };
    public DataProducer()
    {



    }

    public List<IBooking> GenerateIBookingsList(List<Customer> customers, List<IVehicle> vehiclesForRent, int numberOfBookings = 0)
    {
        var rnd = new Random();
        var bookingsList = new List<IBooking>();
        if (numberOfBookings <= 0) numberOfBookings = rnd.Next(customers.Count);

       /* for (int i = 0; i < numberOfBookings; i++)
        {
            if (i == 0)
                var newBooking = new Booking(vehiclesForRent[rnd.Next(vehiclesForRent.Count - 1)], customers[rnd.Next(customers.Count - 1)], GenerateDate(GeneratedDateVariants.BookingsDate));
        }
       */
        throw new NotImplementedException();
    }

    public List<IPerson> GenerateIPersonList(int numberOfPersons = 4)
    {
        if (numberOfPersons < 1) throw new Exception($"numberOfPersons needs to have at least 1, had {numberOfPersons}");

        var rnd = new Random();
        List<string> LastNames = new List<string>() { "Andersson", "Karlsson", "Rayden", "Russel", "Taylor", "Birdie", "Hitchcock", "Penn", "Bacon" };
        List<string> FirstNames = new List<string>() { "Margot", "Astrid", "Charles", "Sean", "Crow", "Welsh", "Tim", "Bob" };

        List<IPerson> list = new List<IPerson>();

        try
        {
            for (int i = 0; i < numberOfPersons; i++)
            {
                string firtName = FirstNames[rnd.Next(FirstNames.Count - 1)];
                string lastName = LastNames[rnd.Next(LastNames.Count - 1)];
                string SSN = GenerateSSN();
                DateOnly date = GenerateDate(GeneratedDateVariants.RegistryDate);
                var cus = new Customer(firtName, lastName, SSN, date, customerId);
                customerId++;
                list.Add(cus);
            }
        }
        catch (ArgumentException e)
        {
            throw e;
        }
        catch
        {
            throw new Exception("Whatdefuq");
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
            var vehicle = GetVehicle(plate, carID);
            if (CarLib.TryAdd(plate, vehicle))
            {
                carID++;
                list.Add(vehicle);
            }
            else { fails++; }
        }
        while (fails > 0 && trials > 0)
        {
            trials--;
            var plate = GetSingleLicencePlate();
            var vehicle = GetVehicle(plate, carID);
            if (CarLib.TryAdd(plate, vehicle))
            {
                carID++;
                fails--;
                list.Add(vehicle);
            }
        }

        return list;

    }

    #region Help Methods

    private IVehicle GetVehicle(string licencePlate, int id)
    {
        var rnd = new Random();
        bool car = rnd.Next(10) <= 5;
        VehicleManufacturer manu = car ? (VehicleManufacturer)rnd.Next(0, totalCarManu) : motoMakers[rnd.Next(motoMakers.Count)];
        VehicleTypes type = (VehicleTypes)rnd.Next(0, 3);
        int odo = rnd.Next(1000, 20000);

        IVehicle vehicle = car ? new Car(id, licencePlate, manu.ToString(), type, odo, (int)GetCost(manu), GetCost(manu, true), "") : new Motorcycle(id, licencePlate, manu.ToString(), odo, (int)GetCost(manu), GetCost(manu, true), "");
        return vehicle;
    }

    private double GetCost(VehicleManufacturer manufacturer, bool milageCost = false)
    {
        double cost = 0;
        switch (manufacturer)
        {
            case VehicleManufacturer.Volvo:
                cost = milageCost ? 1.25d : 210;
                break;
            case VehicleManufacturer.KIA:
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
            case VehicleManufacturer.BMW:
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
            sb.Append(charData[rnd.Next(0, charData.Length - 1)]);
        sb.Append(" " + String.Format("{0:000}", rnd.Next(0, 999)));
        return sb.ToString();
    }

    private string GenerateSSN()
    {
        var rnd = new Random();
        return GenerateDate(GeneratedDateVariants.DateOfBirth).ToShortDateString() + rnd.Next(1000, 9999);
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

    private const int totalCarManu = 10;
    enum VehicleManufacturer
    {
        Toyota,
        BMW,
        Honda,
        Suzuki,
        Volvo,
        KIA,
        Jeep,
        Ford,
        Škoda,
        Rivian
    }

    enum GeneratedDateVariants
    {
        DateOfBirth,
        RegistryDate,
        BookingsDate
    }
}
