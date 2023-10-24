using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Enums;

public static class Manufacturer
{
    private const int TotalCarManu = 10;
    public static int GetTotalCarManufacturers() => TotalCarManu;

    public static readonly List<VehicleManufacturer> MotoMakers = new()
        { VehicleManufacturer.Toyota, VehicleManufacturer.Bmw, VehicleManufacturer.Honda, VehicleManufacturer.Suzuki };

    private static List<string>? _MotoMakersString;
    public static IEnumerable<string> MotoMakersString => _MotoMakersString ?? ConvertList(MotoMakers);
    static Manufacturer() => _MotoMakersString = ConvertList(MotoMakers);
    private static List<string> ConvertList(List<VehicleManufacturer> list)
    {
        var newList = new List<string>();
        list.ForEach(x => newList.Add(x.ToString()));
        return newList;
    }
}

public enum VehicleManufacturer
{
    Toyota = 1,
    Bmw = 2,
    Honda = 3,
    Suzuki = 4,
    Volvo = 5,
    Kia = 6,
    Jeep = 7,
    Ford = 8,
    Škoda = 9,
    Rivian = 10
}