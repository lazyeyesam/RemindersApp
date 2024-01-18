namespace RemindersApp.Models
{
    public class AppUserViewModel
    {
        public string Email { get; set; }
        public string? CurrentPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string? Error { get; set; }
    }
}
