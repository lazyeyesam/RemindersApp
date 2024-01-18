using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RemindersApp.Context;
using RemindersApp.Models;
using System.Diagnostics;

namespace RemindersApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ReminderContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ReminderContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // get the list of reminders from the database
            var user = await _userManager.FindByNameAsync(User!.Identity!.Name);
            var list = _context.ReminderLists.Include(list => list.User).Where(list => list.User.Id == user.Id).AsQueryable();
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ReminderListViewModel listViewModel)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            // get the user
            var user = await _userManager.FindByNameAsync(User!.Identity!.Name);

            // create the new reminder list and assign it to the user
            var list = new ReminderList();
            list.Name = listViewModel.Name;
            list.Description = listViewModel.Description;
            list.User = user;

            // save it to the database
            _context.ReminderLists.Add(list);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        
        public IActionResult Delete(int id)
        {
            // get the reminder list from the database, including the reminders too so they get deleted as well
            var list = _context.ReminderLists.Include(list => list.Reminders).SingleOrDefault(list => list.Id == id);
            if (list != null)
            {
                // delete it from the database
                _context.ReminderLists.Remove(list);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}