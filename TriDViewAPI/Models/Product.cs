using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TriDViewAPI.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [ForeignKey("Store")]
        public int StoreID { get; set; }
        public virtual Store Store { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Category { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Height { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Width { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(1024)]
        public string ImageUrl { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        public bool IsFeatured { get; set; } = false;

        [MaxLength(50)]
        public string SKU { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Discount { get; set; }

        [MaxLength(255)]
        public string Tags { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Weight { get; set; }

        [Column(TypeName = "decimal(3, 2)")]
        public decimal? Rating { get; set; }
    }
}
