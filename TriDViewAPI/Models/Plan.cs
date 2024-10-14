using System.ComponentModel.DataAnnotations;

namespace TriDViewAPI.Models
{
    public class Plan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string PlanName { get; set; }

        public List<Store> Stores { get; set; } = new List<Store>();
    }
}
