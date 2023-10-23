namespace CarRental.Business.Classes;

using Microsoft.AspNetCore.Components;
using Common.Enums;
using Common.Extensions;
using CarRental.Common.Classes;

public class UserInputs
{
    private readonly BookingProcessor _bp;
    public UserInputs(BookingProcessor bp) => _bp = bp;
    public ValidUserInputData ValidUserInputData { get; private set; }
    public bool IsProcessing { get; set; }
    public bool AfterCarRented { get; set; }
    public void ToggleProcessing() => IsProcessing = !IsProcessing;

    #region Feedback

    public bool IsInputValid;
    public readonly List<string> InputFeedbackMessages = new();
    public string DataValues = string.Empty;

    public void FeedbackMessageAdd(List<string> errorMessages)
    {
        InputFeedbackMessages.AddRange(errorMessages);
    }

    #endregion

    #region Search

    public string? UserSearch { get; set; }

    public IEnumerable<string> SearchResult;

    #endregion

    #region New Vehicle

    public string LicensePlate { get; set; } = string.Empty;
    private VehicleManufacturer VehManufacturer { get; set; }
    public int? Odometer { get; set; }
    public double? CostKm { get; set; }
    public VehicleType? VehType { get; private set; }
    public int? CostDay { get; set; }
    public VehicleStatus VisibleVehicle { get; set; }
    public UserInputError UserInputError { get; set; } = new();

    #endregion

    #region New Customer

    private long SocialSecurityNumber { get; set; }

    public string? SsnString
    {
        get => SocialSecurityNumber.ToString("000-00-0000");
        set
        {
            if (value == null) return;
            var parsed = UserDataParsing.RemoveHyphensFrom(value);
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

    public async Task<bool> TestTryAddVehicle()
    {
        ClearFeedbackMessage();
        LicensePlate = UserDataParsing.ParseNewVehicle(LicensePlate, Odometer, VehManufacturer, VehType,
            CostKm, CostDay, UserInputError);
        FeedbackMessageAdd(UserInputError.ErrorMessages());
        if (InputFeedbackMessages.Count > 0) return true;
        return false;
    }

    private async Task TryAddNewVehicle()
    {
        ClearFeedbackMessage();
        try
        {
            LicensePlate = UserDataParsing.ParseNewVehicle(LicensePlate, Odometer, VehManufacturer, VehType,
                CostKm, CostDay, UserInputError);
            FeedbackMessageAdd(UserInputError.ErrorMessages());
            if (InputFeedbackMessages.Count > 0) return;
            //if (InputFeedbackMessages.Count == 0) AfterCarRented = false;
            ValidUserInputData = ValidUserInputData.Vehicle;
            await _bp.HandleUserInput(this);
            ClearData();
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

        ClearData();
    }

    private async Task TryReturnVehicle()
    {
        ClearFeedbackMessage();
        try
        {
            Distance = UserDataParsing.ParseDistance(Distance);
            ParseVehicle();
            ValidUserInputData = ValidUserInputData.Returning;
            await _bp.HandleUserInput(this);
        }
        catch (Exception e)
        {
            InputFeedbackMessages.Add(e.Message);
        }

        ClearData();
    }

    private async Task TryAddNewCustomer()
    {
        ClearFeedbackMessage();
        try
        {
            if (string.IsNullOrEmpty(SsnString) || string.IsNullOrEmpty(FirstName) ||
                string.IsNullOrEmpty(LastName)) return;
            UserDataParsing.ParseNewCustomer(SsnString, FirstName, LastName);
            ValidUserInputData = ValidUserInputData.Customer;
            await _bp.HandleUserInput(this);
            ClearData();
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

    private void ParseDistance()
    {
        DataValues += $"Distance: {Distance}";
        if (Distance is not null or > 0) return;
        Distance = 0;
    }

    private void ParseVehicle()
    {
        if (ReturnVehicle is not null) return;
        ReturnVehicle = null;
        throw new ArgumentException("Return Vehicle missing");
    }

    #endregion

    #region UserEvents

    public void ev_Search(ChangeEventArgs e)
    {
        if (e.Value is null) return;
        Enum.TryParse(e.Value.ToString(), out VehicleType type);
        var str = e.Value.ToString();
        if (str != null)
            _bp.GetSearchResults(str);
    }

    public async Task ev_AddNewCar()
    {
        if (InputFeedbackMessages.Count > 0)
        {
            TestTryAddVehicle();
            return;
        }
        await TryAddNewVehicle();
        AfterCarRented = true;
        if (InputFeedbackMessages.Count == 0) AfterCarRented = false;
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

    private void ClearData()
    {
        switch (ValidUserInputData)
        {
            case ValidUserInputData.Customer:
                ClearNewCustomerData();
                break;
            case ValidUserInputData.Vehicle:
                ClearVehicleData();
                break;
            case ValidUserInputData.Booking:
                ClearBookingData();
                break;
            case ValidUserInputData.Returning:
                ClearReturnData();
                break;
        }

        ValidUserInputData = default;
    }

    private void ClearVehicleData()
    {
        if (AfterCarRented == false) LicensePlate = string.Empty;
        Odometer = null;
        CostKm = null;
        CostDay = null;
        ValidUserInputData = default;
    }

    private void ClearBookingData()
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

    public void ClearFeedbackMessage()
    {
        InputFeedbackMessages.Clear();
        DataValues = string.Empty;
        IsInputValid = false;
        UserInputError.Clear();
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
}

public enum ValidUserInputData
{
    None,
    Customer,
    Vehicle,
    Booking,
    Returning
}