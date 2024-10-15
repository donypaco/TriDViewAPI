using System.ComponentModel.DataAnnotations;

namespace TriDViewAPI.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } 

        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

        [Required]
        [StringLength(50)]
        public string Level { get; set; } 

        [Required]
        public string Message { get; set; } 

        public string Exception { get; set; }
    }
}
