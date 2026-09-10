namespace megamart_backend.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
        public string ImagePublicId { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // JSONB / JSON Columns (Required)
        public List<SecondaryImage> SecondaryImages { get; set; } = new();
        public Specifications Specifications { get; set; } = new();

        // 1:N Navigation to OrderItem
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
