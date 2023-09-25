using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public abstract class Vehicle : IVehicle
{
    private readonly int _id;
    protected int Id { get; init; }
    protected string LicencePlate { get; init; }
    protected string Manufacturer { get; init; }
    protected VehicleTypes VehicleTypes { get; init; }
    public VehicleStatus VehicleStatus { get; set; }
    protected int DayCost { get; init; }
    protected double KmCost { get; init; }

    private int _odometer = 0;

    public int Odometer
    {
        get => _odometer;
        set => _odometer += value;
    }


    protected string? Description { get; init; }

    public string GetLicencePlate() => LicencePlate;
    public string GetManufacturer() => Manufacturer;
    public string? GetDescription() => Description;
    public abstract VehicleTypes GetVehicleType();
    public int GetOdometer() => Odometer;
    public int GetDayCost() => DayCost;
    public double GetKmCost() => KmCost;
    public VehicleStatus GetVehicleStatus() => VehicleStatus;
    public void UpdateBookingStatus(VehicleStatus status) => VehicleStatus = status;

    public void UpdateBookingStatus(bool isBooked) =>
        VehicleStatus = isBooked ? VehicleStatus.Booked : VehicleStatus.Available;
}