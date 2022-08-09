using System.ComponentModel.DataAnnotations;

namespace MutualBank.Areas.Admin.Models.ViewModel
{
    public class PointsIndex
    {
        public int PointId { get; set; }
        [Display(Name = "建立時間")]
        [DisplayFormat(DataFormatString = "{0: yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime PointAddDate { get; set; }
        [Display(Name = "卡片標題")]
        public string? PoCaseTitle { get; set; }
        [Display(Name = "類型")]
        public string? PointNeedDisplay { get; set; }
        public int PointUserId { get; set; }
        [Display(Name = "點數變更者")]
        public string? PointUserName { get; set; }
        [Display(Name = "數量")]
        public int PointQuantity { get; set; }
        public bool PointIsDone { get; set; }
    }
}
