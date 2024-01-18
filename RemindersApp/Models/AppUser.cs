using Microsoft.AspNetCore.Identity;

namespace RemindersApp.Models
{
    public class AppUser : IdentityUser
    {
        public List<ReminderList> ReminderLists { get; set; }
    }
}
