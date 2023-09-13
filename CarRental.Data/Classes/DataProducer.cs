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
    List<string> modelData = new List<string>();
    readonly char[] charData = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z' };
    Dictionary<string, IVehicle> CarLib = new Dictionary<string, IVehicle>();

    public DataProducer()
    {



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
                DateOnly date = GenerateDate();
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
        while (fails > 0 || trials > 0)
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
        bool car = rnd.Next(10) <= 9;
        CarManufacturer manu = (CarManufacturer)rnd.Next(0, 6);
        VehicleTypes type = (VehicleTypes)rnd.Next(0, 3);
        int odo = rnd.Next(1000, 20000);

        IVehicle vehicle = car ? new Car(id, licencePlate, manu.ToString(), type, odo, (int)GetCost(manu), GetCost(manu, true), "") : new Motorcycle(id, licencePlate, manu.ToString(), odo, (int)GetCost(manu), GetCost(manu, true), "");
        return vehicle;
    }

    private double GetCost(CarManufacturer manufacturer, bool milageCost = false)
    {
        double cost = 0;
        switch (manufacturer)
        {
            case CarManufacturer.Volvo:
                cost = milageCost ? 1.2d : 210;
                break;
            case CarManufacturer.KIA:
                cost = milageCost ? 0.8d : 200;
                break;
            case CarManufacturer.Jeep:
                cost = milageCost ? 2.1d : 230;
                break;
            case CarManufacturer.Ford:
                cost = milageCost ? 1.2d : 200;
                break;
            case CarManufacturer.Toyota:
                cost = milageCost ? 1.5d : 110;
                break;
            case CarManufacturer.Škoda:
                cost = milageCost ? 1.2d : 140;
                break;
            case CarManufacturer.Rivian:
                cost = milageCost ? 0.8d : 300;
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
        return GenerateDate(true).ToShortDateString() + rnd.Next(1000, 9999);
    }

    private DateOnly GenerateDate(bool generateDOB = false)
    {
        var rnd = new Random();
        var date = new DateOnly(generateDOB ? rnd.Next(1940, 2003) : rnd.Next(2020, 2023), rnd.Next(1, 12), rnd.Next(1, 31));
        return date;
    }

    private List<string> GetLicencePlateList(int nr = 1)
    {
        var list = new List<string>();
        for (int i = 0; i < nr; i++) list.Add(GetSingleLicencePlate());
        return list;
    }

    #endregion

    enum CarManufacturer
    {
        Volvo,
        KIA,
        Jeep,
        Ford,
        Toyota,
        Škoda,
        Rivian
    }
}
