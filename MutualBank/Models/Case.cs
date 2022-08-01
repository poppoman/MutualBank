using System;
using System.Collections.Generic;

namespace MutualBank.Models
{
    public partial class Case
    {
        public Case()
        {
            Messages = new HashSet<Message>();
            Points = new HashSet<Point>();
        }

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
        public string CaseTitle { get; set; } = null!;
        public string CaseIntroduction { get; set; } = null!;
        public string? CasePhoto { get; set; }
        public string? CaseSerDate { get; set; }
        public int? CaseSerArea { get; set; }
        public int CasePoint { get; set; }
        public bool CaseIsExecute { get; set; }

        public virtual Area? CaseSerAreaNavigation { get; set; }
        public virtual Skill CaseSkil { get; set; } = null!;
        public virtual User CaseUser { get; set; } = null!;
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Point> Points { get; set; }
    }
}
