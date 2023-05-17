using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.IO;
using WebFrontToBack.Areas.Admin.ViewModels;
using WebFrontToBack.DAL;
using WebFrontToBack.Models;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<TeamMember> CreateTeamMemberVM = await _context.TeamMembers.ToListAsync();
            return View(CreateTeamMemberVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateTeamMemberVM CreateTeamMemberVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (CreateTeamMemberVM.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Phote", "image type to enter!");
                return View();
            }
            if (CreateTeamMemberVM.Photo.Length / 1024 > 200)
            {
                ModelState.AddModelError("Phote", "image Length to enter!");
                return View();

            }
            string fileName = Guid.NewGuid().ToString() + CreateTeamMemberVM.Photo.FileName;
            string root = _webHostEnvironment.WebRootPath;
            string resaultRoot = Path.Combine(root, "assets", "img", fileName);
            using (FileStream fileStream = new FileStream(resaultRoot, FileMode.Create))
            {
                await CreateTeamMemberVM.Photo.CopyToAsync(fileStream);
            }
            TeamMember teamMember = new TeamMember
            {
                FullName = CreateTeamMemberVM.FullName,
                ImagePath = fileName,
                Profession = CreateTeamMemberVM.Profession
            };

            await _context.TeamMembers.AddAsync(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


            //bool isExists = await _context.TeamMembers.AnyAsync(c =>
            //c.FullName.ToLower().Trim() == CreateTeamMemberVM.FullName.ToLower().Trim());

            //if (isExists)
            //{
            //    ModelState.AddModelError("FullName", "FullName name already exists");
            //    return View();
            //}
            //await _context.TeamMembers.AddAsync(teamMembers);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));

        }

            public IActionResult Update(int Id)
            {
                TeamMember? teamMember = _context.TeamMembers.Find(Id);

                if (teamMember == null)
                {
                    return NotFound();
                }

                return View(teamMember);
            }

            [HttpPost]
            public IActionResult Update(TeamMember teamMember)
            {
                TeamMember? editedTeamMember = _context.TeamMembers.Find(teamMember.Id);
                if (editedTeamMember == null)
                {
                    return NotFound();
                }
                editedTeamMember.FullName = teamMember.FullName;
                _context.TeamMembers.Update(editedTeamMember);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            public IActionResult Delete(int Id)
            {
                TeamMember? teamMember = _context.TeamMembers.Find(Id);
                if (teamMember == null)
                {
                    return NotFound();
                }
                _context.TeamMembers.Remove(teamMember);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
       
    }
}

