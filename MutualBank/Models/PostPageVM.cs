﻿using System.ComponentModel.DataAnnotations;

namespace MutualBank.Models
{
    public class PostPageVM
    {
        [Key]
        public int CaseId { get; set; }
        public Message Message { get; set; }
        public string CaseTitle { get; set; }

        public string? CasePhoto { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CasesAddDate { get; set; }

        public string? CaseSerDate { get; set; }
        public string CaseIntroduction { get; set; } = null!;

        public string SkillName { get; set; }
        public string UserNName { get; set; } = null!;
        public string? UserPhoto { get; set; }

        public string? Areacity { get; set; }

        public string? LoginHPhoto { get; set; }

        public int? MsgCaseId { get; set; }
        public DateTime MsgAddDate { get; set; }
        public int? MsgUserId { get; set; }
        public string? MsgContent { get; set; }

    }
}