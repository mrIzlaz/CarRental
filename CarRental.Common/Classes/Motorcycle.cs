using CarRental.Common.Enums;

namespace CarRental.Common.Classes;

public class Motorcycle : Vehicle
{
    public Motorcycle(int id,Vehicle motorcycle) : base(id, motorcycle.LicencePlate, motorcycle.Manufacturer, motorcycle.Odometer, VehicleType.Motorcycle, motorcycle.DayCost, motorcycle.KmCost)
    {
        Id = id;
    } 
    public Motorcycle(int id, string licencePlate, string manufacturer, int odometer, int dayCost = 0,
        double kmCost = 0) : base(id, licencePlate, manufacturer, odometer, VehicleType.Motorcycle, dayCost, kmCost)
    {
        Id = id;
    }

    public Motorcycle(string licencePlate, string manufacturer, int odometer, int dayCost = 0,
        double kmCost = 0) : base(licencePlate, manufacturer, odometer, VehicleType.Motorcycle, dayCost, kmCost)
    {
        LicencePlate = licencePlate;
        Manufacturer = manufacturer;
        VehicleType = VehicleType.Motorcycle;
        Odometer = odometer;
        DayCost = dayCost;
        KmCost = kmCost;
        VehicleStatus = VehicleStatus.Available;
    }
}