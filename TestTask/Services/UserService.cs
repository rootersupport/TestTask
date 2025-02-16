using System.Collections.Generic;
using System.Threading.Tasks;
using TestTask.Models;
using TestTask.Repositories;

namespace TestTask.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }
        public async Task<User> AuthenticateAsync(string login, string password)
        {
            var user = await _userRepository.GetUserByLoginAsync(login);

            if (user == null || user.Password != password)
            {
                return null;
            }
            // Проверка блока
            if (user.UserState.Code == "Blocked")
            {
                throw new InvalidOperationException("Пользователь заблокирован.");
            }

            return user;
        }
        //public async Task<User> AuthenticateAsync(string login, string password)
        //{
        //    var user = await _userRepository.GetUserByLoginAsync(login);

        //    if (user == null || user.Password != password)
        //    {
        //        return null;
        //    }

        //    return user;
        //}
    }
}
