using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;

namespace TestTask.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.UserGroup)
                .Include(u => u.UserState)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.UserGroup)
                .Include(u => u.UserState)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task AddUserAsync(User user)
        {
            await Task.Delay(5000); // Задержка 5 секунд

            var userExists = await _context.Users.AnyAsync(u => u.Login == user.Login);
            if (userExists)
            {
                throw new InvalidOperationException("Пользователь с таким логином уже существует.");
            }

            if (user.UserStateId == 0)
            {
                user.UserStateId = 1; // 1 = Active
            }

            if (user.UserGroupId == 1) // 1 = Admin
            {
                var adminExists = await _context.Users.AnyAsync(u => u.UserGroupId == 1);
                if (adminExists)
                {
                    throw new InvalidOperationException("Не может быть более одного администратора.");
                }
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            // Проверка логина
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login && u.UserId != user.UserId);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Пользователь с таким логином уже существует.");
            }

            // Проверка кол-ва админов
            if (user.UserGroupId == 1) // 1 - Admin
            {
                var adminExists = await _context.Users.AnyAsync(u => u.UserGroupId == 1 && u.UserId != user.UserId);
                if (adminExists)
                {
                    throw new InvalidOperationException("Не может быть более одного администратора.");
                }
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.UserStateId = 2; // 2 = Block
                await _context.SaveChangesAsync();
            }
        }
    }
}