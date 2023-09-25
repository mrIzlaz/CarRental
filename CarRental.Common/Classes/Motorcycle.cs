using CarRental.Common.Enums;

namespace CarRental.Common.Classes;

public class Motorcycle : Vehicle
{
    public Motorcycle(int id, string licencePlate, string manufacturer, int odometer, int dayCost = 0,
        double kmCost = 0) : base(id, licencePlate, manufacturer, odometer, VehicleTypes.Motorcycle, dayCost, kmCost)
    {
        Id = id;
        LicencePlate = licencePlate;
        Manufacturer = manufacturer;
        VehicleTypes = VehicleTypes.Motorcycle;
        Odometer = odometer;
        DayCost = dayCost;
        KmCost = kmCost;
        VehicleStatus = VehicleStatus.Available;
    }

    public override VehicleTypes GetVehicleType() => VehicleTypes;
}