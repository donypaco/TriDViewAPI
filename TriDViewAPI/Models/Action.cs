using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TriDViewAPI.Models
{
    public class Action
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActionID { get; set; }

        [Required]
        [StringLength(50)]
        public string ActionName { get; set; }

        public List<Role_Action> Role_Actions { get; set; } = new List<Role_Action>();
    }
}
