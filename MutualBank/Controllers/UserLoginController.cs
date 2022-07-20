using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MutualBank.Models;
using MutualBank.Models.ViewModels;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MutualBank.Controllers
{
    public class UserLoginController : Controller
    {
        private MutualBankContext _mutualBankContext;
        private IConfiguration _configuration;

        public UserLoginController(MutualBankContext mutualBankContext, IConfiguration configuration)
        {
            _mutualBankContext = mutualBankContext;
            _configuration = configuration;
        }

        #region 註冊帳戶
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegister usergister)
        {

            var user = _mutualBankContext.Logins.FirstOrDefault(u => u.LoginName == usergister.LoginName);
            if (user == null)
            {
                var newuser = new Login
                {
                    LoginName = usergister.LoginName,
                    LoginPwd = usergister.LoginPwd,
                    LoginEmail = usergister.LoginEmail
                };
                if (usergister.LoginPwd != usergister.ConfirmPwd)
                {
                    ViewBag.message = "確認密碼不一致";
                    return View();
                }
                else
                {
                    _mutualBankContext.Logins.Add(newuser);
                    _mutualBankContext.SaveChanges();
                    return RedirectToAction("Login", "UserLogin");
                }

            }
            else
            {
                ViewBag.message = "此帳號有人使用過囉";
                return View();
            }

        }
        #endregion
        #region 登入帳戶
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login userlogin)
        {
            var user = (from a in _mutualBankContext.Logins
                        where a.LoginName == userlogin.LoginName
                        && a.LoginPwd == userlogin.LoginPwd
                        select a).SingleOrDefault();
            if (user == null)
            {
                ViewBag.LoginFalse = "帳號或密碼錯誤";
                return View();
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.LoginName)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(claimsIdentity);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal), new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
                });
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
        #region 登出帳戶
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region 修改密碼

        public IActionResult changePwd()
        {
            return PartialView();
        }
        public IActionResult message()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult changePassword(string Password)
        {
            string user = User.Identity.Name;
            var update = _mutualBankContext.Logins.FirstOrDefault(u => u.LoginName == User.Identity.Name);
            if (Password != null)
            {
                update.LoginPwd = Password;
                _mutualBankContext.SaveChanges();
                return RedirectToAction("ProfilePageAjax", "Home");
            }
            else
            {
                TempData["ChangePassword"] = "請填入在按確認!";
                return RedirectToAction("Index", "Home");
            }


        }
        #endregion
        public IActionResult forgetPassword()
        {
            return View();
        }
        public IActionResult resetPassword(string verify)
        {
            // 由信件連結回來會帶參數 verify

            if (verify == "")
            {
                ViewData["ErrorMsg"] = "缺少驗證碼";
                return View();
            }

            // 取得系統自定密鑰，在 Web.config 設定

            string SecretKey = _configuration.GetValue<string>("Email:SecretKey");

            try
            {
                // 使用 3DES 解密驗證碼
                //TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
                //MD5 md5 = new MD5CryptoServiceProvider();
                //byte[] buf = Encoding.UTF8.GetBytes(SecretKey);
                //byte[] md5result = md5.ComputeHash(buf);
                //string md5Key = BitConverter.ToString(md5result).Replace("-", "").ToLower().Substring(0, 24);
                //DES.Key = UTF8Encoding.UTF8.GetBytes(md5Key);
                //DES.Mode = CipherMode.ECB;
                //DES.Padding = PaddingMode.PKCS7;
                //ICryptoTransform DESDecrypt = DES.CreateDecryptor();
                //byte[] Buffer = Convert.FromBase64String(verify);
                //string deCode = UTF8Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));

                //verify = deCode; //解密後還原資料
            }
            catch (Exception ex)
            {
                ViewData["ErrorMsg"] = "驗證碼錯誤";
                return View();
            }

            // 取出帳號
            string UserID = verify.Split('|')[0];

            // 取得重設時間
            string ResetTime = verify.Split('|')[1];

            // 檢查時間是否超過 30 分鐘
            DateTime dResetTime = Convert.ToDateTime(ResetTime);
            TimeSpan TS = new System.TimeSpan(DateTime.Now.Ticks - dResetTime.Ticks);
            double diff = Convert.ToDouble(TS.TotalMinutes);
            if (diff > 30)
            {
                ViewData["ErrorMsg"] = "超過驗證碼有效時間，請重寄驗證碼";
                return View();
            }

            // 驗證碼檢查成功，加入 Session
            HttpContext.Session.SetString("ResetPwdUserId", UserID);
            return View();
        }
    }
}
