using System;
using System.Collections.Generic;

public enum MenuOptions
{
    Execute = 1,
    Exit,
}

public class Product
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }

    public Product(string name, int quantity, double price)
    {
        Name = name;
        Quantity = quantity;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Name}\tQuantity: {Quantity}\tPrice: {Price:C}";
    }
}

public class InventorySystem
{
    private List<Product> products = new List<Product>();

    public void AddProduct(Product product)
    {
        Product existingProduct = FindProduct(product.Name);

        if (existingProduct != null)
        {
            existingProduct.Quantity += product.Quantity;
        }
        else
        {
            products.Add(product);
        }
    }

    public void RemoveProduct(Product product)
    {
        products.Remove(product);
    }

    public void UpdateProduct(Product product, string name, int quantity, double price)
    {
        product.Name = name;
        product.Quantity = quantity;
        product.Price = price;
    }

    public Product FindProduct(string name)
    {
        return products.Find(p => p.Name == name);
    }

    public List<Product> GetInventory()
    {
        return products;
    }
}

class WarehouseSystem
{
    static void Execute(InventorySystem inventory)
    {
        while (true)
        {
            Console.WriteLine("\nChoose an action:");
            Console.WriteLine("1.) Add Product");
            Console.WriteLine("2.) Remove Product");
            Console.WriteLine("3.) Print Inventory");
            Console.WriteLine("4.) Search Product by Name");
            Console.WriteLine("5.) Update Product");
            Console.WriteLine("6.) Back to Main Menu");

            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddProduct(inventory);
                    break;
                case 2:
                    RemoveProduct(inventory);
                    break;
                case 3:
                    PrintInventory(inventory);
                    break;
                case 4:
                    SearchProductByName(inventory);
                    break;
                case 5:
                    UpdateProduct(inventory);
                    break;
                case 6:
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void SearchProductByName(InventorySystem inventory)
    {
        Console.Write("\nEnter the name of the product to search: ");
        string productName = Console.ReadLine();

        Product foundProduct = inventory.FindProduct(productName);

        if (foundProduct != null)
        {
            Console.WriteLine($"\nProduct found:\n{foundProduct}");
        }
        else
        {
            Console.WriteLine($"Product '{productName}' not found in the inventory.");
        }
    }

    static void PrintInventory(InventorySystem inventory)
    {
        List<Product> products = inventory.GetInventory();

        if (products.Count > 0)
        {
            Console.WriteLine("\nInventory:");
            foreach (Product product in products)
            {
                Console.WriteLine(product);
            }
        }
        else
        {
            Console.WriteLine("\nInventory is empty.");
        }
    }

    static void AddProduct(InventorySystem inventory)
    {
        Console.WriteLine("\nEnter product details:");
        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Quantity: ");
        int quantity = Convert.ToInt32(Console.ReadLine());
        Console.Write("Price: ");
        double price = Convert.ToDouble(Console.ReadLine());

        Product newProduct = new Product(name, quantity, price);
        inventory.AddProduct(newProduct);

        Console.WriteLine($"Product '{name}' added to the inventory.");
    }

    static void RemoveProduct(InventorySystem inventory)
    {
        Console.Write("\nEnter the name of the product to remove: ");
        string productName = Console.ReadLine();

        Product productToRemove = inventory.FindProduct(productName);

        if (productToRemove != null)
        {
            inventory.RemoveProduct(productToRemove);
            Console.WriteLine($"Product '{productName}' removed from the inventory.");
        }
        else
        {
            Console.WriteLine($"Product '{productName}' not found in the inventory.");
        }
    }

    static void UpdateProduct(InventorySystem inventory)
    {
        Console.Write("\nEnter the name of the product to update: ");
        string productName = Console.ReadLine();

        Product productToUpdate = inventory.FindProduct(productName);

        if (productToUpdate != null)
        {
            Console.WriteLine($"\nCurrent details:\n{productToUpdate}");

            Console.Write("\nEnter new name: ");
            string newName = (Console.ReadLine().ToString());

            Console.Write("Enter new quantity: ");
            int newQuantity = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter new price: ");
            double newPrice = Convert.ToDouble(Console.ReadLine());

            inventory.UpdateProduct(productToUpdate, newName, newQuantity, newPrice);

            Console.WriteLine($"Product '{productName}' updated successfully.");
            Console.WriteLine($"\nUpdated details:\n{productToUpdate}");
        }
        else
        {
            Console.WriteLine($"Product '{productName}' not found in the inventory.");
        }
    }

    static void Menu()
    {
        Console.WriteLine($"Choose an option:");
        Console.WriteLine($"1.) Execute program");
        Console.WriteLine($"2.) Exit");

        var selectedOption = (MenuOptions)Convert.ToByte(Console.ReadLine());

        InventorySystem inventory = new InventorySystem();

        switch (selectedOption)
        {
            case MenuOptions.Execute:
                Execute(inventory);
                Console.ReadLine();
                break;
            case MenuOptions.Exit:
                Environment.Exit(0);
                break;
            default:
                break;
        }
    }

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Menu();
        }
    }
}
