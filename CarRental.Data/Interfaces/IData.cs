using CarRental.Common.Classes;

namespace CarRental.Data.Interfaces;

using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

public interface IData
{
    IEnumerable<IPerson> GetPersons();

    IEnumerable<Vehicle> GetVehicles(VehicleStatus status = default);

    IEnumerable<IBooking> GetBookings();

    public void Add(Vehicle vehicle);
    public void Add(Customer customer);
    public void Add(Booking booking);
    public string[] VehicleStatusNames => Enum.GetNames(typeof(VehicleStatus));
    public IEnumerable<string> VehicleTypeNames => Enum.GetNames(typeof(VehicleType));
    public IEnumerable<string> VehicleManufacturer => Enum.GetNames(typeof(VehicleManufacturer));
    public VehicleType GetVehicleType(string name) => (VehicleType)Enum.Parse(typeof(VehicleType), name);
}