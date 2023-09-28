using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Booking : IBooking
{
    public bool IsActive { get; set; }
    public Vehicle Vehicle { get; init; }
    public Customer Customer { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime ReturnDate { get; set; }
    public int OdometerStart { get; init; }
    public int OdometerReturn { get; set; }
    public double? TotalCost { get; set; }
    public VehicleStatus BookingStatus { get; set; }

    public Booking(Vehicle vehicle, Customer customer, DateTime startDate) : this(vehicle, customer, startDate,
        startDate, VehicleStatus.Booked)
    {
    }

    public Booking(Vehicle vehicle, Customer customer, DateTime startDate, DateTime returnDate,
        VehicleStatus bookingStatus)
    {
        IsActive = true;
        Vehicle = vehicle;
        Vehicle.VehicleStatus = BookingStatus = bookingStatus;
        Customer = customer;
        OdometerStart = Vehicle.Odometer;
        StartDate = startDate;
        ReturnDate = returnDate;
        SetTotalCost();
    }

    public bool TryCloseBooking(DateTime returnDate, int odometerReturn)
    {
        ReturnDate = returnDate;
        if (!TrySetOdometerReturn(odometerReturn)) return false;
        OdometerReturn = odometerReturn;
        SetTotalCost();
        IsActive = false;
        Vehicle.VehicleStatus = BookingStatus = VehicleStatus.Available;
        return true;
    }

    private void SetTotalCost()
    {
        var dif = Vehicle.Odometer - OdometerStart;
        var daysRented = (ReturnDate - StartDate).TotalDays;
        TotalCost = Math.Round((dif * Vehicle.KmCost) + (daysRented * Vehicle.KmCost), 2);
    }

    public bool TrySetOdometerReturn(int value)
    {
        var dif = value - OdometerStart;
        if (dif < 0) return false;
        Vehicle.Odometer = dif;
        return true;
    }
}