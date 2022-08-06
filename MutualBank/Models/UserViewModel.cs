using System.ComponentModel.DataAnnotations;

namespace MutualBank.Models
{
     public partial class UserViewModel
    {
        public int UserId { get; set; }
        public string? UserLname { get; set; }
        public string? UserFname { get; set; }
        public string? UserNname { get; set; }
        /// <summary>
        /// 0-女 1-男
        /// </summary>
        public string? UserSex { get; set; }
        public string? UserHphoto { get; set; }
        public string? UserEmail { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public string? UserBirthday { get; set; }
        public int? UserSkillId { get; set; }
        public int? UserAreaId { get; set; }
        public string? UserArea { get; set; }
        public string? UserCv { get; set; }
        public string? UserResume { get; set; }
        public string? UserSchool { get; set; }
        public string? UserFaculty { get; set; }
        public int UserPoint { get; set; }
    }
}
