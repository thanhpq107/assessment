using Microsoft.Data.Sqlite;
using LegacyOrderService.Models;

namespace LegacyOrderService.Data
{
    public class OrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string? connectionString = null)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                _connectionString = connectionString;
            }
            else
            {
                _connectionString = $"Data Source={Path.Combine(AppContext.BaseDirectory, "orders.db")}";
            }
        }

        public void Save(Order order)
        {
            if (order is null) throw new ArgumentNullException(nameof(order));

            // Validate the order before saving
            order.Validate();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                using var command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = @"INSERT INTO Orders (CustomerName, ProductName, Quantity, Price)
                    VALUES ($customer, $product, $qty, $price)";

                command.Parameters.AddWithValue("$customer", order.CustomerName ?? string.Empty);
                command.Parameters.AddWithValue("$product", order.ProductName ?? string.Empty);
                command.Parameters.AddWithValue("$qty", order.Quantity);
                command.Parameters.AddWithValue("$price", order.Price);

                command.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    // ignore rollback errors
                }
                throw;
            }
        }

        /// <summary>
        /// Seed a single example order into the database.
        /// </summary>
        public void SeedBadData()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = @"INSERT INTO Orders (CustomerName, ProductName, Quantity, Price)
                                    VALUES ($customer, $product, $qty, $price)";

                cmd.Parameters.AddWithValue("$customer", "John");
                cmd.Parameters.AddWithValue("$product", "Widget");
                cmd.Parameters.AddWithValue("$qty", 9999);
                cmd.Parameters.AddWithValue("$price", 9.99);

                cmd.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                try { transaction.Rollback(); } catch { }
                throw;
            }
        }
    }
}
