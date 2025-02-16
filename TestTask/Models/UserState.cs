using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestTask.Models
{
    public class UserState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserStateId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } // "Active" или "Blocked"

        [MaxLength(200)]
        public string Description { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
