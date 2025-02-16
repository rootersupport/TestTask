using Microsoft.EntityFrameworkCore;
using TestTask.Models;

namespace TestTask.Data
{
    public class AppDbContext : DbContext
    {
        // Конструктор для настройки контекста
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet для каждой сущности
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserState> UserStates { get; set; }

        // Настройка моделей и связей
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User -> UserGroup
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserGroup)
                .WithMany(g => g.Users)
                .HasForeignKey(u => u.UserGroupId);

            // User -> UserState
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserState)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.UserStateId);

            // Начальнве данные
            modelBuilder.Entity<UserGroup>().HasData(
                new UserGroup { UserGroupId = 1, Code = "Admin", Description = "Administrator" },
                new UserGroup { UserGroupId = 2, Code = "User", Description = "Regular user" }
            );

            // Начальные данные
            modelBuilder.Entity<UserState>().HasData(
                new UserState { UserStateId = 1, Code = "Active", Description = "Active user" },
                new UserState { UserStateId = 2, Code = "Blocked", Description = "Blocked user" }
            );
        }
    }
}
