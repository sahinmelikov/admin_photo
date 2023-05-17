namespace WebFrontToBack.Models
{
    public class WorkServiceImage
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public bool IsActive { get; set; }
        public int ServiceId { get; set; }
        public WorkService Service { get; set; }
    }
}
