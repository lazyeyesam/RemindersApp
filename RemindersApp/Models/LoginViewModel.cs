﻿namespace RemindersApp.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Error { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
