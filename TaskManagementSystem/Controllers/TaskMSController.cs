using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    public class TaskMSController : Controller
    {

        private static List<TaskItem> _tasks = new List<TaskItem>();

        public IActionResult Index(string status)
        {
            var tasks = _tasks;

            if (!string.IsNullOrEmpty(status))
            {
                tasks = _tasks
                    .Where(t => t.Status == status)
                    .ToList();
            }

            return View(tasks);
        }


        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                task.Id = _tasks.Count == 0
                ? 1
                : _tasks.Max(t => t.Id) + 1;

                _tasks.Add(task);

                return RedirectToAction("Index");
            }

            return View(task);
        }

        public IActionResult Edit(int Id)
        {
           var exist = _tasks.Find(x => x.Id == Id);
            if (exist == null)
            {
                return NotFound();
            }
            return View(exist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }

            var exist = _tasks.Find(x => x.Id == task.Id);
            if(exist != null)
            {
                exist.Title = task.Title;
                exist.Description = task.Description;
                exist.Status = task.Status;
                exist.DueDate = task.DueDate;
                exist.Priority = task.Priority;

                return RedirectToAction("Index");
            }

            return NotFound();
        }

        public IActionResult Delete(int Id) 
        {
            var exist = _tasks.Find(x => x.Id == Id);
            if(exist == null)
            {
                return NotFound();
            }

            return View(exist);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(int Id) 
        {
            var task = _tasks.Find(x => x.Id == Id);

            if (task == null)
            {
                return NotFound();
            }

            _tasks.Remove(task);

            return RedirectToAction("Index");
        }
    }
}
