using System.ComponentModel.DataAnnotations;

namespace MutualBank.Areas.Admin.Models.ViewModel
{
    public partial class UserLogin
    {
        
        public int LoginId { get; set; }
        [Display(Name = "登入名稱")]
        public string LoginName { get; set; } = null!;
        [Display(Name = "登入密碼")]
        public string LoginPwd { get; set; } = null!;
        [Display(Name = "登入電子郵件")]
        public string LoginEmail { get; set; } = null!;
        [Display(Name = "登入層級")]
        public bool LoginLevel { get; set; }
        public DateTime LoginAddDate { get; set; }
        [Display(Name = "啟用狀態")]
        public bool LoginActive { get; set; }
        
        public int UserId { get; set; }
        [Display(Name = "姓氏")]
        public string? UserLname { get; set; }
        [Display(Name = "名字")]
        public string? UserFname { get; set; }
        [Display(Name = "暱稱")]
        public string? UserNname { get; set; }
        [Display(Name = "性別")]
        public bool? UserSex { get; set; }
        public string? UserHphoto { get; set; }
        [Display(Name = "生日")]
        public DateTime? UserBirthday { get; set; }
        public string? UserEmail { get; set; }
        public int? UserSkillId { get; set; }
        public int? UserAreaId { get; set; }
        public string? UserCV { get; set; }
        public string? UserResume { get; set; }
        public string? UserSchool { get; set; }
        public string? UserFaculty { get; set; }
        public int UserPoint { get; set; }
        [Display(Name = "姓名")]
        public string? UserFullname { get; internal set; }
    }
}