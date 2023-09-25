namespace CarRental.Data.Classes;

using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using CarRental.Data.Interfaces;
using System.Collections.Generic;
using CarRental.Common.Classes;

public class CollectionData : IData
{
    private readonly DataFactory _producer = new();
    private readonly List<IPerson> _persons = new();
    private readonly List<IVehicle> _vehicles = new();
    private readonly List<IBooking> _bookings = new();

    public CollectionData() => SeedData();

    public IEnumerable<IBooking> GetBookings() => _bookings.OrderByDescending(x => x.GetBookingStatus()).Reverse();
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

    private void SeedData()
    {
        try
        {
            _persons.AddRange(_producer.GetPersons());
            _vehicles.AddRange(_producer.GetVehicles());
            _bookings.AddRange(_producer.GetBookings());
        }
        catch (Exception ex)
        {
            var mes = ex.Message;
        }
    }
}