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

    public Customer GetCustomer(int id) =>
        _db.Get<IPerson>(p => p is Customer).Cast<Customer>().Single(c => c.CustomerId == id);

    public IEnumerable<Vehicle> GetVehicles(VehicleStatus status = default) =>
        _db.Get<Vehicle>(status == default ? null : v => v.VehicleStatus == status)
            .OrderByDescending(x => x.VehicleStatus).ToList();


    public IEnumerable<IBooking> GetBookings() =>
        _db.Get<IBooking>(null).OrderBy(x => x.BookingStatus);


    public async Task HandleUserInput(UserInputs inputs)
    {
        inputs.IsProcessing = true;
        switch (inputs.ValidUserInputData)
        {
            case ValidUserInputData.Vehicle:
            {
                await Task.Delay(1000);
                var veh = inputs.GetVehicle(_db.NextVehicleId);
                if (veh != null)
                    _db.Add(veh);
                break;
            }
            case ValidUserInputData.Customer:
            {
                await Task.Delay(1000);
                var cus = inputs.GetCustomer(_db.NextPersonId);
                if (cus == null) break;
                _db.Add((IPerson)cus);
                break;
            }
            case ValidUserInputData.Returning when inputs.ReturnVehicle is null || inputs.Distance is null:
                return;
            case ValidUserInputData.Returning:
            {
                await Task.Delay(1000);
                var booking = _db.ReturnVehicle(inputs.ReturnVehicle.Id);
                booking?.TryCloseBooking(DateTime.Today, inputs.ReturnVehicle.Odometer + (int)inputs.Distance);
                break;
            }
            case ValidUserInputData.Booking:
            {
                if (inputs.RentVehicle == null || inputs.RentCustomer == null) break;
                await Task.Delay(5000);
                var booking = _db.RentVehicle(inputs.RentVehicle.Id, (int)inputs.RentCustomer);
                if (booking != null) _db.Add(booking);

                break;
            }
            case ValidUserInputData.None:
                break;
        }

        inputs.IsProcessing = false;
    }

    public IEnumerable<string> VehicleTypeNames => _db.VehicleTypeNames;
    public IEnumerable<string> VehicleManufacturer => _db.VehicleManufacturer;
}