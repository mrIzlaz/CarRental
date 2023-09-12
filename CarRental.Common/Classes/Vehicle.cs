using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public abstract class Vehicle : IVehicle
{
    private readonly int _id;
    private int ID { get; init; }
    public required string LicencePlate { get; set; }

    public required string Manufacturer { get; set; }
    public required string Model { get; set; }

    public required VehicleTypes VehicleTypes { get; set; }
    public required VehicleStatus VehicleStatus { get; set; }

    private int _odometer = 0;
    public required int Odometer { get => _odometer; set => _odometer += value; }

    public int dayCost = 0;
    public int kmCost = 0;

    private string? Description { get; set; }

    public string GetLicencePlate() => LicencePlate;
    public string GetManufacturer() => Manufacturer;
    public string? GetDescription() => Description;
    public abstract VehicleTypes GetVehicleType();
    public int GetOdometer() => Odometer;
    public int GetDayCost() => dayCost;
    public int GetKmCost() => kmCost;
    public VehicleStatus GetBookingStatus() => VehicleStatus;
}
