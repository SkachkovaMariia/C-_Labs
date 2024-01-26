using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class ReservationManagerTests
    {
        [TestMethod]
        public void TestAddRestaurant()
        {
            ReservationManager manager = new ReservationManager();

            manager.AddRestaurant("A", 10);
            manager.AddRestaurant("B", 5);

            var restaurantCount = manager.GetType().GetField("restaurants", BindingFlags.NonPublic | BindingFlags.Instance)?.
                GetValue(manager) as List<Restaurant>;

            Assert.AreEqual(2, restaurantCount?.Count ?? 0);
        }

        [TestMethod]
        public void TestLoadRestaurantsFromFile()
        {
            ReservationManager manager = new ReservationManager();
            string filePath = "D:\\УДУ\\C#\\7\\ConsoleApp1\\load.txt";

            manager.LoadRestaurantsFromFile(filePath);

            Assert.AreEqual(518, manager.FindAllFreeTables(DateTime.Now).Count());
        }

        [TestMethod]
        public void TestFindAllFreeTables()
        {
            ReservationManager manager = new ReservationManager();
            manager.AddRestaurant("A", 5);
            DateTime bookingDate = new DateTime(2023, 12, 25);

            var freeTables = manager.FindAllFreeTables(bookingDate);

            Assert.IsNotNull(freeTables);
            Assert.AreEqual(5, freeTables.Count());
        }

        [TestMethod]
        public void TestBookTable_SuccessfulBooking()
        {
            ReservationManager manager = new ReservationManager();
            manager.AddRestaurant("A", 10);
            DateTime bookingDate = new DateTime(2023, 12, 25);

            bool bookingResult = manager.BookTable("A", bookingDate, 3);

            Assert.IsTrue(bookingResult);
        }

        [TestMethod]
        public void TestBookTable_InvalidRestaurant()
        {
            ReservationManager manager = new ReservationManager();
            DateTime bookingDate = new DateTime(2023, 12, 25);

            bool bookingResult = manager.BookTable("NonExistentRestaurant", bookingDate, 3);

            Assert.IsFalse(bookingResult);
        }

        [TestMethod]
        public void TestBookTable_InvalidTableNumber()
        {
            ReservationManager manager = new ReservationManager();
            manager.AddRestaurant("A", 5);
            DateTime bookingDate = new DateTime(2023, 12, 25);

            bool bookingResult = manager.BookTable("A", bookingDate, 10);

            Assert.IsFalse(bookingResult);
        }

        [TestMethod]
        public void TestSortRestaurantsByAvailability()
        {
            ReservationManager manager = new ReservationManager();
            manager.AddRestaurant("A", 5);
            manager.AddRestaurant("B", 10);
            DateTime bookingDate = new DateTime(2023, 12, 25);

            manager.SortRestaurantsByAvailability(bookingDate);

            var sortedRestaurants = manager.FindAllFreeTables(bookingDate);
            Assert.IsNotNull(sortedRestaurants);
            CollectionAssert.AreEqual(new List<string> { "B - Table 1", "B - Table 2", "B - Table 3",
                "B - Table 4", "B - Table 5", "B - Table 6", "B - Table 7", "B - Table 8", "B - Table 9", "B - Table 10",
                "A - Table 1", "A - Table 2", "A - Table 3", "A - Table 4", "A - Table 5" }, sortedRestaurants.ToList());
        }
    }

    [TestClass]
    public class RestaurantTests
    {
        [TestMethod]
        public void TestFindAllFreeTables()
        {
            Restaurant restaurant = new Restaurant("A", 5);
            DateTime bookingDate = new DateTime(2023, 12, 25);

            var freeTables = restaurant.FindAllFreeTables(bookingDate);

            Assert.IsNotNull(freeTables);
            Assert.AreEqual(5, freeTables.Count());
        }

        [TestMethod]
        public void TestBookTable_SuccessfulBooking()
        {
            Restaurant restaurant = new Restaurant("A", 10);
            DateTime bookingDate = new DateTime(2023, 12, 25);

            bool bookingResult = restaurant.BookTable(bookingDate, 3);

            Assert.IsTrue(bookingResult);
        }

        [TestMethod]
        public void TestCountAvailableTables()
        {
            Restaurant restaurant = new Restaurant("A", 5);
            DateTime bookingDate = new DateTime(2023, 12, 25);

            var availableTables = restaurant.CountAvailableTables(bookingDate);

            Assert.AreEqual(5, availableTables);
        }
    }

    [TestClass]
    public class RestaurantTableTests
    {
        [TestMethod]
        public void TestBook_SuccessfulBooking()
        {
            RestaurantTable table = new RestaurantTable(1);
            DateTime bookingDate = new DateTime(2023, 12, 25);

            bool bookingResult = table.Book(bookingDate);

            Assert.IsTrue(bookingResult);
        }

        [TestMethod]
        public void TestIsBooked()
        {
            RestaurantTable table = new RestaurantTable(1);
            DateTime bookingDate = new DateTime(2023, 12, 25);

            table.Book(bookingDate);
            bool isBooked = table.IsBooked(bookingDate);

            Assert.IsTrue(isBooked);
        }
    }
}
