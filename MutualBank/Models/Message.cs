using System;
using System.Collections.Generic;

namespace MutualBank.Models
{
    public partial class Message
    {
        public int MsgId { get; set; }
        public DateTime MsgAddDate { get; set; }
        public int? MsgCaseId { get; set; }
        public int? MsgUserId { get; set; }
        public string? MsgContent { get; set; }
        public int MsgGroupId { get; set; }
        public bool MsgGroupP { get; set; }

        public virtual Case? MsgCase { get; set; }
        public virtual User? MsgUser { get; set; }
    }
}
