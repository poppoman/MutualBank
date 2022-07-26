using System.ComponentModel.DataAnnotations;

namespace MutualBank.Models
{
    internal class MessageVM
    {
        [Key]
        public int? MsgCaseId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime MsgAddDate { get; set; }
        public string MsgContent { get; set; }
        public int? MsgUserId { get; set; }
        public string MsgtoUserName { get; set; }
        public string MsgUserName { get; set; }
        public string? MagUserPhoto { get; set; }
    }
}