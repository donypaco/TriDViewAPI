namespace TriDViewAPI.DTO
{
    public class ProductDTO
    {
        public int ProductID { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public decimal? Height { get; set; }

        public decimal? Width { get; set; }

        public string ImageUrl { get; set; }

        public string SKU { get; set; }

        public decimal? Discount { get; set; }

        public string Tags { get; set; }

        public decimal? Weight { get; set; }

        public decimal? Rating { get; set; }
    }
}
