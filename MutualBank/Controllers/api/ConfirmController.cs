using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MutualBank.Models;
using MutualBank.Models.ViewModels;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using static MutualBank.Models.ViewModels.CaseTitle;

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
		public ActionResult<CaseTitle> helpme(string id)
		{
			var userid = _mutualBankContext.Logins.Where(x => x.LoginName == id).Select(x => x.LoginId).FirstOrDefault();
			var casetitle = (_mutualBankContext.Cases.Where(x => x.CaseUserId == userid && x.CaseNeedHelp==true).Select(x => x.CaseTitle)).ToList();
			var caseid = (_mutualBankContext.Cases.Where(x => x.CaseUserId == userid && x.CaseNeedHelp == true).Select(x => x.CaseId)).ToList();
            List<string[]> msg = new List<string[]>();
            List<string[]> msgUsername = new List<string[]>();
			List<string> msgname = new List<string>();
			foreach (int a in caseid)
			{
				var msgContent = _mutualBankContext.Messages.Where(x => x.MsgCaseId == a).Select(x => x.MsgContent).ToArray();
				var msgUserid = (_mutualBankContext.Messages.Where(x => x.MsgCaseId == a).Select(x => x.MsgUserId)).ToList();
				foreach (int b in msgUserid)
				{
					var Username = (_mutualBankContext.Users.Where(x => x.UserId == b).Select(x => x.UserNname)).FirstOrDefault();					
					msgname.Add(Username);
				}
				msg.Add(msgContent);
				msgUsername.Add(msgname.ToArray());
				msgname.Clear();

			}
			CaseTitle helpme = new CaseTitle() 
			{
				caseid=caseid,
				casetitle=casetitle,
			};
			//helpme.msgname = msgUsername;
			//helpme.casemsg = msg;
            return helpme;
		}

		[HttpGet]
		public ActionResult<CaseTitle> helpyou(string id)
		{
			var userid = _mutualBankContext.Logins.Where(x => x.LoginName == id).Select(x => x.LoginId).FirstOrDefault();
			var casetitle = (_mutualBankContext.Cases.Where(x => x.CaseUserId == userid && x.CaseNeedHelp==false).Select(x => x.CaseTitle)).ToList();
			var caseid = (_mutualBankContext.Cases.Where(x => x.CaseUserId == userid && x.CaseNeedHelp == false).Select(x => x.CaseId)).ToList();
			List<string[]> msg = new List<string[]>();
			List<string[]> msgUsername = new List<string[]>();
			List<string> msgname = new List<string>();
			foreach (int a in caseid)
			{
				var msgContent = _mutualBankContext.Messages.Where(x => x.MsgCaseId == a).Select(x => x.MsgContent).ToArray();
				var msgUserid = (_mutualBankContext.Messages.Where(x => x.MsgCaseId == a).Select(x => x.MsgUserId)).ToList();
				foreach (int b in msgUserid)
				{
					var Username = (_mutualBankContext.Users.Where(x => x.UserId == b).Select(x => x.UserNname)).FirstOrDefault();
					msgname.Add(Username);
				}
				msg.Add(msgContent);
				msgUsername.Add(msgname.ToArray());
				msgname.Clear();
			}
			CaseTitle helpyou = new CaseTitle();
			helpyou.casetitle = casetitle;
			helpyou.caseid = caseid;
			//helpyou.msgname = msgUsername;
			//helpyou.casemsg = msg;
			return helpyou;
		}
		[HttpGet]
		public List<string> message(int id)
		{
			List<string> f = new List<string>();
			var msg = _mutualBankContext.Messages.Where(x=>x.MsgCaseId == id).Select(x => x.MsgContent).ToArray();
			var msguser = (_mutualBankContext.Messages.Where(x => x.MsgCaseId == id).Select(x => x.MsgUserId)).ToArray();
			for (int i = 0; i < msg.Length; i++) 
			{
				var username = (_mutualBankContext.Users.Where(x => x.UserId == msguser[i]).Select(x => x.UserNname)).FirstOrDefault();
                f.Add($"{username}說: {msg[i]}");
			}
			return f;
		}

		[HttpPost]
		public ActionResult<Error> ConfirmRegister(UserRegister userregister)
		{
			Error err = new Error();
			var user = _mutualBankContext.Logins.FirstOrDefault(u => u.LoginName == userregister.LoginName);
			var email = _mutualBankContext.Logins.FirstOrDefault(e => e.LoginEmail == userregister.LoginEmail);
			//檢查欄位是否都有輸入
			if (string.IsNullOrEmpty(userregister.LoginName) || string.IsNullOrEmpty(userregister.ConfirmPwd) || string.IsNullOrEmpty(userregister.LoginPwd) || string.IsNullOrEmpty(userregister.LoginEmail))
			{
				err.Message = "有誤";
				err.AccMessage = "帳號不能為空";
				err.ConfMessage = "確認密碼不能為空";
				err.EmailMessage = "信箱不能為空";
				err.PwdMessage = "密碼不能為空";
				return err;
			}
			else if (user != null)
			{
				err.AccMessage = "此帳號已有人使用";
				err.Message = "有誤";
				return err;
			}
			else if (userregister.LoginPwd != userregister.ConfirmPwd)
			{
				err.ConfMessage = "密碼與確認密碼不一致";
				err.Message = "有誤";
				return err;
			}
			else if (email != null)
			{
				err.EmailMessage = "此組信箱已被註冊";
				err.Message = "有誤";
				return err;
			}
			return err;
		}

		[HttpPost]
		public ActionResult<Error> ConfirmLogin(UserLogin userlogin)
		{
			Error err = new Error();
			var user = (from a in _mutualBankContext.Logins
						where a.LoginName == userlogin.LoginName
						&& a.LoginPwd == userlogin.LoginPwd
						select a).SingleOrDefault();
			//檢查欄位是否都有輸入
			if (string.IsNullOrEmpty(userlogin.LoginName) || string.IsNullOrEmpty(userlogin.LoginPwd))
			{
				err.Message = "有誤";
				err.AccMessage = "帳號不能為空";
				err.PwdMessage = "密碼不能為空";
				return err;
			}
			else if (user == null)
			{
				err.Message = "有誤";
				err.AccMessage = "帳號或密碼錯誤";
				err.PwdMessage = "帳號或密碼錯誤";
				return err;
			}
			return err;
		}

		[HttpPost]
		public ActionResult<Error> Confirmchangepwd(Userchangepwd userchangepwd)
		{
			Error err = new Error();
			if (string.IsNullOrEmpty(userchangepwd.ConfirmPwd) || string.IsNullOrEmpty(userchangepwd.LoginPwd))
			{
				err.Message = "有誤";
				err.ConfMessage = "帳號不能為空";
				err.PwdMessage = "密碼不能為空";
				return err;
			}
			else if (userchangepwd.ConfirmPwd != userchangepwd.LoginPwd)
			{
				err.Message = "有誤";
				err.ConfMessage = "確認密碼與密碼不一致";
				return err;
			}
			return err;
		}

		public ActionResult<Error> SendMailToken(string? id)
		{
			Error error = new Error();
			// 檢查資料庫是否有這個帳號
			var user = _mutualBankContext.Logins.FirstOrDefault(u => u.LoginName == id);
			if (user !=null)
				{
				// 取出會員信箱
				string UserEmail = user.LoginEmail;

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

					// 信件內容
					string mailContent = "請點擊以下連結，返回網站重新設定密碼，逾期 30 分鐘後，此連結將會失效。<br><br>";
					//mailContent = $"{mailContent}<a href={webPath}{receivePage}?verify={sVerify}target=_blank>點此連結</a>";
					mailContent = $"{mailContent}<a href={webPath}{receivePage}?verify={sVerify}>點此連結</a>";


				// 信件主題
				string mailSubject = "[MutualBank]密碼變更請求";

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
				return error;
			}
			else
			{
				error.Message = "請輸入您的帳號";
				return error;
			}
			
		}




		public ActionResult<Error> DoResetPwd(Userchangepwd userpwd)
		{
			Error err =  new Error();

			 var userID = HttpContext.Session.GetString("ResetPwdUserId");
			// 檢查帳號 Session 是否存在
			if (userID == null || userID.ToString() == "")
			{
				err.Message = "有誤";
				err.NoAccount = "查無此帳號";
				return err;
			}
			else if (string.IsNullOrEmpty(userpwd.ConfirmPwd) || string.IsNullOrEmpty(userpwd.LoginPwd))
			{
				err.Message = "有誤";
				err.PwdMessage = "密碼不能為空";
				err.ConfMessage = "確認密碼不能為空";
				return err;
			}
			else if (userpwd.ConfirmPwd != userpwd.LoginPwd)
			{
				err.Message = "有誤";
				err.ConfMessage = "密碼與確認密碼不相符";
				return err;
			}
			else
			{
				var user = _mutualBankContext.Logins.FirstOrDefault(u => u.LoginName == userID);
				if (user != null)
				{
					user.LoginPwd = userpwd.LoginPwd;
					_mutualBankContext.SaveChanges();
					return err;
				}
				else
				{
					err.Message = "有誤";
					err.NoAccount = "查無此帳號";
					return err;
				}
			}
			
		}
	}
}
