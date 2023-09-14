using CarRental.Common.Enums;
namespace CarRental.Common.Classes;

public class Motorcycle : Vehicle
{
    public Motorcycle(int id, string licencePlate, string manufacturer, int odometer, int dayCost = 0, double kmCost = 0, string description = "")
    {
        ID = id;
        LicencePlate = licencePlate;
        Manufacturer = manufacturer;
        VehicleTypes = VehicleTypes.Motorcycle;
        Odometer = odometer;
        Description = description;
        this.dayCost = dayCost;
        this.kmCost = kmCost;
        VehicleStatus = VehicleStatus.Available;

    }
    public override VehicleTypes GetVehicleType() => VehicleTypes;        

}
