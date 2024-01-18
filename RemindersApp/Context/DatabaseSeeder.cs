using Microsoft.AspNetCore.Identity;
using RemindersApp.Models;

namespace RemindersApp.Context
{
    public class DatabaseSeeder
    {
        private ReminderContext _context;
        private UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseSeeder(ReminderContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            // make sure the database exists
            _context.Database.EnsureCreated();

            // check if the database already has data
            var users = _context.Users.Count();
            if (users > 0) return;

            // create the roles
            _ = await _roleManager.CreateAsync(new IdentityRole("Admin"));
            _ = await _roleManager.CreateAsync(new IdentityRole("User"));

            // seed our database
            // add first user (you might need to add roles too)
            var user = new AppUser();
            user.UserName = "sam@reminders.com";
            user.Email = "sam@reminders.com";

            var password = "Sam123reminder!";
            _ = _userManager.CreateAsync(user, password);
            _ = _userManager.AddToRoleAsync(user, "Admin");

            // add reminder list for that user
            var reminderList = new ReminderList();
            reminderList.Name = "Starting List";
            reminderList.Description = "List Description";
            reminderList.User = user;

            _context.ReminderLists.Add(reminderList);
            await _context.SaveChangesAsync();
        }
    }
}
