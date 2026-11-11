using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    [Authorize]
    public class TaskMSController : Controller
    {

        private readonly ApplicationDbContext _context;

        public TaskMSController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task <IActionResult> Index(string status)
        {
            var tasks = await _context.Tasks.ToListAsync();

            if (!string.IsNullOrEmpty(status))
            {
                tasks = tasks
                    .Where(t => t.Status == status)
                    .ToList();
            }

            return View(tasks);
        }

        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(task);
        }

        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> Edit(int Id)
        {
           var exist = await _context.Tasks.FindAsync(Id);
            if (exist == null)
            {
                return NotFound();
            }
            return View(exist);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }

            var exist = await _context.Tasks.FindAsync(task.Id);
            if(exist != null)
            {
                exist.Title = task.Title;
                exist.Description = task.Description;
                exist.Status = task.Status;
                exist.DueDate = task.DueDate;
                exist.Priority = task.Priority;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id) 
        {
            var exist = await _context.Tasks.FindAsync(Id);
            if(exist == null)
            {
                return NotFound();
            }

            return View(exist);

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int Id) 
        {
            var task = await _context.Tasks.FindAsync(Id);

            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
