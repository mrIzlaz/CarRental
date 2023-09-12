namespace CarRental.Common.Classes;

public class Bookings
{
    public bool IsActive { get; set; }
    public required Vehicle Vehicle { private get; init; }

    public string LicencePlate() => Vehicle.LicencePlate;

}