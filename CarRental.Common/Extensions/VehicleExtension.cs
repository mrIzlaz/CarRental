using CarRental.Common.Enums;
namespace CarRental.Common.Extensions;

public static class VehicleExtension
{
    private static List<VehicleTypes> vehicleTypesList = new() {
        VehicleTypes.Sedan,
        VehicleTypes.Combi,
        VehicleTypes.Van,
        VehicleTypes.Motorcycle
    };
    private static List<VehicleManufacturer> manufacturerList = new() {
        VehicleManufacturer.Toyota,
        VehicleManufacturer.BMW,
        VehicleManufacturer.Honda,
        VehicleManufacturer.Suzuki,
        VehicleManufacturer.Volvo,
        VehicleManufacturer.KIA,
        VehicleManufacturer.Jeep,
        VehicleManufacturer.Ford,
        VehicleManufacturer.Škoda,
        VehicleManufacturer.Rivian
    };

    public static List<VehicleTypes> GetVehicleTypesList() => vehicleTypesList;


    public static List<VehicleManufacturer> GetManufacturersList() => manufacturerList;

}
