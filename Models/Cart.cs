namespace megamart_backend.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 1:N Navigation to CartItem
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
