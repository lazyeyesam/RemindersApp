namespace RemindersApp.Models
{
    public class ReminderViewModel
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public ReminderPriority Priority { get; set; }
        public DateTime DateDue { get; set; }
        public int? ReminderListId { get; set; }
        public int? ReminderId { get; set; }
    }
}
