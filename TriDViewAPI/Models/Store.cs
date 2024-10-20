using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TriDViewAPI.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int? UserID { get; set; }
        public User? UserRegistered { get; set; }

        public DateTimeOffset DateTimeRegistered { get; set; } = DateTimeOffset.UtcNow;

        [Required]
        [MaxLength(255)]
        public string StoreName { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        [MaxLength(255)]
        public string StoreLocation { get; set; }

        public string? LogoKey { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("Plan")]
        public int PlanID { get; set; }
        public Plan Plan { get; set; }
        public bool IsApproved { get; set; } = false;  
        public string? ApprovalNotes { get; set; }
        public DateTimeOffset? ApprovalDate { get; set; }
        public List<Store> Stores { get; set; } = new List<Store>();
        public List<Product> Products { get; set; } = new List<Product>();

    }
}
