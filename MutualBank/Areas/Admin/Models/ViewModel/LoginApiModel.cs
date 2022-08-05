namespace MutualBank.Areas.Admin.Models.ViewModel
{
    public class LoginApiModel
    {
        public int loginId { get; set; }
        public string loginName { get; set; } = null!;
        public string? loginPwd { get; set; }
        public string? loginEmail { get; set; }
    }
}
