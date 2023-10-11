using System.Linq.Expressions;
using CarRental.Common.Classes;

namespace CarRental.Data.Interfaces;

using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

public interface IData
{
    int NextVehicleId { get; }
    int NextPersonId { get; }
    int NextBookingId { get; }

    //IEnumerable<IPerson> GetPersons();
    //IEnumerable<Vehicle> GetVehicles(VehicleStatus status = default);
    //IEnumerable<IBooking> GetBookings();

    T? Single<T>(Expression<Func<T, bool>>? expression);
    List<T> Get<T>(Expression<Func<T, bool>>? expression);
    public void Add<T>(T item);


    public string[] VehicleStatusNames => Enum.GetNames(typeof(VehicleStatus));

    public IEnumerable<string> VehicleTypeNames => Enum.GetNames(typeof(VehicleType));
    public IEnumerable<string> VehicleManufacturer => Enum.GetNames(typeof(VehicleManufacturer));
    public VehicleType GetVehicleType(string name) => (VehicleType)Enum.Parse(typeof(VehicleType), name);
}