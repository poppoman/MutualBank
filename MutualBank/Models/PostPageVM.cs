using System.ComponentModel.DataAnnotations;

namespace MutualBank.Models
{
    internal class PostPageVM
    {
        public Case? Case { get; set; }
        public Skill? Skill { get; set; }
        public Area? Area { get; set; }
        public int CaseId { get; set; }
        public string CaseTitle { get; set; }

        public string? CasePhoto { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime CasesAddDate { get; set; }

        public string? CaseSerDate { get; set; }
        public string CaseIntroduction { get; set; } = null!;
        public string SkillName { get; set; }

        public string? Areacity { get; set; }


    }
}