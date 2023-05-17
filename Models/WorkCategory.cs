using System.ComponentModel.DataAnnotations;

namespace WebFrontToBack.Models
{
    public class WorkCategory
    {
        public WorkCategory()
        {
            Services = new List<WorkService>();
        }
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public virtual List<WorkService> Services { get; set; }
    }
}
