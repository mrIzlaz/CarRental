using CarRental.Common.Enums;
namespace CarRental.Common.Extensions;

public static class VehicleExtension
{
    private static List<VehicleTypes> _vehicleTypesList = new() {
        VehicleTypes.Sedan,
        VehicleTypes.Combi,
        VehicleTypes.Van,
        VehicleTypes.Motorcycle
    };
    private static List<VehicleManufacturer> _manufacturerList = new() {
        VehicleManufacturer.Toyota,
        VehicleManufacturer.Bmw,
        VehicleManufacturer.Honda,
        VehicleManufacturer.Suzuki,
        VehicleManufacturer.Volvo,
        VehicleManufacturer.Kia,
        VehicleManufacturer.Jeep,
        VehicleManufacturer.Ford,
        VehicleManufacturer.Škoda,
        VehicleManufacturer.Rivian
    };

    public static List<VehicleTypes> GetVehicleTypesList() => _vehicleTypesList;


    public static List<VehicleManufacturer> GetManufacturersList() => _manufacturerList;

}
