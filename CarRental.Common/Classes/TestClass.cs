using static System.Net.WebRequestMethods;
using static System.Net.Http.HttpClient;
using System.Collections.Generic;
using CarRental.Common.Classes;
using CarRental.Common.Interfaces;
using System.Net.Http.Json;

namespace CarRental.Common.Classes;

public class TestClass

{
    private HttpClient _http;
    public string Message { get; private set; } = string.Empty;
    public VehicleData[] Vehicles;
    public TestClass(HttpClient http)
    {
        _http = http;
    }
    public async Task<VehicleData[]> LoadVehicles()
    {

        var les = await _http.GetFromJsonAsync<VehicleData[]>("sample-data/vehicles.json");

        if (les != null)
        {
            Vehicles = les;
        }
        return les;
    }
    
}
public class VehicleData
{
    public string LicencePlate { get; set; }
    public string Make { get; set; }
}
