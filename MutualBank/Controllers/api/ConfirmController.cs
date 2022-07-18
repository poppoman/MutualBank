using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MutualBank.Models;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using static MutualBank.Models.ViewModels.MemberModel;

namespace MutualBank.Controllers.api
{
    [Route("api/Confirm/{action}")]
    [ApiController]
    public class ConfirmController : ControllerBase
    {
        private readonly MutualBankContext _mutualBankContext;
		private readonly IConfiguration _configuration;

		public ConfirmController(MutualBankContext mutualBankContext, IConfiguration configuration)
        {
            _mutualBankContext = mutualBankContext;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<string> ConfirmAccount(string id)
        {
            var user = _mutualBankContext.Logins.FirstOrDefault(u => u.LoginName == id);
            if (user == null) return "OK";
            else return "有人使用";
        }

		public async Task<string> SendMailToken(string id)
		{

			// 檢查資料庫是否有這個帳號
			var user = _mutualBankContext.Logins.FirstOrDefault(u => u.LoginName == id);
			if (user !=null)
				{
				// 取出會員信箱
				string UserEmail = user.LoginEmail;

                // 取得系統自定密鑰，在 Web.config 設定
                string SecretKey = _configuration.GetValue<string>("Email:SecretKey");

					// 產生帳號+時間驗證碼
					string sVerify = id + "|" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

					// 將驗證碼使用 3DES 加密
					//TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
					//MD5 md5 = new MD5CryptoServiceProvider();
					//byte[] buf = Encoding.UTF8.GetBytes(SecretKey);
					//byte[] result = md5.ComputeHash(buf);
					//string md5Key = BitConverter.ToString(result).Replace("-", "").ToLower().Substring(0, 24);
					//DES.Key = UTF8Encoding.UTF8.GetBytes(md5Key);
					//DES.Mode = CipherMode.ECB;
					//ICryptoTransform DESEncrypt = DES.CreateEncryptor();
					//byte[] Buffer = UTF8Encoding.UTF8.GetBytes(sVerify);
					//sVerify = Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length)); // 3DES 加密後驗證碼

					// 將加密後密碼使用網址編碼處理
					sVerify = HttpUtility.UrlEncode(sVerify);

					// 網站網址
					 string webPath = new StringBuilder()
					 .Append(HttpContext.Request.Scheme)
					 .Append("://")
					 .Append(HttpContext.Request.Host)
					 .Append('/')
					 .ToString();

				// 從信件連結回到重設密碼頁面
				string receivePage = "UserLogin/resetPassword";

					// 信件內容範本
					string mailContent = "請點擊以下連結，返回網站重新設定密碼，逾期 30 分鐘後，此連結將會失效。<br><br>";
					//mailContent = $"{mailContent}<a href={webPath}{receivePage}?verify={sVerify}target=_blank>點此連結</a>";
					mailContent = $"{mailContent}<a href={webPath}{receivePage}?verify={sVerify}>點此連結</a>";


				// 信件主題
				string mailSubject = "[測試]密碼變更請求";

					//發信帳號密碼
					string MailUserID = _configuration.GetValue<string>("Email:MailUserID"); 
					string MailUserPwd = _configuration.GetValue<string>("Email:MailUserPwd"); 

					// Mail Server 發信
					string SmtpServer = _configuration.GetValue<string>("Email:SmtpServer");
					int SmtpPort = _configuration.GetValue<int>("Email:SmtpPort");
					MailMessage mms = new MailMessage();
					mms.From = new MailAddress(MailUserID);
					mms.Subject = mailSubject;
					mms.Body = mailContent;
					mms.IsBodyHtml = true;
					mms.SubjectEncoding = Encoding.UTF8;
					mms.To.Add(new MailAddress(UserEmail));
					using (SmtpClient client = new SmtpClient(SmtpServer, SmtpPort))
					{
						client.EnableSsl = true;
						client.Credentials = new NetworkCredential(MailUserID, MailUserPwd);//寄信帳密 
						client.Send(mms); //寄出信件
					}
				return "OK111";
			}
				else
				{
				return " NOT OK2";
				}
			
		}




		public async Task<string> DoResetPwd(string id)
		{
			//DoResetPwdOut outModel = new DoResetPwdOut();

			//// 檢查是否有輸入密碼
			//if (string.IsNullOrEmpty(inModel.NewUserPwd))
			//{
			//	outModel.ErrMsg = "請輸入新密碼";
			//	return Json(outModel);
			//}
			//if (string.IsNullOrEmpty(inModel.CheckUserPwd))
			//{
			//	outModel.ErrMsg = "請輸入確認新密碼";
			//	return Json(outModel);
			//}
			//if (inModel.NewUserPwd != inModel.CheckUserPwd)
			//{
			//	outModel.ErrMsg = "新密碼與確認新密碼不相同";
			//	return Json(outModel);
			//}
			 var userID = HttpContext.Session.GetString("ResetPwdUserId");
			// 檢查帳號 Session 是否存在
			if (userID == null || userID.ToString() == "")
			{
				//outModel.ErrMsg = "無修改帳號";
				//return Json(outModel);
				return "final NOOOOOOOO";
			}
			else 
			{
				var user = _mutualBankContext.Logins.FirstOrDefault(u=>u.LoginName ==userID);
				if (user != null)
				{
					user.LoginPwd = id;
					_mutualBankContext.SaveChanges();
					return "final OKKKKKKKK";
				}
				else 
				{
					return "user = null";
				}

				
				
			}
			
		}
	}
}
