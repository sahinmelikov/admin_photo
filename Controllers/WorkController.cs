using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.ViewModel;

namespace WebFrontToBack.Controllers
{
    public class WorkController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public WorkController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            WorkVM WorkVM = new WorkVM()
            {
               
                WorkCategories = await _appDbContext.WorkCategories.Where(c => !c.IsDeleted).ToListAsync(),
                WorkServices = await _appDbContext.WorkServices
                .Include(s => s.Category)
                .OrderByDescending(s => s.Id)
                .Where(s => !s.IsDeleted)
                .ToListAsync()
            };
            return View(WorkVM);
        }
    }
}
