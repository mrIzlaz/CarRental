using CarRental.Common.Enums;

namespace CarRental.Common.Classes;

public class Car : Vehicle
{
    public Car(int id, string licencePlate, string manufacturer, VehicleTypes vehicleType, int odometer, int dayCost = 0, double kmCost = 0, string description = "")
    {
        Id = id;
        LicencePlate = licencePlate;
        Manufacturer = manufacturer;
        VehicleTypes = vehicleType;
        Odometer = odometer;
        Description = description;
        DayCost = dayCost;
        KmCost = kmCost;
        VehicleStatus = VehicleStatus.Available;
    }
    public override VehicleTypes GetVehicleType() => VehicleTypes;

}
