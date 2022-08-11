namespace MutualBank.Areas.Admin.Models.ViewModel
{
    public class CaseApiModel
    {
        public int CaseId { get; set; }
        public int CaseUserId { get; set; }
        /// <summary>
        /// 0提供 1需要
        /// </summary>
        public bool CaseNeedHelp { get; set; }
        public int CaseSkilId { get; set; }
        public DateTime CaseAddDate { get; set; }
        public DateTime CaseReleaseDate { get; set; }
        public DateTime CaseExpireDate { get; set; }
        public DateTime? CaseClosedDate { get; set; }
        public DateTime CaseClosedIn { get; set; }

        public string? CaseCloseDisplay
        {
            set => CaseClosedIn.ToString("yyyy-MM-dd");
            get => (CaseClosedIn.ToString("yyyy-MM-dd") != "2022-08-06") ? CaseClosedIn.ToString("yyyy-MM-dd") : "未結案";
        }
        public string CaseTitle { get; set; } = null!;
        public string CaseIntroduction { get; set; } = null!;
        public string? CasePhoto { get; set; }
        public string? CaseSerDate { get; set; }
        public int? CaseSerArea { get; set; }

        public int CasePoint { get; set; }
        public bool CaseIsExecute { get; set; }
        public string? CaseReleaseString { get; internal set; }
        public string? CaseExpireString { get; internal set; }
        public string? CaseAddString { get; internal set; }
        public string? userFullName { get; internal set; }
        public string? SerAreaDisplay { get; set; }
    }
}
