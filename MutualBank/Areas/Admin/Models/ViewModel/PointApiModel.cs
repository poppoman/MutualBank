namespace MutualBank.Areas.Admin.Models.ViewModel
{
    public class PointApiModel
    {
        public int PointId { get; set; }
        public DateTime PointAddDate { get; set; }
        public int PointCaseId { get; set; }
        public bool PointNeedHelp { get; set; }
        public int PointUserId { get; set; }
        public int PointQuantity { get; set; }
        public bool PointIsDone { get; set; }
        public string? PointSpgorder { get; set; }
    }
}
