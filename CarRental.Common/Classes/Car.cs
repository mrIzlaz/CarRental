using CarRental.Common.Enums;

namespace CarRental.Common.Classes;

public class Car : Vehicle
{
    public Car(int id, Vehicle vehicle) : base(id, vehicle.LicencePlate, vehicle.Manufacturer, vehicle.Odometer,
        vehicle.VehicleType, vehicle.DayCost, vehicle.KmCost) { }

    public Car(int id, string licencePlate, string manufacturer, int odometer, VehicleType vehicleType, int dayCost,
        double kmCost) : base(id, licencePlate, manufacturer, odometer, vehicleType, dayCost, kmCost) { }
    
}