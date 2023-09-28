namespace CarRental.Business.Classes;

using Microsoft.AspNetCore.Components;
using Common.Enums;
using System.Text.RegularExpressions;
using CarRental.Common.Extensions;

public partial class UserInputs
{
    private readonly BookingProcessor _bp;

    public bool IsInputValid = true;
    public readonly List<string> InputFeedbackMessages = new();
    public string DataValues = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public VehicleManufacturer VehManufacturer { get; set; }
    public int? Odometer { get; set; }
    public double? CostKm { get; set; }
    public VehicleType? VehType { get; set; }
    public int? CostDay { get; set; }
    public VehicleStatus VisibleVehicle { get; set; }

    public int? Distance { get; set; }
    public UserInputs(BookingProcessor bp) => _bp = bp;

    public bool ValidVehicle { get; private set; }
    public bool ValidCustomer { get; private set; }
    public bool ValidBooking { get; private set; }

    public void TryAddNewCar()
    {
        ClearFeedbackMessage();
        try
        {
            ParseLicensePlate();
            ParseManufacturer();
            ParseOdometer();
            ParseCostKm();
            ParseVehicleType();
            ParseCostDay();
        }
        catch (ArgumentException e)
        {
            InputFeedbackMessages.Add(e.Message);
        }
        catch (Exception e)
        {
            InputFeedbackMessages.Add(e.Message);
        }

        if (InputFeedbackMessages.Count != 0) return;

        InputFeedbackMessages.Add("All Data is valid");
        ValidVehicle = true;
        _bp.Add(this);
        ClearVehicleData();
    }

    public void TryRent()
    {
    }

    private void ParseLicensePlate()
    {
        DataValues += $"License plate: {LicensePlate} ";
        var rx = MyRegex(); //^[A-Z]{3} ?[0-9]{2}[A-z0-9]$
        if (!rx.IsMatch(LicensePlate))
            throw new ArgumentException("Not a valid Swedish License Plate");
    }

    private void ParseOdometer()
    {
        DataValues += $"Odometer: {Odometer} ";
        if (Odometer is not (null or <= 0)) return;
        Odometer = null;
        throw new ArgumentException("Odometer Value incorrect");
    }

    private void ParseManufacturer()
    {
        DataValues += $"Manufacturer: {VehManufacturer.ToString()} ";
        if (VehManufacturer == default)
            throw new ArgumentException("Please select a Manufacturer");
    }

    private void ParseCostKm()
    {
        DataValues += $"CostKM: {CostKm} ";
        if (CostKm is not (null or <= 0)) return;
        CostKm = null;
        throw new ArgumentException("CostKM Value incorrect");
    }

    private void ParseVehicleType()
    {
        DataValues += $"VehicleType: {VehType.ToString()} ";
        if (VehType != VehicleType.Motorcycle || VehManufacturer.IsMotoMaker()) return;
        VehManufacturer = default;

        throw new ArgumentException($"{VehManufacturer.ToString()} does not make motorcycles");
    }

    private void ParseCostDay()
    {
        DataValues += $"Cost Day: {CostDay} ";
        if (CostDay is not (null or <= 0)) return;
        CostDay = null;
        throw new ArgumentException("Cost Day Value incorrect");
    }

    public void SelectedVehicleType(ChangeEventArgs e)
    {
        if (e.Value is null) return;
        Enum.TryParse(e.Value.ToString(), out VehicleType type);
        this.VehType = type;
    }

    public void SelectedManufacturerChanged(ChangeEventArgs e)
    {
        if (e.Value is null) return;
        Enum.TryParse(e.Value.ToString(), out VehicleManufacturer manufacturer);
        VehManufacturer = manufacturer;
    }

    private void ClearVehicleData()
    {
    LicensePlate = string.Empty;
     VehManufacturer  = default;
    Odometer  = null;
   CostKm = null;
    VehType = null;
    CostDay = null;
    VisibleVehicle = default;
    }

    private void ClearFeedbackMessage()
    {
        InputFeedbackMessages.Clear();
        DataValues = string.Empty;
        IsInputValid = false;
    }

    [GeneratedRegex("\\b(^[A-Z]{3} ?[0-9]{2}[A-z0-9]$)\\b", RegexOptions.IgnoreCase, "sv-SE")]
    private static partial Regex MyRegex();
}