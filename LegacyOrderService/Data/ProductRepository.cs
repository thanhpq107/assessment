
namespace LegacyOrderService.Data
{
    public class ProductRepository
    {
        private readonly Dictionary<string, double> _productPrices = new()
        {
            ["Widget"] = 12.99,
            ["Gadget"] = 15.49,
            ["Doohickey"] = 8.75
        };

        private readonly Dictionary<string, double> _cache = new();

        public double GetPrice(string productName)
        {
            if (string.IsNullOrEmpty(productName))
                throw new ArgumentException("productName");

            if (_cache.TryGetValue(productName, out var cached))
                return cached;

            // Simulate an expensive lookup
            Thread.Sleep(500);

            if (_productPrices.TryGetValue(productName, out var price))
            {
                _cache[productName] = price;
                return price;
            }

            throw new Exception("Product not found");
        }
    }
}
