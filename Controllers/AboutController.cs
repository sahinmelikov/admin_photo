using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFrontToBack.DAL;
using WebFrontToBack.ViewModel;

namespace WebFrontToBack.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AboutController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            AboutVM AboutVM = new AboutVM()
            {
               
                TeamMembers = await _appDbContext.TeamMembers.ToListAsync(),
                
            };
            return View(AboutVM);
        }
    }
}
