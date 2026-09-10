namespace megamart_backend.Models
{
    public class SecondaryImage
    {
        public string ImageUrl { get; set; } = string.Empty;
        public string ImagePublicId { get; set; } = string.Empty;
    }

    public class Specifications
    {
        public string Model { get; set; } = string.Empty;
        public string Warranty { get; set; } = string.Empty;
        public string Delivery { get; set; } = string.Empty;
    }

}
