using System.Linq.Expressions;
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

    public IEnumerable<Vehicle> GetVehicles(VehicleStatus status = default)
    {
        var list = status == default
            ? _db.GetVehicles().ToList()
            : _db.GetVehicles().Where(x => x.VehicleStatus == status).ToList();
        var vehicles = list.OrderByDescending(x => x.VehicleStatus == VehicleStatus.Available).ToList();
        return vehicles.GetRange(0, vehicles.Count);
    }

    public IEnumerable<IBooking> GetBookings()
    {
        var list = _db.GetBookings().ToList();
        return list.GetRange(0, list.Count);

        _db.Get<IBooking>(null);
    }

    public void Add(UserInputs inputs)
    {
        if (inputs.Vehicle != null)
            _db.Add(inputs.Vehicle);
        else if (inputs.Customer != null)
            _db.Add(inputs.Customer);
        else if (inputs.HasValidReturn)
        {
            if (inputs.ReturnVehicle is null || inputs.Distance is null) return;
            var b = _db.GetBookings().FirstOrDefault(b =>
                b.BookingStatus == VehicleStatus.Booked &&
                b.Vehicle.LicencePlate.Equals(inputs.ReturnVehicle.LicencePlate))!.TryCloseBooking(DateTime.Today,
                inputs.ReturnVehicle.Odometer + (int)inputs.Distance);
        }
        else if (inputs.HasValidBooking)
        {
            var customer = _db.GetPersons().Cast<Customer>().FirstOrDefault(c => c.CustomerId == inputs.RentClientId);
            if (customer == null || inputs.NewBookingVehicle == null) return;
            _db.Add(new Booking(inputs.NewBookingVehicle, customer, inputs.RentDate));
        }
    }

    public void TestGet()
    {
        Expression<Func<Vehicle, bool>> expr = i => i.VehicleStatus == VehicleStatus.Available;
        _db.Get<Vehicle>(expr);
    }

    public string[] GetVehicleStatusNames => _db.VehicleStatusNames;
    public IEnumerable<string> VehicleTypeNames => _db.VehicleTypeNames;
    public IEnumerable<string> VehicleManufacturer => _db.VehicleManufacturer;
    public VehicleType GetVehicleType(string name) => _db.GetVehicleType(name);
}