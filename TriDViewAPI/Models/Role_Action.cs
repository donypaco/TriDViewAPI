using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TriDViewAPI.Models
{
    public class Role_Action
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleActionID { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        public int ActionId { get; set; }

        [ForeignKey("ActionId")]
        public Action Action { get; set; }

    }
}
