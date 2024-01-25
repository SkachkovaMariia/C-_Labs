using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

public enum FlightStatus
{
    OnTime,
    Delayed,
    Cancelled,
    Boarding,
    InFlight
}

public class Flight
{
    public string FlightNumber { get; set; }
    public string Airline { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public FlightStatus Status { get; set; }
    public TimeSpan Duration { get; set; }
    public string AircraftType { get; set; }
    public string Terminal { get; set; }    
}

public class FlightInformationSystem
{
    private List<Flight> flights = new List<Flight>();

    public void LoadFlightsFromJson(string jsonFilePath)
    {
        try
        {
            if (File.Exists(jsonFilePath))
            {
                string jsonData = File.ReadAllText(jsonFilePath);
                var jsonObject = JObject.Parse(jsonData);

                if (jsonObject.TryGetValue("flights", out var flightsToken))
                {
                    var flightsList = flightsToken.ToObject<List<Flight>>();

                    if (flightsList != null)
                    {
                        flights = flightsList;
                        Console.WriteLine($"Flights loaded successfully from {jsonFilePath}.");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("No flights data found in the JSON file.");
                        Environment.Exit(1);
                    }
                }
                else
                {
                    Console.WriteLine("No 'flights' key found in the JSON file.");
                    Environment.Exit(1);
                }
            }
            else
            {
                Console.WriteLine("JSON file does not exist.");
                Environment.Exit(1);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data from JSON: {ex.Message}");
            Environment.Exit(1);
        }
    }

    public void SaveFlightsToJson(List<Flight> flights, string jsonFilePath)
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(flights, Formatting.Indented);
            File.WriteAllText(jsonFilePath, jsonData);
            Console.WriteLine($"Flights data saved to {jsonFilePath}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data to JSON: {ex.Message}");
            Environment.Exit(1);
        }
    }

    public int GetFlightsCount(List<Flight> flights)
    {
        try
        {
            var suitableFlights = flights.Count;
            Console.WriteLine($"\nFlights found - {suitableFlights}");
            return suitableFlights;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError counting flights: {ex.Message}");
            Environment.Exit(1);
            return 0;
        }
    }

    public void AddFlight(Flight flight)
    {
        try
        {
            if (flight != null)
            {
                flights.Add(flight);
                Console.WriteLine("\nFlight added successfully.");
            }
            else
            {
                Console.WriteLine("\nCannot add null flight.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError adding flight: {ex.Message}");
            Environment.Exit(1);
        }
    }

    public void RemoveFlight(string flightNumber)
    {
        try
        {
            var flightToRemove = flights.Find(f => f.FlightNumber == flightNumber);
            if (flightToRemove != null)
            {
                flights.Remove(flightToRemove);
                Console.WriteLine("\nFlight removed successfully.");
            }
            else
            {
                Console.WriteLine("\nFlight not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError removing flight: {ex.Message}");
            Environment.Exit(1);
        }
    }

    public Flight FindFligh(string flightNumber)
    {
        try
        {
            var foundFlight = flights.Find(f => f.FlightNumber == flightNumber);

            if (foundFlight != null)
            {
                Console.WriteLine($"Flight found: {foundFlight.FlightNumber}");
                return foundFlight;
            }
            else
            {
                Console.WriteLine($"Flight with number '{flightNumber}' not found.");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error finding flight: {ex.Message}");
            Environment.Exit(1);
            return null;
        }
    }

    public void PrintAll()
    {
        try
        {
            if (flights.Count > 0)
            {
                foreach (var flight in flights)
                {
                    Console.WriteLine($"\nFlight Number: {flight.FlightNumber}, Airline: {flight.Airline}, " +
                        $"Destination: {flight.Destination}, Departure Time: {flight.DepartureTime}, " +
                        $"Arrival Time: {flight.ArrivalTime}, Status: {flight.Status}, Duration: {flight.Duration}, " +
                        $"Aircraft Type: {flight.AircraftType}, Terminal: {flight.Terminal}");
                }
            }
            else
            {
                Console.WriteLine("\nNo flights available.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError printing flights: {ex.Message}");
            Environment.Exit(1);
        }
    }

    public void UpdateFlightStatus()
    {
        try
        {
            DateTime currentDateTime = DateTime.Now;

            foreach (var flight in flights)
            {
                if (currentDateTime < flight.DepartureTime)
                {
                    flight.Status = FlightStatus.OnTime;
                }
                else if (currentDateTime >= flight.DepartureTime && currentDateTime < flight.ArrivalTime)
                {
                    flight.Status = FlightStatus.InFlight;
                }
                else if (currentDateTime == flight.ArrivalTime)
                {
                    flight.Status = FlightStatus.Boarding;
                }
                else if (currentDateTime > flight.ArrivalTime.Add(flight.Duration))
                {
                    flight.Status = FlightStatus.Delayed;
                }
                else
                {
                    flight.Status = FlightStatus.Cancelled;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError flights updating: {ex.Message}");
            Environment.Exit(1);
        }
    }

    public List<Flight> Task1(string airline)
    {
        try
        {
            UpdateFlightStatus();

            var filteredFlights = flights
                .Where(f => f.Airline == airline)
                .OrderBy(f => f.DepartureTime)
                .ToList();
           

            if (filteredFlights.Count > 0)
            {
                GetFlightsCount(filteredFlights);
                SaveFlightsToJson(filteredFlights, "D:\\УДУ\\C#\\5\\task1.json");
            }
            else
            {
                Console.WriteLine("\nNo flights found for this airline.");
            }

            return filteredFlights;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError Task1: {ex.Message}");
            Environment.Exit(1);
            return null;
        }
    }

    public List<Flight> Task2()
    {
        try
        {
            UpdateFlightStatus();

            var filteredFlights = flights
                .Where(f => f.Status == FlightStatus.Delayed)
                .OrderBy(f => f.DepartureTime - DateTime.Now)
                .ToList();

            if (filteredFlights.Count > 0)
            {
                GetFlightsCount(filteredFlights);
                SaveFlightsToJson(filteredFlights, "D:\\УДУ\\C#\\5\\task2.json");
            }
            else
            {
                Console.WriteLine("\nNo delayed flights found.");
            }

            return filteredFlights;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError Task2: {ex.Message}");
            Environment.Exit(1);
            return null;
        }
    }

    public List<Flight> Task3(DateTime date)
    {
        try
        {
            UpdateFlightStatus();

            var filteredFlights = flights
                .Where(f => f.DepartureTime.Date == date.Date)
                .OrderBy(f => f.DepartureTime)
                .ToList();

            if (filteredFlights.Count > 0)
            {
                GetFlightsCount(filteredFlights);
                SaveFlightsToJson(filteredFlights, "D:\\УДУ\\C#\\5\\task3.json");
            }
            else
            {
                Console.WriteLine($"\nNo flights for {date.ToShortDateString()} found.");
            }

            return filteredFlights;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError Task3: {ex.Message}");
            Environment.Exit(1);
            return null;
        }
    }

    public List<Flight> Task4(DateTime startTime, DateTime endTime, string destination)
    {
        try
        {
            UpdateFlightStatus();

            var filteredFlights = flights
                .Where(f => f.DepartureTime.Date >= startTime && f.ArrivalTime <= endTime
                && f.Destination == destination)
                .OrderBy(f => f.DepartureTime)
                .ToList();

            if (filteredFlights.Count > 0)
            {
                GetFlightsCount(filteredFlights);
                SaveFlightsToJson(filteredFlights, "D:\\УДУ\\C#\\5\\task4.json");
            }
            else
            {
                Console.WriteLine($"\nNo flights for {startTime.ToShortDateString()} - {endTime.ToShortDateString()} found.");
            }

            return filteredFlights;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError Task4: {ex.Message}");
            Environment.Exit(1);
            return null;
        }
    }

    public List<Flight> Task5(DateTime startTime, DateTime endTime)
    {
        try
        {
            UpdateFlightStatus();

            var filteredFlights = flights
                .Where(f => f.ArrivalTime >= startTime && f.ArrivalTime <= endTime)
                .OrderBy(f => f.ArrivalTime)
                .ToList();

            if (filteredFlights.Count > 0)
            {
                GetFlightsCount(filteredFlights);
                SaveFlightsToJson(filteredFlights, "D:\\УДУ\\C#\\5\\task5.json");
            }
            else
            {
                Console.WriteLine($"\nNo flights found for {startTime} - {endTime}.");
            }

            return filteredFlights;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError Task5: {ex.Message}");
            Environment.Exit(1);
            return null;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var flightSystem = new FlightInformationSystem();
        string jsonFilePath = "D:\\УДУ\\C#\\5\\flights_data.json";
        string airline = "MAU";
        string destination = "New York";
        DateTime date = DateTime.ParseExact("2023-08-16", "yyyy-MM-dd", null);
        DateTime startTime = DateTime.ParseExact("2023-01-23", "yyyy-MM-dd", null);
        DateTime endTime = DateTime.ParseExact("2023-01-25", "yyyy-MM-dd", null);

        flightSystem.LoadFlightsFromJson(jsonFilePath);
        //flightSystem.PrintAll();
        flightSystem.Task1(airline);
        flightSystem.Task2();
        flightSystem.Task3(date);
        flightSystem.Task4(startTime, endTime, destination);
        flightSystem.Task5(startTime, endTime);
    }
}
