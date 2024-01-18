using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindersApp.Context;
using RemindersApp.Models;

namespace RemindersApp.Controllers
{
    [Authorize]
    public class ReminderController : Controller
    {
        private readonly ReminderContext _context;

        public ReminderController(ReminderContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            // get the reminder list and the reminders
            var list = _context.ReminderLists.Include(list => list.Reminders).SingleOrDefault(list => list.Id == id);
            if (list == null)
                return RedirectToAction("Index", "Home");

            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ReminderViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            // get the reminder list to link with the new reminder
            var list = _context.ReminderLists.Find(viewModel.ReminderListId);
            if (list == null)
                return RedirectToAction("Index");

            // create the new reminder and add the values
            var reminder = new Reminder();
            reminder.Name = viewModel.Name;
            reminder.Details = viewModel.Details;
            reminder.DateDue = viewModel.DateDue;
            reminder.Priority = viewModel.Priority;
            reminder.ReminderList = list;

            // save it to the database
            _context.Reminders.Add(reminder);
            _context.SaveChanges();

            return RedirectToAction("Index", new { id = viewModel.ReminderListId });
        }

        public IActionResult Delete(int id)
        {
            // get the reminder from the database
            var reminder = _context.Reminders.Include(reminder => reminder.ReminderList).SingleOrDefault(reminder => reminder.Id == id);
            if (reminder == null)
                return RedirectToAction("Index", "Home");

            // delete it from the database
            _context.Reminders.Remove(reminder);
            _context.SaveChanges();

            return RedirectToAction("Index", new { reminder.ReminderList.Id });
        }

        public IActionResult Complete(int id)
        {
            // get the reminder from the database
            var reminder = _context.Reminders.Include(reminder => reminder.ReminderList).SingleOrDefault(reminder => reminder.Id == id);
            if (reminder == null)
                return RedirectToAction("Index", "Home");

            // swap the complete value and save to the database
            reminder.IsComplete = !reminder.IsComplete;
            _context.Reminders.Update(reminder);
            _context.SaveChanges();

            return RedirectToAction("Index", new { reminder.ReminderList.Id });
        }

        public IActionResult Edit(int id)
        {
            // get the reminder from the database
            var reminder = _context.Reminders.Include(reminder => reminder.ReminderList).SingleOrDefault(reminder => reminder.Id == id);
            if (reminder == null)
                return RedirectToAction("Index", "Home");

            return View(reminder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateReminder(ReminderViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Home");

            // get the reminder by reminder id
            var reminder = _context.Reminders.Include(reminder => reminder.ReminderList).SingleOrDefault(reminder => reminder.Id == viewModel.ReminderId);
            if (reminder == null)
                return RedirectToAction("Index", "Home");

            // change the fields of the reminder
            reminder.Name = viewModel.Name;
            reminder.Details = viewModel.Details;
            reminder.DateDue = viewModel.DateDue;
            reminder.Priority = viewModel.Priority;
            reminder.IsComplete = false;

            // save to the database
            _context.Reminders.Update(reminder);
            _context.SaveChanges();

            return RedirectToAction("Index", new { reminder.ReminderList.Id });
        }
    }
}
