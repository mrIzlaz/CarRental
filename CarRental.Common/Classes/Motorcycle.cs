using CarRental.Common.Enums;

namespace CarRental.Common.Classes;

public class Motorcycle : Vehicle
{
    public Motorcycle(int id, string licencePlate, string manufacturer, int odometer, int dayCost = 0,
        double kmCost = 0) : base(id, licencePlate, manufacturer, odometer, VehicleType.Motorcycle, dayCost, kmCost)
    {
        Id = id;
        LicencePlate = licencePlate;
        Manufacturer = manufacturer;
        VehicleType = VehicleType.Motorcycle;
        Odometer = odometer;
        DayCost = dayCost;
        KmCost = kmCost;
        VehicleStatus = VehicleStatus.Available;
    }
}