﻿using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public abstract class Vehicle 
{
    public int Id { get; init; }
    public string LicencePlate { get; init; }
    public string Manufacturer { get; init; }
    public VehicleType VehicleType { get; init; }
    public VehicleStatus VehicleStatus { get; set; }

    private int _odometer = 0;
    public int Odometer
    {
        get => _odometer;
        set => _odometer += value;
    }
    public int DayCost { get; init; } = 0;
    public double KmCost { get; init; } = 0;

    protected Vehicle(int id, string licencePlate, string manufacturer, int odometer, VehicleType vehicleType,
        int dayCost, double kmCost)
    {
        Id = id;
        LicencePlate = licencePlate;
        Manufacturer = manufacturer;
        _odometer = odometer;
        VehicleType = vehicleType;
        VehicleStatus = VehicleStatus.Available;
        DayCost = dayCost;
        KmCost = kmCost;
    }

}