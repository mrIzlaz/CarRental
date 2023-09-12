namespace CarRental.Data.Classes;

using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using CarRental.Data.Interfaces;
using System.Collections.Generic;
using System.Text.Json;

public class CollectionData : IData
{
    readonly List<IPerson> _persons = new List<IPerson>();
    readonly List<IVehicle> _vehicles = new List<IVehicle>();
    readonly List<IBooking> _bookings = new List<IBooking>();

    public CollectionData() => SeedData();

    public IEnumerable<IBooking> GetBookings() => _bookings;
    public IEnumerable<IPerson> GetPersons() => _persons;


    public IEnumerable<IVehicle> GetVehicles(VehicleStatus status = 0)
    {
        return _vehicles;
    }

    #region VG
    /*
        public IEnumerator<List<T>> Get();
        public IEnumerator<Generic<T>> Single();
        public void Add(<T>);
    */
    #endregion

    private VehicleData[]? vehicleData;
    private BookingData[]? bookingData;
    private CustomerData[]? customerData;

    public void DebugCall()
    {
        SeedData();
    }
    void SeedData()
    {
        string text = File.ReadAllText(@".Seed-Data\bookings.json");
        vehicleData = JsonSerializer.Deserialize<VehicleData[]>(text);
        text = File.ReadAllText(@".Seed-Data\vehicles.json");
        bookingData = JsonSerializer.Deserialize<BookingData[]>(text);
        text = File.ReadAllText(@".Seed-Data\customers.json");
        customerData = JsonSerializer.Deserialize<CustomerData[]>(text);

    }

    public class VehicleData
    {
        public required string RegNum { get; set; }
        public required string Make { get; set; }
        public int Odometer { get; set; }
        public double CostKilometer { get; set; }
        public VehicleTypes vehicleType { get; set; }
        public double CostDay { get; set; }
        public VehicleStatus vehicleStatus { get; set; }

    }
    public class BookingData
    {
        public required string RegNum { get; set; }
        public required string Customer { get; set; }
        public int OdometerStartKm { get; set; }
        public int OdometerReturnKm { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public double TotalCost { get; set; }
        public VehicleStatus vehicleStatus { get; set; }

    }
    public class CustomerData
    {
        public int SocialSecurityNumber { get; set; }
        public required string Surname { get; set; }
        public required string FirstName { get; set; }
        public DateTime RegistryDate { get; set; }
    }
}