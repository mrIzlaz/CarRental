using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using CarRental.Data.Interfaces;

namespace CarRental.Business.Classes;

public class BookingProcessor
{
    private readonly IData _db;
    public BookingProcessor(IData db) => _db = db;

    public IEnumerable<Customer> GetCustomers()
    {
        var list = _db.GetPersons().Where(x => x is Customer).Cast<Customer>().ToList();
        return list.GetRange(0, list.Count);
    }
    public IEnumerable<IVehicle> GetVehicles(VehicleStatus status = default)
    {
        var list = status == default ? _db.GetVehicles().ToList() : _db.GetVehicles().Where(x => x.VehicleStatus == status).ToList();
        var vehicles = list.OrderByDescending(x => x.VehicleStatus == VehicleStatus.Available).ToList();
        return vehicles.GetRange(0, vehicles.Count);
    }

    public IEnumerable<IBooking> GetBookings()
    {
        var list = _db.GetBookings().ToList();
        return list.GetRange(0, list.Count);
    }
}