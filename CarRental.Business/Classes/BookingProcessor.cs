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
        var list = _db.GetVehicles().ToList();
        return list.GetRange(0, list.Count);
    }

    public IEnumerable<IBooking> GetBookings()
    {
        throw new NotImplementedException();
    }
}