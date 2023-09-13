using CarRental.Common.Enums;
namespace CarRental.Common.Interfaces;

public interface IVehicle
{
    public string GetLicencePlate();
    public string GetManufacturer();
    public string? GetDescription();
    public VehicleTypes GetVehicleType();
    public int GetOdometer();
    public int GetDayCost();
    public double GetKmCost();
    public VehicleStatus GetBookingStatus();
}
