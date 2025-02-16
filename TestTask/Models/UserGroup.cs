using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestTask.Models
{
    public class UserGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserGroupId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } // "Admin" или "User"

        [MaxLength(200)]
        public string Description { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
