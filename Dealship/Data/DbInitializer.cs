using Dealship.Models;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        // Автоматично застосовує міграції
        context.Database.Migrate();

        // Перевірка: чи є адмін в базі
        if (!context.Users.Any(u => u.Role == "Admin"))
        {
            var admin = new User
            {
                FullName = "Admin User",
                Email = "admin@dealship.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), // Пароль "admin123"
                Role = "Admin"
            };

            context.Users.Add(admin);
            context.SaveChanges();
        }
    }
}
