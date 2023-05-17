using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFrontToBack.Areas.Admin.ViewModels
{
    public class CreateTeamMemberVM
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Profession { get; set; }
       
        [Required, NotMapped]
        public IFormFile Photo { get; set; }
    }
}
