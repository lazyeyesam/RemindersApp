using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RemindersApp.Models;

namespace RemindersApp.Context
{
    public class ReminderContext : IdentityDbContext<AppUser>
    {
        public DbSet<ReminderList> ReminderLists { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            var path = Environment.GetFolderPath(folder);
            var dbPath = System.IO.Path.Join(path, "reminders.db");
            optionbuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
