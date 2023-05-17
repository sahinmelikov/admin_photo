using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.Areas.Admin.ViewModels;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RecentWorksController : Controller
    {
        private readonly AppDbContext _Context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RecentWorksController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _Context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<RecentWorks> recentWorks =await _Context.RecentWorks.ToListAsync();
            return View(recentWorks);
        }
        [HttpGet]
         public IActionResult Create()
        {
            return View();
        }
       

      

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateREcetWorkVM workVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!workVM.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Phote", "image type to enter!");
                return View();
            }
            if (workVM.Photo.Length / 1024 > 200)
            {
                ModelState.AddModelError("Phote", "image Length to enter!");
                return View();

            }
            string fileName = Guid.NewGuid().ToString() + workVM.Photo.FileName;
            string root = _webHostEnvironment.WebRootPath;
            string resaultRoot = Path.Combine(root, "assets", "img", fileName);
            using (FileStream fileStream = new FileStream(resaultRoot, FileMode.Create))
            {
                await workVM.Photo.CopyToAsync(fileStream);
            }
            RecentWorks Works = new RecentWorks
            {
               Name  = workVM.Name,
                ImagePath = fileName,
                Description = workVM.Description
            };

            await _Context.RecentWorks.AddAsync(Works);
            await _Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
      
            //await _Context.RecentWorks.AddAsync(recentWorks);
            //await _Context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int Id)
        {
            RecentWorks? recentWorks = _Context.RecentWorks.Find(Id);
            if (recentWorks == null)
            {
                return NotFound();
            }
            return View(recentWorks);
        }
        [HttpPost]
        public IActionResult Update(RecentWorks recentWorks)
        {
            RecentWorks? editedRecentWorks = _Context.RecentWorks.Find(recentWorks.Id);
            if (editedRecentWorks == null)
            {
                return NotFound();
            }
            editedRecentWorks.Name = recentWorks.Name;
            _Context.RecentWorks.Update(editedRecentWorks);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            RecentWorks? recentWorks = _Context.RecentWorks.Find(id);
            if (recentWorks==null)
            {
                return NotFound();
            }
            _Context.RecentWorks.Remove(recentWorks);
            _Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
