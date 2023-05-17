using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    
        [Area("Admin")]
        public class ServiceController : Controller
        {
            private readonly AppDbContext _context;

            public ServiceController(AppDbContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Index()
            {
                ICollection<WorkService> workservices = await _context.WorkServices.ToListAsync();
                return View(workservices);
            }

            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(WorkService workservice)
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                bool isExists = await _context.WorkCategories.AnyAsync(c =>
                c.Name.ToLower().Trim() == workservice.Name.ToLower().Trim());

                if (isExists)
                {
                    ModelState.AddModelError("Name", "Category name already exists");
                    return View();
                }
                await _context.WorkServices.AddAsync(workservice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            public IActionResult Update(int Id)
            {
                WorkService? workService = _context.WorkServices.Find(Id);

                if (workService == null)
                {
                    return NotFound();
                }

                return View(workService);
            }

            [HttpPost]
            public IActionResult Update(WorkService workService)
            {
                WorkService? editedWorkService = _context.WorkServices.Find(workService.Id);
                if (editedWorkService == null)
                {
                    return NotFound();
                }
                editedWorkService.Name = workService.Name;
                _context.WorkServices.Update(editedWorkService);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            public IActionResult Delete(int Id)
            {
                WorkService? workService = _context.WorkServices.Find(Id);
                if (workService == null)
                {
                    return NotFound();
                }
                _context.WorkServices.Remove(workService);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }
    }

