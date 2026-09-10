namespace megamart_backend.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // 1:N Navigation to Product
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
