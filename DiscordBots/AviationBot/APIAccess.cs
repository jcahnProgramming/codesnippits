using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class APIAccess
{
	static HttpClient client = new HttpClient();
    private static string APIKey = "ecfb4e-5bbeda";

    static async Task<Airplane> GetAircraftInfoAsync(string path)
    {
        Airplane aircraft = null;

        HttpResponseMessage msg = await client.GetAsync(path);
        if (msg.IsSuccessStatusCode)
        {
            aircraft = await msg.Content.ReadFromJsonAsync<Airplane>();
        }
        return aircraft;
    }

    static void Main()
    {
        RunAsync().GetAwaiter().GetResult();
    }

    static async Task<Uri> CreateAirplaneAsync(Airplane airplane)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("api/products", airplane);
        response.EnsureSuccessStatusCode();

        return response.Headers.Location;
    }

    static async Task<Uri> CreateTrackedFlight(TrackedFlight flight)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("api/products", flight);
        response.EnsureSuccessStatusCode();

        return response.Headers.Location;
    }

    static async Task RunAsync()
    {
        client.BaseAddress = new Uri($"https://aviation-edge.com/v2/public/flights?key={APIKey}&lat={SavedData.Instance.Lat}&lng={SavedData.Instance.Long}&distance=1000&status=en-route");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        try
        {

            TrackedFlight flight = new TrackedFlight();

            //create aircraft
            var url = await CreateTrackedFlight(flight);
            SavedData.Instance.AddTrackedFlight(flight);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.ReadLine();
    }
}
