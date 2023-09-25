namespace CarRental.Data.Classes;

using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using CarRental.Data.Interfaces;
using System.Collections.Generic;
using CarRental.Common.Classes;

public class CollectionData : IData
{
    DataFactory _producer = new DataFactory();
    readonly List<IPerson> _persons = new List<IPerson>();
    readonly List<IVehicle> _vehicles = new List<IVehicle>();
    readonly List<IBooking> _bookings = new List<IBooking>();

    public CollectionData() => SeedData();

    public IEnumerable<IBooking> GetBookings() => _bookings;
    public IEnumerable<IPerson> GetPersons() => _persons;
    public IEnumerable<IVehicle> GetVehicles(VehicleStatus status = default)
    {
        return status == default ? _vehicles : _vehicles.Where(x => x.GetVehicleStatus() == status);
    }

    #region VG
    /*
        public IEnumerator<List<T>> Get();
        public IEnumerator<Generic<T>> Single();
        public void Add(<T>);
    */
    #endregion

    void SeedData()
    {
        try
        {
            var rnd = new Random();
            var perList = _producer.GenerateIPersonList(6);
            _persons.AddRange(perList);

            var vehlist = _producer.GenerateIVehicleList(12);
            _vehicles.AddRange(vehlist);

            var customers = _persons.Where(x => x is Customer).Cast<Customer>().ToList();
            var cus = customers.GetRange(0, customers.Count);
            var veh = _vehicles.GetRange(0, _vehicles.Count);
            _bookings.AddRange(_producer.GenerateIBookingsList(cus, veh, VehicleStatus.Booked, rnd.Next(1, 3)));
            _bookings.AddRange(_producer.GenerateIBookingsList(cus, veh, VehicleStatus.Available, rnd.Next(1, 2)));
            _bookings.AddRange(_producer.GenerateIBookingsList(cus, veh, VehicleStatus.Unavailable, rnd.Next(2)));

        }
        catch (Exception ex)
        {
            string mes = ex.Message;
        }




    }

}