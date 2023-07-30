using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;
using Models;
using Serilog;

namespace BusReporter;

// todo fix folders naming, i.e. bus-reporter -> bustracker
public class Reporter : BackgroundService
{
    private readonly IHttpClientFactory _http;
    private readonly ILogger _logger;
    
    public Reporter(IHttpClientFactory http)
    {
        _http = http;
        _logger = Log.ForContext<Reporter>();
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var meta = await _http.CreateClient().GetFromJsonAsync<Meta>($" http://localhost:5077/v2/metadata", cancellationToken: stoppingToken);

        if (meta!.Buses.Count == 0)
        {
            _logger.Information("No bus detected");
            return;
        }
        
        _logger.Information("Going to start reporter for {BusCount} buses", meta.Buses.Count);
        
        var processingBuses = new Task[meta.Buses.Count];
        var i = 0;
        foreach (var bus in meta.Buses.Values)
        {
            processingBuses[i] = ProcessBus(bus, stoppingToken);
            i++;
        }

        await Task.WhenAll(processingBuses);
    }

    private async Task ProcessBus(Meta.Bus bus, CancellationToken stoppingToken)
    {
        var reportInterval = TimeSpan.FromSeconds(3);
        var nextTimeToReport = DateTime.MinValue;
        const double speedDistance = 0.01; // km
        const int speedInterval = 100; // ms
        const int stayingOnBusStop = 1000;
        
        int i = 0;
        double shift = 0;
        bool backwards = false;
        
        while (true)
        {
            if(i == bus.Route.Length - 1)
            {
                backwards = true;
            } 
            else if (i == 0)
            {
                backwards = false;
            }
            
            stoppingToken.ThrowIfCancellationRequested();

            var from = bus.Route[i];
            var to = bus.Route[backwards ? i - 1 : i + 1];
            
            double distance = HaversineDistance(from, to);
            shift += speedDistance;
            if(shift > distance)
            {
                shift = distance;
            }

            Meta.Location nextLocation = InterpolateCoordinates(from, to, shift/distance);

            var now = DateTime.UtcNow;
            //if (now > nextTimeToReport)
            {
                try
                {
                    var geoLocation = new GeoLocation
                    {
                        Latitude = nextLocation.Latitude,
                        Longitude = nextLocation.Longitude,
                        Timestamp = DateTime.UtcNow
                    };
                    await _http.CreateClient().PostAsync($" http://localhost:5077/v2/buses/{bus.Name}/location",
                        JsonContent.Create(geoLocation), stoppingToken);

                    nextTimeToReport = now.Add(reportInterval);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            if(Math.Abs(shift - distance) < 0.001)
            {
                if (backwards) {
                    i--;
                }
                else{
                    i++;
                }
                shift = 0;

                // bus stop
                await Task.Delay(stayingOnBusStop, stoppingToken);
            }

            await Task.Delay(speedInterval, stoppingToken);
        }
    }
    
    // Function to convert degrees to radians
    private static double ToRadians(double degrees) => degrees * (Math.PI / 180);

    // Function to convert radians to degrees
    private static double ToDegrees(double radians) => radians * (180 / Math.PI);

    // Function to calculate the distance between two points in kilometers
    private static double HaversineDistance(Meta.Location from, Meta.Location to) 
    {
        const int r = 6371; // Earth's radius in kilometers

        // Convert latitude and longitude from degrees to radians
        double φ1 = ToRadians(from.Latitude);
        double φ2 = ToRadians(to.Latitude);
        double Δφ = ToRadians(to.Latitude - from.Latitude);
        double Δλ = ToRadians(to.Longitude - from.Longitude);

        // Calculate haversine of differences
        double a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
            Math.Cos(φ1) * Math.Cos(φ2) *
            Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);

        // Calculate central angle using atan2
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        // Calculate the distance in kilometers
        return r * c;
    }

    private static Meta.Location InterpolateCoordinates(Meta.Location from, Meta.Location to, double fraction) 
    {
        // Convert latitudes and longitudes from degrees to radians
        double φ1 = ToRadians(from.Latitude);
        double φ2 = ToRadians(to.Latitude);
        double λ1 = ToRadians(from.Longitude);
        double λ2 = ToRadians(to.Longitude);

        // Calculate the latitude and longitude of the point at the specified fraction of the distance
        double φC = φ1 + fraction * (φ2 - φ1);
        double λC = λ1 + fraction * (λ2 - λ1);

        // Convert the latitude and longitude of point C back to degrees
        return new Meta.Location
        {
            Latitude = ToDegrees(φC), 
            Longitude = ToDegrees(λC)
        };
    }
}