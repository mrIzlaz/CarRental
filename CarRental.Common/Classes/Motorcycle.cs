using CarRental.Common.Enums;
namespace CarRental.Common.Classes;

public class Motorcycle : Vehicle
{
    public override VehicleTypes GetVehicleType() => VehicleTypes.Motorcycle;        

}
