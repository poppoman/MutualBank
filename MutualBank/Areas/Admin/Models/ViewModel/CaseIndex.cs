using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MutualBank.Areas.Admin.Models.ViewModel
{
    public class CaseIndex
    {
        public int CaseId { get; set; }
        public int CaseUserId { get; set; }
        [Display(Name = "po 文者")]
        public string? UserName { get; set; }
        /// <summary>
        /// 0提供 1需要
        /// </summary>
        public bool CaseNeedHelp { get; set; }
        [Display(Name = "類型")]
        public string CaseNeedDisplay 
        {
            set => CaseNeedHelp.ToString();
            get => (CaseNeedHelp.ToString() != "True")? "提供":"需要";
        }
        [Display(Name = "技能")]
        public string? CaseSkil { get; set; }
        [Display(Name = "發佈時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? CaseReleaseDate { get; set; }
        [Display(Name = "到期時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? CaseExpireDate { get; set; }

        public DateTime CaseClosedIn { get; set; }

        [Display(Name = "結案日期")]
        public string? CaseCloseDisplay
        {
            set => CaseClosedIn.ToString("yyyy-MM-dd");
            get => (CaseClosedIn.ToString("yyyy-MM-dd")!= "2022-08-06" )? CaseClosedIn.ToString("yyyy-MM-dd"): "未結案";
        }

        [Display(Name = "標題")]
        public string? CaseTitle { get; set; }
        [Display(Name = "可服務時間")]
        public string? CaseSerDate { get; set; }
        [Display(Name = "所在地")]
        public string? SerArea { get; set; }
        [Display(Name = "點數")]
        public int CasePoint { get; set; }
        /// <summary>
        /// 0: 等待 1:交易中
        /// </summary>
        [Display(Name = "交易狀態")]
        public bool? CaseIsExecute { get; set; }
    }
}
