using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.Enums;
public static class Manufacturer
{
    private const int TotalCarManu = 10;
    public static int GetTotalCarManufacturers() => TotalCarManu;

}
public enum VehicleManufacturer
{
    Toyota,
    BMW,
    Honda,
    Suzuki,
    Volvo,
    KIA,
    Jeep,
    Ford,
    Škoda,
    Rivian
}

