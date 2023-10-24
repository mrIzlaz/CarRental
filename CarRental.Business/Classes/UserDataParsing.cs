﻿using System.Reflection.Metadata.Ecma335;
using CarRental.Common.Enums;
using CarRental.Common.Extensions;
using System.Text.RegularExpressions;

namespace CarRental.Business.Classes;

public static class UserDataParsing
{
    public static string ParseNewVehicle(string? licensePlate, int? odometer, VehicleManufacturer? vehicleManufacturer,
        VehicleType? vehicleType, double? costKm, int? costDay, UserInputError error)
    {
        var (lp, boolValue) = ParseLicensePlate(licensePlate);
        if (boolValue) error.LicenseError = true;
        if (ParseOdometer(odometer)) error.OdometerError = true;
        if (ParseManufacturer(vehicleManufacturer)) error.ManufacturerError = true;
        if (ParseVehicleType(vehicleManufacturer, vehicleType)) error.VehicleTypeError = true;
        if (ParseCostDay(costDay)) error.CostDayError = true;
        if (ParseCostKm(costKm)) error.CostKmError = true;
        return lp;
    }


    public static void ParseNewCustomer(string ssnString, string firstName, string lastName)
    {
        ParseSsn(ssnString);
        ParseNames(firstName, lastName);
    }


    #region New Customer

    private static void ParseNames(string firstName, string lastName)
    {
        var rx = new Regex(@"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$");
        if (!rx.IsMatch(firstName))
            throw new ArgumentException("Not a valid First Name");
        if (!rx.IsMatch(lastName))
            throw new ArgumentException("Not a valid Last Name");
    }

    private static void ParseSsn(string ssnString)
    {
        var parsedSsnString = RemoveHyphensFrom(ssnString);
        if (parsedSsnString.Length != 9)
            throw new ArgumentException(
                $"Social Security Number is {(parsedSsnString.Length < 9 ? " to short" : "to long")}");
        if (parsedSsnString.Equals("000000000")) throw new ArgumentException("Please enter a Social Security Number");
    }

    public static string RemoveHyphensFrom(string ssnString) => string.Concat(ssnString.Where(c => c != '-'));

    #endregion

    #region New Vehicle

    private static (string?, bool) ParseLicensePlate(string? licensePlate)
    {
        if (string.IsNullOrEmpty(licensePlate)) return (null, true);
        var rx = new Regex("^[A-Z]{3} ?[0-9]{2}[A-z0-9]$", RegexOptions.IgnoreCase);
        if (!rx.IsMatch(licensePlate))
            return (licensePlate, true);
        licensePlate = licensePlate.ToUpper();
        return (char.IsWhiteSpace(licensePlate[3]) ? licensePlate : licensePlate.Insert(3, " "), false);
    }

    private static bool ParseOdometer(int? odometer)
    {
        if (odometer >= 0) return false;
        return true;
        //throw new ArgumentException("Odometer Value incorrect");
    }

    private static bool ParseManufacturer(VehicleManufacturer? vehicleManufacturer)
    {
        if (vehicleManufacturer == null || ((int)vehicleManufacturer) == 0) return true;
        return false;

        //throw new ArgumentException("Please select a Manufacturer");
    }

    private static bool ParseCostKm(double? costKm)
    {
        if (costKm > 0d) return false;
        return true;
        //throw new ArgumentException("CostKM Value incorrect");
    }

    private static bool ParseCostDay(int? costDay)
    {
        switch (costDay)
        {
            case null:
                return true;
            case > 0:
                return false;
            default:
                return true;
        }
    }

    private static bool ParseVehicleType(VehicleManufacturer? vehicleManufacturer, VehicleType? vehicleType)
    {
        if (vehicleManufacturer == null || vehicleManufacturer == 0) return true;
        VehicleManufacturer vm = (VehicleManufacturer)vehicleManufacturer;
        if (vehicleType == null || (int)vehicleType == 0) return true; 
        if (vehicleType != VehicleType.Motorcycle || vm.IsMotoMaker()) return false;
        return true;
    }

    public static int ParseDistance(int? distance) => (int)(distance > 0 ? distance : 0);

    #endregion
}