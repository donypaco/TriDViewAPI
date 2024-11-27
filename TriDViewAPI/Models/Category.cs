using System.ComponentModel.DataAnnotations;

namespace TriDViewAPI.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string CategoryName { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();

    }
}
