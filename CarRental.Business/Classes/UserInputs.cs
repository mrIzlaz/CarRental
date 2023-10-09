using CarRental.Common.Classes;

namespace CarRental.Business.Classes;

using Microsoft.AspNetCore.Components;
using Common.Enums;
using System.Text.RegularExpressions;
using Common.Extensions;

public partial class UserInputs
{
    private readonly BookingProcessor _bp;
    public UserInputs(BookingProcessor bp) => _bp = bp;

    public bool IsProcessing { get; set; } = false;

    public bool ToggleProcessing()
    {
        IsProcessing = !IsProcessing;
        return IsProcessing;
    }


    #region Feedback

    public bool IsInputValid;
    public readonly List<string> InputFeedbackMessages = new();
    public string DataValues = string.Empty;

    #endregion

    #region New Vehicle

    public string LicensePlate { get; set; } = string.Empty;
    public VehicleManufacturer VehManufacturer { get; private set; }
    public int? Odometer { get; set; }
    public double? CostKm { get; set; }
    public VehicleType? VehType { get; private set; }
    public int? CostDay { get; set; }
    public VehicleStatus VisibleVehicle { get; set; }

    #endregion

    #region New Booking

    public int? RentClientId { get; private set; }
    public Vehicle? NewBookingVehicle { get; private set; }
    public DateTime RentDate { get; private set; }

    #endregion

    #region Returning

    public Vehicle? ReturnVehicle { get; private set; }
    public int? Distance { get; set; }

    #endregion

    #region New Customer

    public long SocialSecurityNumber { get; private set; }

    public string? SsnString
    {
        get => SocialSecurityNumber.ToString("000-00-0000");
        set
        {
            var parsed = ParseSsn(value);
            if (parsed.Length < 9)
            {
                for (int i = parsed.Length - 1; i < 9; i++)
                    parsed += "0";
            }

            if (long.TryParse(value, out long result))
                SocialSecurityNumber = result;
        }
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    #endregion

    public bool ValidVehicle { get; private set; }
    public bool ValidCustomer { get; private set; }
    public bool ValidBooking { get; private set; }
    public bool ValidReturn { get; private set; }

    public void TryAddNewCar()
    {
        ClearFeedbackMessage();
        try
        {
            ParseLicensePlate();
            ParseManufacturer();
            ParseVehicleType();
            ParseOdometer();
            ParseCostKm();
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


    private void TryRent()
    {
        try
        {
            if (_bp.GetCustomers().All(c => c.CustomerId != RentClientId)) return;
            RentDate = DateTime.Today;
            ValidBooking = true;
            _bp.Add(this);
        }
        catch (Exception e)
        {
            InputFeedbackMessages.Add(e.Message);
        }

        ClearRentData();
    }

    private void TryReturnVehicle()
    {
        try
        {
            ParseDistance();
            ParseVehicle();
            ValidReturn = true;
            _bp.Add(this);
        }
        catch (Exception e)
        {
            InputFeedbackMessages.Add(e.Message);
        }

        ClearReturnData();
    }

    private void TryAddNewCustomer()
    {
        try
        {
            ParseSsn();
            ParseNames();
            ValidCustomer = true;
            _bp.Add(this);
        }
        catch (Exception e)
        {
            InputFeedbackMessages.Add(e.Message);
        }

        ClearNewCustomerData();
    }

    private void ClearNewCustomerData()
    {
        SocialSecurityNumber = 0;
        SsnString = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        ValidCustomer = false;
    }

    private void ParseNames()
    {
        DataValues += $"Name: {LastName} {FirstName}";
        var rx = MyRegexValidNames();
        if (FirstName != null && !rx.IsMatch(FirstName))
            throw new ArgumentException("Not a valid First Name");
        if (LastName != null && !rx.IsMatch(LastName))
            throw new ArgumentException("Not a valid Last Name");
    }

    private void ParseSsn()
    {
        var parsedSsnString = ParseSsn(SsnString);
        if (parsedSsnString.Length != 9)
            throw new ArgumentException(
                $"Social Security Number is {(parsedSsnString.Length < 9 ? " to short" : "to long")}");
        if (parsedSsnString.Equals("000000000")) throw new ArgumentException("Please enter a Social Security Number");
    }

    private string ParseSsn(string? ssnString) => String.Concat(ssnString!.Where(c => c != '-'));


    private void ParseLicensePlate()
    {
        DataValues += $"License plate: {LicensePlate} ";
        var rx = MyRegexValidLicensePlate(); //^[A-Z]{3} ?[0-9]{2}[A-z0-9]$
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
        var debugMess = VehManufacturer.ToString();
        if (VehType != VehicleType.Motorcycle || VehManufacturer.IsMotoMaker()) return;
        VehManufacturer = default;

        throw new ArgumentException($"{debugMess} does not make motorcycles");
    }

    private void ParseCostDay()
    {
        DataValues += $"Cost Day: {CostDay} ";
        if (CostDay is not (null or <= 0)) return;
        CostDay = null;
        throw new ArgumentException("Cost Day Value incorrect");
    }

    private void ParseDistance()
    {
        DataValues += $"Distance: {Distance}";
        if (Distance is > 0 or not null) return;
        Distance = null;
        throw new ArgumentException("Distance for returning vehicle not valid");
    }

    private void ParseVehicle()
    {
        DataValues += $"Vehicle Returning: {ReturnVehicle?.LicencePlate}";
        if (ReturnVehicle is not null) return;
        ReturnVehicle = null;
        throw new ArgumentException("Return Vehicle missing");
    }

    public void ev_SelectedVehicleType(ChangeEventArgs e)
    {
        if (e.Value is null) return;
        Enum.TryParse(e.Value.ToString(), out VehicleType type);
        VehType = type;
    }

    public void ev_SelectedManufacturerChanged(ChangeEventArgs e)
    {
        if (e.Value is null) return;
        Enum.TryParse(e.Value.ToString(), out VehicleManufacturer manufacturer);
        VehManufacturer = manufacturer;
        UpdateCosts();
    }

    public void ev_SelectClient(ChangeEventArgs e)
    {
        if (e.Value is null) return;
        if (int.TryParse(e.Value.ToString(), out int client))
        {
            RentClientId = client;
        }
    }

    public void ev_RentVehicle(Vehicle vehicle)
    {
        NewBookingVehicle = vehicle;
        TryRent();
    }

    public void ev_ReturnVehicle(Vehicle vehicle)
    {
        ReturnVehicle = vehicle;
        TryReturnVehicle();
    }

    public void ev_AddNewCustomer()
    {
        TryAddNewCustomer();
    }

    private void UpdateCosts()
    {
        CostKm = VehManufacturer.GetVehicleCost(true);
        CostDay = (int)VehManufacturer.GetVehicleCost();
    }

    private void ClearVehicleData()
    {
        LicensePlate = string.Empty;
        VehManufacturer = default;
        Odometer = null;
        CostKm = null;
        VehType = null;
        CostDay = null;
        ValidVehicle = default;
    }

    private void ClearRentData()
    {
        RentClientId = null;
        NewBookingVehicle = null;
        ValidBooking = false;
    }

    private void ClearReturnData()
    {
        Distance = null;
        ReturnVehicle = null;
        ValidReturn = false;
    }

    private void ClearFeedbackMessage()
    {
        InputFeedbackMessages.Clear();
        DataValues = string.Empty;
        IsInputValid = false;
    }

    [GeneratedRegex("\\b(^[A-Z]{3} ?[0-9]{2}[A-z0-9]$)\\b", RegexOptions.IgnoreCase, "sv-SE")]
    private static partial Regex MyRegexValidLicensePlate();

    [GeneratedRegex(@"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$")]
    private static partial Regex MyRegexValidNames();
}