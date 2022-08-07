using System.ComponentModel.DataAnnotations;

namespace MutualBank.Areas.Admin.Models.ViewModel
{
    public class SkillsIndex
    {
        public int SkillId { get; set; }
        [Display(Name = "技能名稱")]
        public string SkillName { get; set; } = null!;
    }
}
