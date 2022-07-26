namespace MutualBank.Areas.Admin.Models.ViewModel
{
    internal class UserLogin
    {
        public string LoginName { get; set; } = null!;
        public string LoginPwd { get; set; } = null!;
        public string LoginEmail { get; set; } = null!;
        public bool LoginLevel { get; set; }
        public DateTime LoginAddDate { get; set; }
        public bool LoginActive { get; set; }
        public int UserId { get; set; }
        public string? UserLname { get; set; }
        public string? UserFname { get; set; }
        public string? UserNname { get; set; }
        public bool? UserSex { get; set; }
        public string? UserHphoto { get; set; }
        public DateTime? UserBirthday { get; set; }
        public string? UserEmail { get; set; }
        public int? UserSkillId { get; set; }
        public int? UserAreaId { get; set; }
        public string? UserCV { get; set; }
        public string? UserResume { get; set; }
        public string? UserSchool { get; set; }
        public string? UserFaculty { get; set; }
        public int UserPoint { get; set; }
    }
}