using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;

public class ReservationSystem // Перейменовано з TableReservationApp
{
    static void Main(string[] args)
    {
        Serilog.Log.Logger = new LoggerConfiguration() // Додано логування
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            ReservationManager manager = new ReservationManager();
            manager.AddRestaurant("A", 10);
            manager.AddRestaurant("B", 5);

            Console.WriteLine(manager.BookTable("A", new DateTime(2023, 12, 25), 3));
            Console.WriteLine(manager.BookTable("A", new DateTime(2023, 12, 25), 3));
        }
        catch (Exception ex)
        {
            Log.Error($"Unhandled exception: {ex}");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}

public class ReservationManager // Перейменовано з ReservationManagerClass
{
    private readonly List<Restaurant> restaurants; // Перейменовано з res

    public ReservationManager()
    {
        restaurants = new List<Restaurant>();
    }

    public void LoadRestaurantsFromFile(string filePath)  // Перейменовано метод та аргумент
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath); // Перейменовано з ls та fileP
            foreach (string line in lines) // Перейменовано з l
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                {
                    AddRestaurant(parts[0], tableCount);
                }
                else
                {
                    Log.Warning($"Invalid line format: {line}");
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error LoadRestaurantsFromFile: {ex}");
            Environment.Exit(1);
        }
    }

    public void AddRestaurant(string name, int tableCount) // Змінено метод
    {
        try
        {
            restaurants.Add(new Restaurant(name, tableCount));
        }
        catch (Exception ex)
        {
            Log.Error($"Error AddRestaurant: {ex}");
        }
    }

    public IEnumerable<string> FindAllFreeTables(DateTime date) // Розділено метод
    {
        try
        {
            return restaurants.SelectMany(r => r.FindAllFreeTables(date));
        }
        catch (Exception ex)
        {
            Log.Error($"Error FindAllFreeTables: {ex}"); // Виведення деталей про помилку
            return null;
        }
    }

    public bool BookTable(string restaurantName, DateTime date, int tableNumber) // Змінено метод
    {
        try
        {
            var restaurant = restaurants.FirstOrDefault(r => r.Name == restaurantName);
            if (restaurant == null || tableNumber < 1 || tableNumber > restaurant.TableCount)
            {
                Log.Warning("Invalid reservation request."); // Змінено текст помилки
                return false;
            }

            return restaurant.BookTable(date, tableNumber);
        }
        catch (Exception ex)
        {
            Log.Error($"Error BookTable: {ex}"); // Виведення деталей про помилку
            return false;
        }
    }

    public void SortRestaurantsByAvailability(DateTime date) // Змінено метод
    {
        try
        {
            restaurants.Sort((r1, r2) => r2.CountAvailableTables(date).CompareTo(r1.CountAvailableTables(date)));
        }
        catch (Exception ex)
        {
            Log.Error($"Error SortRestaurantsByAvailability: {ex}");
        }
    }
}

public class Restaurant  // Перейменовано з RestaurantClass
{
    private readonly List<RestaurantTable> tables;

    public string Name { get; }
    public int TableCount => tables.Count;

    public Restaurant(string name, int tableCount) // Частину логіки винесено у конструктор
    {
        Name = name;
        tables = Enumerable.Range(1, tableCount).Select(i => new RestaurantTable(i)).ToList();
    }

    public IEnumerable<string> FindAllFreeTables(DateTime date) // Розділено метод
    {
        return tables.Where(t => !t.IsBooked(date)).Select(t => $"{Name} - Table {t.Number}");
    }

    public bool BookTable(DateTime date, int tableNumber) // Змінено метод
    {
        return tables[tableNumber - 1].Book(date);
    }

    public int CountAvailableTables(DateTime date) // Змінено метод
    {
        return tables.Count(t => !t.IsBooked(date));
    }
}

public class RestaurantTable // Перейменовано з RestaurantTableClass
{
    public int Number { get; }
    public List<DateTime> bookedDates { get; } = new List<DateTime>();  // Перейменовано з bd

    public RestaurantTable(int number)
    {
        Number = number;
    }

    public bool Book(DateTime date) // Перейменовано аргумент з d
    {
        try
        {
            if (bookedDates.Contains(date))
            {
                return false;
            }
            bookedDates.Add(date);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Book: {ex}"); // Виведення деталей про помилку
            return false;
        }
    }

    public bool IsBooked(DateTime date)  // Перейменовано аргумент з d
    {
        return bookedDates.Contains(date);
    }
}
