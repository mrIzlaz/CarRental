﻿using CarRental.Common.Classes;

namespace CarRental.Data.Interfaces;
using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

public interface IData
{

    IEnumerable<IPerson> GetPersons();

    IEnumerable<Vehicle> GetVehicles(VehicleStatus status = default);

    IEnumerable<IBooking> GetBookings();

}
