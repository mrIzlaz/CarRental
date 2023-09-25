using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public abstract class Vehicle : IVehicle
{
    protected int Id { get; init; }
    protected string LicencePlate { get; init; }
    protected string Manufacturer { get; init; }
    protected VehicleTypes VehicleTypes { get; init; }
    public VehicleStatus VehicleStatus { get; set; }
    private int _odometer = 0;
    public int Odometer { get => _odometer; set => _odometer += value; }
    protected int DayCost { get; init; } = 0;
    protected double KmCost { get; init; } = 0;
    protected Vehicle(int id, string licencePlate, string manufacturer, int odometer,VehicleTypes vehicleTypes, int dayCost, double kmCost)
    {
        Id = id;
        LicencePlate = licencePlate;
        Manufacturer = manufacturer;
        _odometer = odometer;
        VehicleTypes = vehicleTypes;
        VehicleStatus = VehicleStatus.Available;
        DayCost = dayCost;
        KmCost = kmCost;
    }

    public string GetLicencePlate() => LicencePlate;
    public string GetManufacturer() => Manufacturer;
    public abstract VehicleTypes GetVehicleType();
    public int GetOdometer() => Odometer;
    public int GetDayCost() => DayCost;
    public double GetKmCost() => KmCost;
    public VehicleStatus GetVehicleStatus() => VehicleStatus;
    public void UpdateBookingStatus(VehicleStatus status) => VehicleStatus = status;
    public void UpdateBookingStatus(bool isBooked) => VehicleStatus = isBooked ? VehicleStatus.Booked : VehicleStatus.Available;
}
