using System.ComponentModel.DataAnnotations;

namespace TestTask.Models
{
    public class UserUpdateRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int UserGroupId { get; set; }

        [Required]
        public int UserStateId { get; set; }
    }

}
