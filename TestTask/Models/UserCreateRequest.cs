using System.ComponentModel.DataAnnotations;

namespace TestTask.Models
{
    public class UserCreateRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int UserGroupId { get; set; }

        public int UserStateId { get; set; } = 1; // По умолчанию статус "Active"
    }
}
