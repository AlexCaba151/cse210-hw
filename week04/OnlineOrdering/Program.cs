using System;
using System.Collections.Generic;

// Address Class
class Address
{
    private string street;
    private string city;
    private string state;
    private string country;

    public Address(string street, string city, string state, string country)
    {
        this.street = street;
        this.city = city;
        this.state = state;
        this.country = country;
    }

    public bool IsInUSA()
    {
        return country.ToLower() == "usa";
    }

    public string GetFullAddress()
    {
        return $"{street}\n{city}, {state}\n{country}";
    }
}

// Customer Class
class Customer
{
    private string name;
    private Address address;

    public Customer(string name, Address address)
    {
        this.name = name;
        this.address = address;
    }

    public bool LivesInUSA()
    {
        return address.IsInUSA();
    }

    public string GetName()
    {
        return name;
    }

    public string GetShippingAddress()
    {
        return address.GetFullAddress();
    }
}

// Product Class
class Product
{
    private string name;
    private string productId;
    private double price;
    private int quantity;

    public Product(string name, string productId, double price, int quantity)
    {
        this.name = name;
        this.productId = productId;
        this.price = price;
        this.quantity = quantity;
    }

    public double GetTotalCost()
    {
        return price * quantity;
    }

    public string GetProductDetails()
    {
        return $"{name} (ID: {productId})";
    }
}

// Order Class
class Order
{
    private List<Product> products;
    private Customer customer;

    public Order(Customer customer)
    {
        this.customer = customer;
        this.products = new List<Product>();
    }

    public void AddProduct(Product product)
    {
        products.Add(product);
    }

    public double GetTotalPrice()
    {
        double total = 0;
        foreach (var product in products)
        {
            total += product.GetTotalCost();
        }

        double shippingCost = customer.LivesInUSA() ? 5 : 35;
        return total + shippingCost;
    }

    public string GetPackingLabel()
    {
        string label = "Packing Label:\n";
        foreach (var product in products)
        {
            label += product.GetProductDetails() + "\n";
        }
        return label;
    }

    public string GetShippingLabel()
    {
        return $"Shipping Label:\n{customer.GetName()}\n{customer.GetShippingAddress()}";
    }
}

// Program Class (Main)
class Program
{
    static void Main()
    {
        // Create customers
        Customer customer1 = new Customer("John Doe", new Address("123 Main St", "New York", "NY", "USA"));
        Customer customer2 = new Customer("Alice Smith", new Address("456 Maple Ave", "Toronto", "ON", "Canada"));

        // Create orders
        Order order1 = new Order(customer1);
        order1.AddProduct(new Product("Laptop", "LPT1001", 1200, 1));
        order1.AddProduct(new Product("Mouse", "MS200", 25, 2));

        Order order2 = new Order(customer2);
        order2.AddProduct(new Product("Headphones", "HD500", 100, 1));
        order2.AddProduct(new Product("Keyboard", "KB700", 80, 1));
        order2.AddProduct(new Product("Monitor", "MON900", 300, 1));

        // Display results
        Console.WriteLine(order1.GetPackingLabel());
        Console.WriteLine(order1.GetShippingLabel());
        Console.WriteLine($"Total Price: ${order1.GetTotalPrice():0.00}\n");

        Console.WriteLine(order2.GetPackingLabel());
        Console.WriteLine(order2.GetShippingLabel());
        Console.WriteLine($"Total Price: ${order2.GetTotalPrice():0.00}");
    }
}
