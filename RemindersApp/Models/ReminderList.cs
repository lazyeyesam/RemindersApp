namespace RemindersApp.Models
{
    public class ReminderList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Reminder> Reminders { get; set; }
        public AppUser User { get; set; }
    }
}
