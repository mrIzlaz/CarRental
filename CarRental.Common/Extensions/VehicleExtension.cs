using CarRental.Common.Enums;

namespace CarRental.Common.Extensions;

public static class VehicleExtension
{
    public static bool IsMotoMaker(this VehicleManufacturer vehicleManufacturer) =>
        Manufacturer.MotoMakers.Contains(vehicleManufacturer);
}