using CarRental.Common.Enums;

namespace CarRental.Common.Classes;

public class Car : Vehicle
{
    public Car(int id, string licencePlate, string manufacturer, int odometer, VehicleType vehicleType, int dayCost, double kmCost) : base(id, licencePlate, manufacturer, odometer, vehicleType, dayCost, kmCost)
    {
    }

}
