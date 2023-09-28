namespace CarRental.Business.Classes;
using Microsoft.AspNetCore.Components;
using Common.Enums;
using System.Text.RegularExpressions;
using Common.Extensions;

public partial class UserInputs
{
    public bool IsInputValid = true;
    public readonly List<string> InputFeedbackMessages = new();
    public readonly List<string> DataValues = new();
    public string LicensePlate { get; set; } = string.Empty;
    private VehicleManufacturer Manufacturer { get; set; }
    public int? Odometer { get; set; }
    public double? CostKm { get; set; }
    private VehicleType VehicleType { get; set; }
    public int? CostDay { get; set; }
    public VehicleStatus VisibleVehicle { get; set; }
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
        IsInputValid = true;
    }

    private void ParseLicensePlate()
    {
        DataValues.Add($"License plate: {LicensePlate}");
        var rx = MyRegex(); //^[A-Z]{3} ?[0-9]{2}[A-z0-9]$
        if (!rx.IsMatch(LicensePlate))
            throw new ArgumentException("Not a valid Swedish License Plate");
    }

    private void ParseOdometer()
    {
        if (Odometer is not (null or <= 0)) return;
        Odometer = null;
        throw new ArgumentException("Odometer Value incorrect");
    }

    private void ParseManufacturer()
    {
        DataValues.Add($"Manufacturer: {Manufacturer.ToString()}");
        if (Manufacturer == default)
            throw new ArgumentException("Please select a Manufacturer");
    }

    private void ParseCostKm()
    {
        DataValues.Add($"CostKM: {CostKm}");
        if (CostKm is not (null or <= 0)) return;
        CostKm = null;
        throw new ArgumentException("CostKM Value incorrect");
    }

    private void ParseVehicleType()
    {
        DataValues.Add($"VehicleType: {VehicleType.ToString()}");
        if (VehicleType != VehicleType.Motorcycle || Manufacturer.IsMotoMaker()) return;
        Manufacturer = default;
        throw new ArgumentException($"{Manufacturer.ToString()} does not make motorcycles");
    }

    private void ParseCostDay()
    {
        DataValues.Add($"Cost Day: {CostDay}");
        if (CostDay is not (null or <= 0)) return;
        CostDay = null;
        throw new ArgumentException("Cost Day Value incorrect");
    }

    public void SelectedVehicleType(ChangeEventArgs e)
    {
        if (e.Value is null) return;
        Enum.TryParse(e.Value.ToString(), out VehicleType type);
        VehicleType = type;
    }

    public void SelectedManufacturerChanged(ChangeEventArgs e)
    {
        if (e.Value is null) return;
        Enum.TryParse(e.Value.ToString(), out VehicleManufacturer manufacturer);
        Manufacturer = manufacturer;
    }

    private void ClearFeedbackMessage()
    {
        InputFeedbackMessages.Clear();
        DataValues.Clear();
        IsInputValid = false;
    }

    [GeneratedRegex("\\b(^[A-Z]{3} ?[0-9]{2}[A-z0-9]$)\\b", RegexOptions.IgnoreCase, "sv-SE")]
    private static partial Regex MyRegex();
}