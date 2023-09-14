using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Booking : IBooking
{
    public bool IsActive { get; set; }
    private Vehicle Vehicle { get; init; }
    private Customer Customer { get; init; }

    public DateTime StartDate { get; init; }
    public DateTime ReturnDate { get; set; }

    private int OdometerStart { get; set; }

    private double? TotalCost { get; set; }

    public Booking(Vehicle vehicle, Customer customer, DateTime startDate)
    {
        Vehicle = vehicle;
        Customer = customer;
        IsActive = Vehicle.GetBookingStatus() == VehicleStatus.Booked;
        OdometerStart = Vehicle.GetOdometer();
        StartDate = startDate;
        ReturnDate = startDate;
    }

    public bool TryCloseBooking(DateTime returnDate, int odometerReturn)
    {
        ReturnDate = returnDate;
        if (TrySetOdometerReturn(odometerReturn))
        {
            SetTotalCost();
            IsActive = false;
            Vehicle.VehicleStatus = VehicleStatus.Available;
            return true;
        }
        else return false;
    }

    private void SetTotalCost()
    {
        var dif = OdometerStart - Vehicle.Odometer;
        var days = StartDate.DayOfYear - ReturnDate.DayOfYear;
        TotalCost = (dif * Vehicle.kmCost) + (days * Vehicle.dayCost);
    }

    public VehicleStatus BookingStatus() => Vehicle.GetBookingStatus();

    public string CustomerName() => Customer.GetFullInfo();

    public string LicensePlate() => Vehicle.GetLicencePlate();

    public int? GetOdometerReturn() => IsActive ? null : Vehicle.GetOdometer();

    protected bool TrySetOdometerReturn(int value)
    {

        var dif = OdometerStart - value;
        if (dif < 0) { return false; }
        else
            Vehicle.Odometer = dif;
        return true;

    }

    public int GetOdometerStart() => OdometerStart;

    public DateTime? GetReturnDate() => ReturnDate;

    public DateTime GetStartDate() => StartDate;

    public double? GetTotalCost() => IsActive ? null : TotalCost;

    bool IBooking.TrySetOdometerReturn(int value)
    {
        throw new NotImplementedException();
    }
}