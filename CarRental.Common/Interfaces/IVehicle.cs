using CarRental.Common.Enums;
namespace CarRental.Common.Interfaces;

public interface IVehicle
{
    public int Id { get; init; }
    public string LicencePlate { get; init; }
    public string Manufacturer { get; init; }
    public VehicleTypes VehicleType { get; init; }
    public VehicleStatus VehicleStatus { get; set; }
    public int Odometer { get; set; }
    public int DayCost { get; init; }
    public double KmCost { get; init; }

}
