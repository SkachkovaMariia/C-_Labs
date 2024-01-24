using System;

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
    private Product product;

    public void AddProduct(string name, int quantity, double price)
    {
        if (product != null && product.Name == name)
        {
            product.Quantity += quantity;
        }
        else
        {
            product = new Product(name, quantity, price);
        }
    }

    public void RemoveProduct()
    {
        product = null;
    }

    public void UpdateProduct(string name, int quantity, double price)
    {
        if (product != null)
        {
            product.Name = name;
            product.Quantity = quantity;
            product.Price = price;
        }
    }

    public Product FindProduct()
    {
        return product;
    }

    public void PrintInventory()
    {
        if (product != null)
        {
            Console.WriteLine(product);
        }
        else
        {
            Console.WriteLine("Inventory is empty.");
        }
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
        Product foundProduct = inventory.FindProduct();

        if (foundProduct != null)
        {
            Console.WriteLine($"\nProduct found:\n{foundProduct}");
        }
        else
        {
            Console.WriteLine("Inventory is empty.");
        }
    }

    static void PrintInventory(InventorySystem inventory)
    {
        inventory.PrintInventory();
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

        inventory.AddProduct(name, quantity, price);
        Console.WriteLine($"Product '{name}' added to the inventory.");
    }

    static void RemoveProduct(InventorySystem inventory)
    {
        inventory.RemoveProduct();
        Console.WriteLine("Product removed from the inventory.");
    }

    static void UpdateProduct(InventorySystem inventory)
    {
        Product currentProduct = inventory.FindProduct();

        if (currentProduct != null)
        {
            Console.WriteLine($"\nCurrent details:\n{currentProduct}");

            Console.Write("\nEnter new name: ");
            string newName = Console.ReadLine();

            Console.Write("Enter new quantity: ");
            int newQuantity = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter new price: ");
            double newPrice = Convert.ToDouble(Console.ReadLine());

            inventory.UpdateProduct(newName, newQuantity, newPrice);

            Console.WriteLine($"Product updated successfully.");
            Console.WriteLine($"\nUpdated details:\n{inventory.FindProduct()}");
        }
        else
        {
            Console.WriteLine("Inventory is empty.");
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
