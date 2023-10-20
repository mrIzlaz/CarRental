using CarRental.Common.Enums;
using CarRental.Common.Extensions;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Booking : IBooking, ISearchable
{
    public bool IsActive { get; set; }
    public int BookingId { get; set; }
    public Vehicle Vehicle { get; init; }
    public Customer Customer { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime ReturnDate { get; set; }
    public int OdometerStart { get; init; }
    public int OdometerReturn { get; set; }
    public double? TotalCost { get; set; }
    public VehicleStatus BookingStatus { get; set; }

    public Booking(int id, Vehicle vehicle, Customer customer, DateTime startDate) : this(id, vehicle, customer,
        startDate, startDate, VehicleStatus.Booked)
    {
    }

    public Booking(int id, Vehicle vehicle, Customer customer, DateTime startDate, DateTime returnDate,
        VehicleStatus bookingStatus)
    {
        BookingId = id;
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
        var daysRented = StartDate.Duration(ReturnDate);
        if (daysRented == 0) daysRented = 1;
        TotalCost = Math.Round((dif * Vehicle.KmCost) + (daysRented * Vehicle.DayCost), 2);
    }

    public bool TrySetOdometerReturn(int value)
    {
        var dif = value - OdometerStart;
        if (dif < 0) return false;
        Vehicle.Odometer = dif;
        return true;
    }

    public bool IsMatchingThis(string prompt)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"{(IsActive ? "Active" : "Closed")} Booking: {Vehicle.ToString()} by Customer: {Customer.ToString()} {(IsActive ? "Start Date: "+StartDate.ToShortDateString() : "")}";
    }
}