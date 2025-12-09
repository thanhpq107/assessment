using System.Globalization;
using LegacyOrderService.Models;
using LegacyOrderService.Data;

namespace LegacyOrderService
{
    class Program
    {
        static void Main(string[] args)
        {
            string name;
            Console.WriteLine("Welcome to Order Processor!");

            do
            {
                Console.WriteLine("Enter customer name:");
                name = Console.ReadLine()?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(name))
                    Console.WriteLine("Customer name cannot be empty. Please re-enter.");
            } while (string.IsNullOrEmpty(name));

            string product;
            double price;
            var productRepo = new ProductRepository();
            while (true)
            {
                Console.WriteLine("Enter product name:");
                product = Console.ReadLine()?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(product))
                {
                    Console.WriteLine("Product name cannot be empty. Please re-enter.");
                    continue;
                }

                try
                {
                    price = productRepo.GetPrice(product);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not find product or error occurred: {ex.Message}. Please re-enter.");
                }
            }

            int qty;
            while (true)
            {
                Console.WriteLine("Enter quantity:");
                var input = Console.ReadLine()?.Trim();
                if (!int.TryParse(input, out qty) || qty <= 0)
                {
                    Console.WriteLine("Invalid quantity. Please enter a positive integer.");
                    continue;
                }
                break;
            }

            Console.WriteLine("Processing order...");

            var order = new Order
            {
                CustomerName = name,
                ProductName = product,
                Quantity = qty,
                Price = price
            };

            try
            {
                order.Validate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Order is invalid: {ex.Message}");
                return;
            }

            double total = order.Quantity * order.Price;

            Console.WriteLine("Order complete!");
            Console.WriteLine("Customer: " + order.CustomerName);
            Console.WriteLine("Product: " + order.ProductName);
            Console.WriteLine("Quantity: " + order.Quantity);
            Console.WriteLine("Total: " + total.ToString("C", CultureInfo.CurrentCulture));

            Console.WriteLine("Saving order to database...");
            var repo = new OrderRepository();
            try
            {
                repo.Save(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save order: {ex.Message}");
            }
            Console.WriteLine("Done.");
        }
    }
}
