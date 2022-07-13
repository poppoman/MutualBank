namespace MutualBank.Models.ViewModels
{
    public class MemberModel
    {
        /// <summary>
        /// [寄送驗證碼]參數
        /// </summary>
        public class SendMailTokenIn
        {
            public string UserID { get; set; }
        }

        /// <summary>
        /// [寄送驗證碼]回傳
        /// </summary>
        public class SendMailTokenOut
        {
            public string ErrMsg { get; set; }
            public string ResultMsg { get; set; }
        }
    }
}
