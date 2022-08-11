namespace MutualBank.Areas.Admin.Models.ViewModel
{
    public class UserApiModel
    {
        public int userId { get; set; }
        public int? userAreaId { get; set; }
        public DateTime? userBirthday { get; set; }
        public DateTime userBnotNull { get; set; }
        public string? userDate {
            set => userBnotNull.ToString("yyyy-MM-dd");
            get => userBnotNull.ToString("yyyy-MM-dd") != "2022-08-06" ?
                   userBnotNull.ToString("yyyy-MM-dd") : null;
        }
        public string? userCv { get; set; }
        public string? userEmail { get; set; }
        public string? userFaculty { get; set; }
        public string? userFname { get; set; }
        public string? userLname { get; set; }
        public string? userNname { get; set; }
        public int userPoint { get; set; }
        public string? userResume { get; set; }
        public string? userSchool { get; set; }
        public bool? userSex { get; set; }
        public int? userSkillId { get; set; }
        public string? userAreaDisplay { get; set; }
    }
}
