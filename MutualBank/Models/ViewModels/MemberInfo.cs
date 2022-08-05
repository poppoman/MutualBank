namespace MutualBank.Models.ViewModels
{
    public class MemberInfo
    {
        public string UserFname { get; set; }
        public string UserLname { get; set; }
        public string UserNname { get; set; }
        public bool? UserSex { get; set; }
        public int? UserSkillId { get; set; }
        public string UserEmail { get; set; }
        public string UserSchool { get; set; }
        public string UserResume { get; set; }
        public string UserCv { get; set; }
        public Area? UserAreaId { get; set; }
        public string? UserBirthday { get; set; }
    }
    public class SkillInfo 
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
    }
}
