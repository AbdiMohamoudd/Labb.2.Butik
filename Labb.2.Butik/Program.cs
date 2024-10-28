public class Product
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Name} - {Price:C}";
    }
}

public class Customer
{
    public string Name { get; private set; }
    public string Password { get; }
    private List<Product> _cart;
    public List<Product> Cart { get { return _cart; } }

    public Customer(string name, string password)
    {
        Name = name;
        Password = password;
        _cart = new List<Product>();
    }

    public void AddToCart(Product product)
    {
        _cart.Add(product);
    }

    public decimal GetTotalPrice()
    {
        decimal total = 0;
        foreach (var product in _cart)
        {
            total += product.Price;
        }
        return total;
    }

    public bool VerifyPassword(string password)
    {
        return Password == password;
    }

    public override string ToString()
    {
        string cartInfo = string.Join("\n", _cart);
        return $"Name: {Name}\nKundvagn:\n{cartInfo}\nTotalpris: {GetTotalPrice():C}";
    }
}

class Program
{
    static List<Customer> customers = new List<Customer>
    {
        new Customer("Knatte", "123"),
        new Customer("Fnatte", "321"),
        new Customer("Tjatte", "213")
    };

    static void Main(string[] args)
    {
        List<Product> storeProducts = new List<Product>
        {
            new Product("Korv", 22.00m),
            new Product("Dricka", 20.00m),
            new Product("Äpple", 10.00m)
        };

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("Välkommen till butiken!");
            Console.WriteLine("1. Registrera ny kund");
            Console.WriteLine("2. Logga in");
            Console.WriteLine("3. Avsluta");
            Console.Write("Välj ett alternativ: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    RegisterCustomer();
                    break;
                case "2":
                    Customer loggedInCustomer = LoginCustomer();
                    if (loggedInCustomer != null)
                    {
                        HandleShopping(loggedInCustomer, storeProducts);
                    }
                    break;
                case "3":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    break;
            }
        }
    }

    static void RegisterCustomer()
    {
        Console.Write("Ange namn: ");
        string name = Console.ReadLine();
        Console.Write("Ange lösenord: ");
        string password = Console.ReadLine();

        Customer newCustomer = new Customer(name, password);
        customers.Add(newCustomer);

        Console.WriteLine($"Kunden {name} har registrerats.\n");
    }

    static Customer LoginCustomer()
    {
        Console.Write("Ange namn: ");
        string name = Console.ReadLine();
        Console.Write("Ange lösenord: ");
        string password = Console.ReadLine();

        foreach (var customer in customers)
        {
            if (customer.Name == name && customer.VerifyPassword(password))
            {
                Console.WriteLine("Inloggning lyckades!\n");
                return customer;
            }
        }

        Console.WriteLine("Felaktigt namn eller lösenord.\n");
        return null;
    }

    static void HandleShopping(Customer customer, List<Product> storeProducts)
    {
        bool shopping = true;

        while (shopping)
        {
            Console.WriteLine("Tillgängliga produkter:");
            for (int i = 0; i < storeProducts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {storeProducts[i]}");
            }

            Console.WriteLine("4. Visa kundvagn");
            Console.WriteLine("5. Gå till kassan");
            Console.WriteLine("6. Logga ut");
            Console.Write("Välj ett alternativ: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                case "2":
                case "3":
                    int productIndex = int.Parse(choice) - 1;
                    if (productIndex >= 0 && productIndex < storeProducts.Count)
                    {
                        customer.AddToCart(storeProducts[productIndex]);
                        Console.WriteLine($"{storeProducts[productIndex].Name} har lagts till i din kundvagn.\n");
                    }
                    break;
                case "4":
                    Console.WriteLine(customer);
                    break;
                case "5":
                    Console.WriteLine("Totalt att betala: " + customer.GetTotalPrice().ToString("C"));
                    customer.Cart.Clear();
                    shopping = false;
                    break;
                case "6":
                    shopping = false;
                    break;
                default:
                    Console.WriteLine("Ogiltigt val, försök igen.");
                    break;
            }
        }
    }
}
