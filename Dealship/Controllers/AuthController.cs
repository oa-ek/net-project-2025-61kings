using Dealship.Models;
using Microsoft.AspNetCore.Mvc;

public class AuthController : Controller
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /Auth/Register
    public IActionResult Register() => View();

    // POST: /Auth/Register
    [HttpPost]
    public IActionResult Register(User user, string password)
    {
        if (_context.Users.Any(u => u.Email == user.Email))
        {
            ModelState.AddModelError("", "Користувач з таким email вже існує.");
            return View();
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        user.Role = "User";
        _context.Users.Add(user);
        _context.SaveChanges();

        return RedirectToAction("Login");
    }

    // GET: /Auth/Login
    public IActionResult Login() => View();

    // POST: /Auth/Login
    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            ModelState.AddModelError("", "Неправильний email або пароль.");
            return View();
        }

        // Тут можна зберегти дані в сесію (додамо пізніше)
        TempData["UserName"] = user.FullName;
        TempData["UserRole"] = user.Role;

        // Перевірка ролі користувача
        if (user.Role == "Admin")
        {
            return RedirectToAction("Index", "Home"); // Якщо роль Admin, перекидуємо на головну сторінку для адміна
        }

        return RedirectToAction("Index", "Main"); // В іншому випадку на головну сторінку для звичайного користувача
    }

}
