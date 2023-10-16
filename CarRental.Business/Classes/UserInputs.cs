namespace CarRental.Business.Classes;

using Microsoft.AspNetCore.Components;
using Common.Enums;
using System.Text.RegularExpressions;
using Common.Extensions;
using CarRental.Common.Classes;

public partial class UserInputs
{
    private readonly BookingProcessor _bp;
    public UserInputs(BookingProcessor bp) => _bp = bp;
    public ValidUserInputData ValidUserInputData { get; private set; }
    public bool IsProcessing { get; set; }
    public void ToggleProcessing() => IsProcessing = !IsProcessing;

    #region Feedback

    public bool IsInputValid;
    public readonly List<string> InputFeedbackMessages = new();
    public string DataValues = string.Empty;

    #endregion

    #region New Vehicle

    public string LicensePlate { get; set; } = string.Empty;
    private VehicleManufacturer VehManufacturer { get; set; }
    public int? Odometer { get; set; }
    public double? CostKm { get; set; }
    public VehicleType? VehType { get; private set; }
    public int? CostDay { get; set; }
    public VehicleStatus VisibleVehicle { get; set; }

    #endregion

    #region New Customer

    private long SocialSecurityNumber { get; set; }

    public string? SsnString
    {
        get => SocialSecurityNumber.ToString("000-00-0000");
        set
        {
            var parsed = ParseSsn(value);
            if (parsed.Length < 9)
            {
                for (var i = parsed.Length - 1; i < 9; i++)
                    parsed += "0";
            }

            parsed = parsed[..9];
            if (long.TryParse(parsed, out var result))
                SocialSecurityNumber = result;
        }
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    #endregion

    #region New Booking

    public readonly Dictionary<Vehicle, int> RentList = new Dictionary<Vehicle, int>();
    public int? RentCustomer { get; private set; }
    public Vehicle? RentVehicle { get; private set; }

    #endregion

    #region Returning

    public Vehicle? ReturnVehicle { get; private set; }
    public int? Distance { get; set; }

    #endregion

    public async Task TryAddNewVehicle()
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

            ValidUserInputData = ValidUserInputData.Vehicle;
            await _bp.HandleUserInput(this);
            ClearVehicleData();
        }
        catch (ArgumentException e)
        {
            InputFeedbackMessages.Add(e.Message);
        }
        catch (Exception e)
        {
            InputFeedbackMessages.Add(e.Message);
        }
    }

    private async Task TryRent()
    {
        ClearFeedbackMessage();
        try
        {
            if (RentVehicle == null) throw new Exception("Error Renting Vehicle");
            if (!RentList.TryGetValue(RentVehicle, out var customerId)) return;
            if (_bp.GetCustomers().All(c => c.CustomerId != customerId)) return;
            RentCustomer = customerId;
            ValidUserInputData = ValidUserInputData.Booking;
            var t = _bp.HandleUserInput(this);
            await t;
            RentList.Remove(RentVehicle); //Calling this
        }
        catch (Exception e)
        {
            InputFeedbackMessages.Add(e.Message);
        }

        ClearRentData();
    }

    private async Task TryReturnVehicle()
    {
        ClearFeedbackMessage();
        try
        {
            ParseDistance();
            ParseVehicle();
            ValidUserInputData = ValidUserInputData.Returning;
            await _bp.HandleUserInput(this);
        }
        catch (Exception e)
        {
            InputFeedbackMessages.Add(e.Message);
        }

        ClearReturnData();
    }

    private async Task TryAddNewCustomer()
    {
        ClearFeedbackMessage();
        try
        {
            ParseSsn();
            ParseNames();
            ValidUserInputData = ValidUserInputData.Customer;
            await _bp.HandleUserInput(this);
            ClearNewCustomerData();
        }
        catch (Exception e)
        {
            InputFeedbackMessages.Add(e.Message);
        }
    }

    public Vehicle? GetVehicle(int idNo)
    {
        if (ValidUserInputData != ValidUserInputData.Vehicle) return null;
        if (Odometer == null || CostDay == null || CostKm == null || VehType == default) return null;
        Vehicle vehicle = VehType == VehicleType.Motorcycle
            ? new Motorcycle(idNo, LicensePlate, VehManufacturer.ToString(), (int)Odometer, (int)CostDay,
                (double)CostKm)
            : new Car(idNo, LicensePlate, VehManufacturer.ToString(), (int)Odometer, (VehicleType)VehType, (int)CostDay,
                (double)CostKm);
        return vehicle;
    }

    public Customer? GetCustomer(int idNo) => ValidUserInputData == ValidUserInputData.Customer
        ? new Customer(idNo, FirstName ?? "", LastName ?? "", SocialSecurityNumber,
            DateOnly.FromDateTime(DateTime.Today))
        : null;


    #region DataValidation

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

    private static string ParseSsn(string? ssnString) => string.Concat(ssnString!.Where(c => c != '-'));


    private void ParseLicensePlate()
    {
        DataValues += $"License plate: {LicensePlate} ";
        var rx = MyRegexValidLicensePlate(); //^[A-Z]{3} ?[0-9]{2}[A-z0-9]$
        if (!rx.IsMatch(LicensePlate))
            throw new ArgumentException("Not a valid Swedish License Plate");
        LicensePlate = LicensePlate.ToUpper();
        if (char.IsWhiteSpace(LicensePlate[3])) return;
        LicensePlate = LicensePlate.Insert(3, " ");
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
        if (Distance is not null or > 0) return;
        Distance = 0;
    }

    private void ParseVehicle()
    {
        DataValues += $"Vehicle Returning: {ReturnVehicle?.LicencePlate}";
        if (ReturnVehicle is not null) return;
        ReturnVehicle = null;
        throw new ArgumentException("Return Vehicle missing");
    }

    #endregion

    #region UserEvents

    public async Task ev_AddNewCar()
    {
        await TryAddNewVehicle();
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
        OnManufacturerUpdate();
    }

    public void ev_SelectClient(ChangeEventArgs e, Vehicle vehicle)
    {
        if (e.Value is null) return;
        if (!int.TryParse(e.Value.ToString(), out var customerId)) return;
        if (!RentList.TryAdd(vehicle, customerId))
            RentList[vehicle] = customerId;
    }

    public async Task ev_RentVehicle(Vehicle vehicle)
    {
        RentVehicle = vehicle;
        await TryRent();
    }

    public async Task ev_ReturnVehicle(Vehicle vehicle)
    {
        ReturnVehicle = vehicle;
        await TryReturnVehicle();
    }

    public async Task ev_AddNewCustomer() => await TryAddNewCustomer();


    private void OnManufacturerUpdate()
    {
        CostKm = VehManufacturer.GetVehicleCost(true);
        CostDay = (int)VehManufacturer.GetVehicleCost();
    }

    #endregion

    #region ClearData

    private void ClearVehicleData()
    {
        LicensePlate = string.Empty;
        Odometer = null;
        CostKm = null;
        CostDay = null;
        ValidUserInputData = default;
    }

    private void ClearRentData()
    {
        RentCustomer = null;
        RentVehicle = null;
        ValidUserInputData = default;
    }

    private void ClearReturnData()
    {
        Distance = null;
        ReturnVehicle = null;
        ValidUserInputData = default;
    }

    private void ClearFeedbackMessage()
    {
        InputFeedbackMessages.Clear();
        DataValues = string.Empty;
        IsInputValid = false;
    }


    private void ClearNewCustomerData()
    {
        SocialSecurityNumber = 0;
        SsnString = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        ValidUserInputData = default;
    }

    #endregion

    [GeneratedRegex("\\b(^[A-Z]{3} ?[0-9]{2}[A-z0-9]$)\\b", RegexOptions.IgnoreCase, "sv-SE")]
    private static partial Regex MyRegexValidLicensePlate();

    [GeneratedRegex(@"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$")]
    private static partial Regex MyRegexValidNames();
}

public enum ValidUserInputData
{
    None,
    Customer,
    Vehicle,
    Booking,
    Returning
}