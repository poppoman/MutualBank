namespace MutualBank.Models
{
    internal class MessageChVM
    {
        public int? MsgCaseId { get; set; }
        public int? MsgParentId { get; set; }
        public DateTime MsgAddDate { get; set; }
        public string MsgContent { get; set; }
        public int? MsgUserId { get; set; }
        public string MsgtoUserName { get; set; }
        public string MsgUserName { get; set; }
        public string MagUserPhoto { get; set; }
    }
}