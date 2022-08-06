namespace MutualBank.Models.ViewModels
{
    public class MemberInfo
    {
        public string? UserFname { get; set; }
        public string? UserLname { get; set; }
        public string? UserNname { get; set; }
        public bool? UserSex { get; set; }
        public int? UserSkillId { get; set; }
        public string? UserEmail { get; set; }
        public string? UserSchool { get; set; }
        public string? UserResume { get; set; }
        public string? UserCv { get; set; }
        public Area? UserAreaId { get; set; }
        public string? UserBirthday { get; set; }
        public string? UserHphoto { get; set; }
    }
    public class SkillInfo 
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
    }

    public class MemberUpdate
    {
        public string? UserFname { get; set; }
        public string? UserLname { get; set; }
        public string? UserNname { get; set; }
        public bool? UserSex { get; set; }
        public int? UserSkillId { get; set; }
        public string? UserBirthday { get; set; }
        public string? City { get; set; }
        public string? Town { get; set; }
        public string? UserSchool { get; set; }
        public string? UserResume { get; set; }
        public string? UserCv { get; set; }        
        public string? UserPhoto { get; set; }

        public object blob { get; set; }
    }

    public class Sex 
    {
        public string SexName { get; set; }
        public bool? SexId { get; set; }
    }
}
