using CarRental.Common.Classes;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using CarRental.Data.Interfaces;

namespace CarRental.Business.Classes;

public class BookingProcessor
{
    private readonly IData _db;
    public BookingProcessor(IData db) => _db = db;

    public IEnumerable<Customer> GetCustomers() => _db.Get<IPerson>(p => p is Customer).Cast<Customer>();

    public IEnumerable<Vehicle> GetVehicles(VehicleStatus status = default) =>
        _db.Get<Vehicle>(status == default ? null : v => v.VehicleStatus == status);


    public IEnumerable<IBooking> GetBookings() => _db.Get<IBooking>(null);


    public void Add(UserInputs inputs)
    {
        if (inputs.Vehicle != null)
            _db.Add(inputs.Vehicle);
        else if (inputs.Customer != null)
            _db.Add(inputs.Customer);
        else if (inputs.HasValidReturn)
        {
            if (inputs.ReturnVehicle is null || inputs.Distance is null) return;
            _db.Single<IBooking>(b => b.BookingStatus == VehicleStatus.Booked &&
                                      b.Vehicle.LicencePlate.Equals(inputs.ReturnVehicle
                                          .LicencePlate))
                ?.TryCloseBooking(DateTime.Today, inputs.ReturnVehicle.Odometer + (int)inputs.Distance);
        }
        else if (inputs.HasValidBooking)
        {
            var customer = GetCustomers().SingleOrDefault(c => c.CustomerId == inputs.RentClientId);
            if (customer == null || inputs.NewBookingVehicle == null) return;
            _db.Add(new Booking(inputs.NewBookingVehicle, customer, inputs.RentDate));
        }
    }

    public string[] GetVehicleStatusNames => _db.VehicleStatusNames;
    public IEnumerable<string> VehicleTypeNames => _db.VehicleTypeNames;
    public IEnumerable<string> VehicleManufacturer => _db.VehicleManufacturer;
    public VehicleType GetVehicleType(string name) => _db.GetVehicleType(name);
}