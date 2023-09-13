namespace CarRental.Data.Classes;

using CarRental.Common.Enums;
using CarRental.Common.Interfaces;
using CarRental.Data.Interfaces;
using System.Collections.Generic;


public class CollectionData : IData
{
    DataProducer producer = new DataProducer();
    readonly List<IPerson> _persons = new List<IPerson>();
    readonly List<IVehicle> _vehicles = new List<IVehicle>();
    readonly List<IBooking> _bookings = new List<IBooking>();

    public CollectionData() => SeedData();

    public IEnumerable<IBooking> GetBookings() => _bookings;
    public IEnumerable<IPerson> GetPersons() => _persons;
    public IEnumerable<IVehicle> GetVehicles(VehicleStatus status = 0)
    {
        return _vehicles;
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
            var perList = producer.GenerateIPersonList(4);
            _persons.AddRange(perList);

            var vehlist = producer.GenerateIVehicleList();
            _vehicles.AddRange(vehlist);
        }
        catch
        {
            throw new ArgumentException("Seed Data error!");
        }

    }



}