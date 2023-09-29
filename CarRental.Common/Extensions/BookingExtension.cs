﻿namespace CarRental.Common.Extensions;

public static class BookingExtension
{
    public static int Duration(this DateTime startDate, DateTime endDate) => (int)(startDate - endDate).TotalDays;
}