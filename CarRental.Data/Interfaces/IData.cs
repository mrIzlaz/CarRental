﻿namespace CarRental.Data.Interfaces;

using System.Linq.Expressions;
using Common.Enums;
using CarRental.Common.Interfaces;

public interface IData
{
    int NextVehicleId { get; }
    int NextPersonId { get; }
    int NextBookingId { get; }
    IBooking? RentVehicle(int vehicleId, int customerId);
    IEnumerable<string> SearchResult<T>(string searchPrompt) where T : ISearchable ;
    IBooking? ReturnVehicle(int vehicleId);
    T? Single<T>(Expression<Func<T, bool>>? expression);
    IEnumerable<T> Get<T>(Expression<Func<T, bool>>? expression);
    public void Add<T>(T item);
    public string[] VehicleStatusNames => Enum.GetNames(typeof(VehicleStatus));
    public IEnumerable<string> VehicleTypeNames => Enum.GetNames(typeof(VehicleType));
    public IEnumerable<string> VehicleManufacturer => Enum.GetNames(typeof(VehicleManufacturer));
    public VehicleType GetVehicleType(string name) => (VehicleType)Enum.Parse(typeof(VehicleType), name);
}