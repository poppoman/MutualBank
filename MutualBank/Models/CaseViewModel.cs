namespace MutualBank.Models
{
    public class CaseViewModel
    {
        public int CaseId { get; set; }
        public int CaseUserId { get; set; }
        public bool CaseNeedHelp { get; set; }
        public int CaseSkillId { get; set; }
        public string CaseSkillName { get; set; }
        public DateTime CaseReleaseDate { get; set; }
        public DateTime CaseExpireDate { get; set; }
        public DateTime? CaseClosedDate { get; set; }
        public bool? IsCaseExpire { get; set; }
        public string CaseTitle { get; set; } = null!;
        public string CaseIntroduction { get; set; } = null!;
        public string? CasePhoto { get; set; }
        public string CaseSerDate { get; set; } = null!;
        public int? CaseSerArea { get; set; }
        public string CaseSerAreaName { get; set; }
        public string CaseUserName { get; set; } = null!;
        public int? MessageCount { get; set; }
        public string? UserPhoto { get; set; }
    }
}