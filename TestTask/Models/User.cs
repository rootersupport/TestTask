using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestTask.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }

        public int UserStateId { get; set; }
        public UserState UserState { get; set; }
    }
}
