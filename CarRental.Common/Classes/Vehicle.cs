using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public abstract class Vehicle : IVehicle
{

    private readonly int _id;
    protected int ID { get; init; }
    public string LicencePlate { get; init; }
    public string Manufacturer { get; init; }
    public VehicleTypes VehicleTypes { get; set; }
    public VehicleStatus VehicleStatus { get; set; }

    private int _odometer = 0;
    public int Odometer { get => _odometer; set => _odometer += value; }

    public int dayCost = 0;
    public double kmCost = 0;



    protected string? Description { get; set; }

    public string GetLicencePlate() => LicencePlate;
    public string GetManufacturer() => Manufacturer;
    public string? GetDescription() => Description;
    public abstract VehicleTypes GetVehicleType();
    public int GetOdometer() => Odometer;
    public int GetDayCost() => dayCost;
    public double GetKmCost() => kmCost;
    public VehicleStatus GetBookingStatus() => VehicleStatus;
}
