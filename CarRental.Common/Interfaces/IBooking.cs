using CarRental.Common.Classes;
using CarRental.Common.Enums;

namespace CarRental.Common.Interfaces;

public interface IBooking
{
    public bool TryCloseBooking(DateTime returnDate, int odometerReturn);
    public bool IsActive { get; set; }
    public Vehicle Vehicle { get; init; }
    public Customer Customer { get; init; }
    public int OdometerStart { get; init; }
    public int OdometerReturn { get; set; }
    protected bool TrySetOdometerReturn(int value);
    public DateTime StartDate { get; init; }
    public DateTime ReturnDate { get; set; }
    public double? TotalCost { get; set; }
    public VehicleStatus BookingStatus { get; set; }
}