using CarRental.Common.Enums;
using CarRental.Common.Interfaces;

namespace CarRental.Common.Classes;

public class Booking : IBooking
{
    const int DaysInAYear = 365;
    const int MaxNotesLength = 400;
    public bool IsActive { get; set; }
    private Vehicle Vehicle { get; init; }
    private Customer Customer { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime ReturnDate { get; set; }
    private int OdometerStart { get; set; }
    private double? TotalCost { get; set; }
    private string? Notes { get; set; }

    public Booking(IVehicle vehicle, Customer customer, DateTime startDate)
    {
        IsActive = true;
        Vehicle = (Vehicle)vehicle;
        Vehicle.UpdateBookingStatus(true);
        Customer = customer;
        OdometerStart = Vehicle.GetOdometer();
        StartDate = startDate;
        ReturnDate = startDate;
    }
    public Booking(IVehicle vehicle, Customer customer, DateTime startDate, DateTime returnDate, VehicleStatus bookingStatus)
    {
        IsActive = true;
        Vehicle = (Vehicle)vehicle;
        Vehicle.UpdateBookingStatus(bookingStatus);
        Customer = customer;
        OdometerStart = Vehicle.GetOdometer();
        StartDate = startDate;
        ReturnDate = returnDate;
        SetTotalCost();
    }
    public Booking(IVehicle vehicle, Customer customer, DateTime startDate, DateTime returnDate, VehicleStatus bookingStatus, string notes)
    {
        IsActive = true;
        Vehicle = (Vehicle)vehicle;
        Vehicle.UpdateBookingStatus(bookingStatus);
        Customer = customer;
        OdometerStart = Vehicle.GetOdometer();
        StartDate = startDate;
        ReturnDate = returnDate;
        Notes = FormatString(notes);
        SetTotalCost();
    }

    private string FormatString(string text)
    {
        if (text == null)
            return "";
        var trunc = text.Substring(0, MaxNotesLength);
        return trunc;
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
        var dif = Vehicle.Odometer - OdometerStart;
        var years = ReturnDate.Year - StartDate.Year;
        var days = (ReturnDate.DayOfYear + (years * DaysInAYear) - StartDate.DayOfYear);
        TotalCost = Math.Round((dif * Vehicle.kmCost) + (days * Vehicle.dayCost), 2);
    }

    public bool TrySetOdometerReturn(int value)
    {

        var dif = value - OdometerStart;
        if (dif < 0) { return false; }
        else
            Vehicle.Odometer = dif;
        return true;

    }
    public VehicleStatus GetBookingStatus() => Vehicle.GetVehicleStatus();

    public string CustomerName() => Customer.GetFullInfo();
    public int CustomerID() => Customer.CustomerId;

    public string LicensePlate() => Vehicle.GetLicencePlate();

    public int? GetOdometerReturn() => IsActive ? null : Vehicle.GetOdometer();


    public int GetOdometerStart() => OdometerStart;

    public DateTime? GetReturnDate() => ReturnDate;

    public DateTime GetStartDate() => StartDate;

    public double? GetTotalCost() => IsActive ? null : TotalCost;

    public bool IsBookingActive() => IsActive;

    public string? GetNotes() => Notes;
}