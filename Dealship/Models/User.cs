﻿namespace Dealship.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }
        public string PasswordHash { get; set; } // Захешований пароль

        public string Role { get; set; } // "Admin" або "Client"
    }
}

