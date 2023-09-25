using CarRental.Common.Enums;

namespace CarRental.Common.Classes;

public class Car : Vehicle
{
    public Car(int id, string licencePlate, string manufacturer, int odometer, VehicleTypes vehicleTypes, int dayCost, double kmCost) : base(id, licencePlate, manufacturer, odometer, vehicleTypes, dayCost, kmCost)
    {
    }

    public override VehicleTypes GetVehicleType() => VehicleTypes;
}
