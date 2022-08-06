namespace MutualBank.Models.ViewModels.Message
{
    internal class MsgInfo
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string UserPhoto { get; set; }
        public int CasePoint { get; set; }
        public int UserPoint { get; set; }
        public List<MsgUserInPost>? MsgList { get; set; }
    }
}