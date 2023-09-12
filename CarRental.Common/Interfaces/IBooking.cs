
using CarRental.Common.Enums;

namespace CarRental.Common.Interfaces;

public interface IBooking
{
    public string LicensePlate();
    public string CustomerName();
    public int OdometerStart();
    public int? OdometerReturn();
    public DateTime StartDate();
    public DateTime? ReturnDate();
    public int? TotalCost();
    public VehicleStatus BookingStatus();

}
