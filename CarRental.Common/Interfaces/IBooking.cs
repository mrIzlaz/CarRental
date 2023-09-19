﻿
using CarRental.Common.Classes;
using CarRental.Common.Enums;

namespace CarRental.Common.Interfaces;

public interface IBooking
{
    public bool TryCloseBooking(DateTime returnDate, int odometerReturn);
    public string LicensePlate();
    public string CustomerName();
    public int CustomerID();
    public int GetOdometerStart();
    protected bool TrySetOdometerReturn(int value);
    public int? GetOdometerReturn();
    public DateTime GetStartDate();
    public DateTime? GetReturnDate();
    public double? GetTotalCost();
    public VehicleStatus GetBookingStatus();
    public bool IsBookingActive();
    public string? GetNotes();
}
