using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using CarRental.Data.Interfaces;

namespace CarRental.Business.Classes;

public class BookingProcessor
{
    private readonly IData _db;

    public BookingProcessor(IData db) => _db = db;

    public void DebugSeed()
    {
        _db.DebugCall();
    }
    public IEnumerable<Customer> GetCustomers()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IVehicle> GetVehicles(VehicleStatus status = default)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IBooking> GetBookings()
    {
        throw new NotImplementedException();
    }
}