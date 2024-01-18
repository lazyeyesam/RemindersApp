namespace RemindersApp.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public DateTime DateDue { get; set; }
        public ReminderPriority Priority { get; set; }
        public ReminderList ReminderList { get; set; }
        public bool IsComplete { get; set; }
    }

    public enum ReminderPriority
    {
        Low,
        Medium,
        High,
    }
}
