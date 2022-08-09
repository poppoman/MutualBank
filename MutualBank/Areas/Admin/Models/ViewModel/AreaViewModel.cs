namespace MutualBank.Areas.Admin.Models.ViewModel
{
    public class AreaViewModel
    {
        public int AreaId { get; set; }
        public string AreaCity { get; set; } = null!;
        public string AreaTown { get; set; } = null!;
        public string SerAreaDisplay { get; set; } = null!;
        
    }
}
