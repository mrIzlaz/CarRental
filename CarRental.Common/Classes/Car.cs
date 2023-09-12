using CarRental.Common.Enums;

namespace CarRental.Common.Classes;

public class Car : Vehicle
{
    public override VehicleTypes GetVehicleType() => VehicleTypes;
    
}
