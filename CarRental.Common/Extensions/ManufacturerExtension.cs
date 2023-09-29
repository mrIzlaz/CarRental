using CarRental.Common.Enums;

namespace CarRental.Common.Extensions;

public static class ManufacturerExtension
{
    public static double GetVehicleCost(this VehicleManufacturer manufacturer, bool mileageCost = false)
    {
        var cost = manufacturer switch
        {
            VehicleManufacturer.Volvo => mileageCost ? 1.25d : 210,
            VehicleManufacturer.Kia => mileageCost ? 0.88d : 200,
            VehicleManufacturer.Jeep => mileageCost ? 2.12d : 230,
            VehicleManufacturer.Ford => mileageCost ? 1.2d : 200,
            VehicleManufacturer.Toyota => mileageCost ? 1.5d : 110,
            VehicleManufacturer.Škoda => mileageCost ? 1.22d : 140,
            VehicleManufacturer.Rivian => mileageCost ? 0.85d : 300,
            VehicleManufacturer.Bmw => mileageCost ? 2.1d : 230,
            VehicleManufacturer.Honda => mileageCost ? 1.52d : 100,
            VehicleManufacturer.Suzuki => mileageCost ? 1.12d : 190,
            _ => 0
        };
        return cost;
    }
}