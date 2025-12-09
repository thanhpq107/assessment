using System.Globalization;

namespace LegacyOrderService.Models
{
    public record Order
    {
        /// <summary>
        /// Customer placing the order. Required.
        /// </summary>
        public required string CustomerName { get; init; }

        /// <summary>
        /// Product being ordered. Required.
        /// </summary>
        public required string ProductName { get; init; }

        /// <summary>
        /// Quantity ordered. Defaults to 0.
        /// </summary>
        public int Quantity { get; init; } = 0;

        /// <summary>
        /// Price per unit. Defaults to 10.0 (double).
        /// </summary>
        public double Price { get; init; } = 10.0;

        /// <summary>
        /// Validate the order and throw an exception if invalid.
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(CustomerName))
                throw new ArgumentException("CustomerName is required", nameof(CustomerName));

            if (string.IsNullOrWhiteSpace(ProductName))
                throw new ArgumentException("ProductName is required", nameof(ProductName));

            if (Quantity < 0)
                throw new ArgumentOutOfRangeException(nameof(Quantity), "Quantity cannot be negative");

            if (Price < 0)
                throw new ArgumentOutOfRangeException(nameof(Price), "Price cannot be negative");
        }

        public override string ToString()
            => $"{CustomerName} ordered {Quantity} x {ProductName} @ {Price.ToString("C", CultureInfo.CurrentCulture)}";
    }
}
