namespace MutualBank.Areas.Admin.Models.ViewModel
{
    public class ApiUserLoginModel
    {
        public int? loginId { get; set; }
        public string loginName { get; set; } = null!;
        public string? loginPwd { get; set; }
        public string? loginEmail { get; set; }
        public int userId { get; set; }
        public int? userAreaId { get; set; }
        public DateTime? userBirthday { get; set; }
        public string? userCv { get; set; }
        public string? userEmail { get; set; }
        public string? userFaculty { get; set; }
        public string? userFname { get; set; }
        public string? userHphoto { get; set; }
        public string? userLname { get; set; }
        public string? userNname { get; set; }
        public int userPoint { get; set; }
        public string? userResume { get; set; }
        public string? userSchool { get; set; }
        public bool? userSex { get; set; }
        public int? userSkillId { get; set; }

    }
}
