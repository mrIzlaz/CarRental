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
    }

    public void Add(UserInputs inputs)
    {
        if (inputs.ValidVehicle)
        {
            var id = _db.GetVehicles().ToList().Count + 1;
            var plate = inputs.LicensePlate;
            var manu = inputs.VehManufacturer;
            var odo = (int)inputs.Odometer;
            var costD = (int)inputs.CostDay;
            var costK = (double)inputs.CostKm;
            var type = (VehicleType)inputs.VehType;

            Vehicle vehicle = inputs.VehType == VehicleType.Motorcycle
                ? new Motorcycle(id, plate, manu.ToString(), odo, costD, costK)
                : new Car(id, plate, manu.ToString(), odo, type, costD, costK);
            _db.Add(vehicle);
        }
        else if (inputs.ValidCustomer)
        {
        }
        else if (inputs.ValidBooking)
        {
        }
    }

    public string[] GetVehicleStatusNames => _db.VehicleStatusNames;
    public IEnumerable<string> VehicleTypeNames => _db.VehicleTypeNames;
    public IEnumerable<string> VehicleManufacturer => _db.VehicleManufacturer;
    public VehicleType GetVehicleType(string name) => _db.GetVehicleType(name);
}