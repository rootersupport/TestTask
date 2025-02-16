namespace TestTask.Models
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserGroup { get; set; } // Название группы
        public string UserState { get; set; } // Название состояния
    }
}
