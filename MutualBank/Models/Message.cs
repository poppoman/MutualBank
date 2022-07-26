using System;
using System.Collections.Generic;

namespace MutualBank.Models
{
    public partial class Message
    {
        public Message()
        {
            InverseMsgParent = new HashSet<Message>();
        }

        public int MsgId { get; set; }
        public DateTime MsgAddDate { get; set; }
        public int? MsgCaseId { get; set; }
        public int? MsgUserId { get; set; }
        public int MsgToUserId { get; set; }
        public string? MsgContent { get; set; }
        public int? MsgParentId { get; set; }
        public bool MsgIsRead { get; set; }

        public virtual Case? MsgCase { get; set; }
        public virtual Message? MsgParent { get; set; }
        public virtual User MsgToUser { get; set; } = null!;
        public virtual User? MsgUser { get; set; }
        public virtual ICollection<Message> InverseMsgParent { get; set; }
    }
}
