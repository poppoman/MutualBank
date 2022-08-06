using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using MutualBank.Extensions;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserLoginController(MutualBankContext mutualBankContext, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _mutualBankContext = mutualBankContext;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
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
                _mutualBankContext.Logins.Add(newuser);
                _mutualBankContext.SaveChanges();
                var user2 = _mutualBankContext.Logins.Where(u => u.LoginName == usergister.LoginName).Select(u => u.LoginId).FirstOrDefault();
                var newuser2 = new User
                {
                    UserEmail = usergister.LoginEmail,
                    UserNname = usergister.LoginName,
                    UserId = user2
                };
                    _mutualBankContext.Users.Add(newuser2);
                    _mutualBankContext.SaveChanges();
                    return RedirectToAction("Login", "UserLogin");
            }
            else
            {
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
            if (userlogin.LoginName == "Admin" && userlogin.LoginPwd == "Admin") { return Redirect("~/Admin/Home"); }
            else 
            {
                var user = (from a in _mutualBankContext.Logins
                            where a.LoginName == userlogin.LoginName
                            && a.LoginPwd == userlogin.LoginPwd
                            select a).SingleOrDefault();
                if (user == null)
                {
                    return View();
                }
                else
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.LoginName),
                    new Claim("UserId",user.LoginId.ToString())
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
        public IActionResult changePassword(Userchangepwd Password)
        {
            string user = User.Identity.Name;
            var update = _mutualBankContext.Logins.FirstOrDefault(u => u.LoginName == User.Identity.Name);
            if (Password != null)
            {
                update.LoginPwd = Password.LoginPwd;
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
        #region 忘記密碼
        public IActionResult forgetPassword()
        {
            return View();
        }
        public IActionResult resetPassword(string verify)
        {
            // 由信件連結回來會帶參數 verify

            if (verify == "")
            {
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
        #endregion
        #region OAuth
        public IActionResult FacebookLogin()
        {
            var auth = new AuthenticationProperties()
            {
                RedirectUri = "/UserLogin/FacebookResponse"
            };
            return Challenge(auth, FacebookDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> FacebookResponse()
        {
            var data = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            
            return RedirectToAction("Index", "Home");
        }
        public IActionResult GoogleLogin()
        {
            var auth = new AuthenticationProperties()
            {
                RedirectUri = "/UserLogin/GoogleResponse"
            };
            return Challenge(auth, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var data = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        public string UpdateMember(MemberUpdate memberUpdate)
        {
            var userid = this.User.GetId();
            var user = _mutualBankContext.Users.Where(x => x.UserId == userid).FirstOrDefault();
            user.UserLname = memberUpdate.UserLname;
            user.UserFname = memberUpdate.UserFname;
            user.UserNname = memberUpdate.UserNname;
            user.UserSex = memberUpdate.UserSex;
            user.UserSkillId = memberUpdate.UserSkillId;
            user.UserBirthday = Convert.ToDateTime(memberUpdate.UserBirthday);
            user.UserCv = memberUpdate.UserCv;
            user.UserSchool = memberUpdate.UserSchool;
            user.UserResume = memberUpdate.UserResume;
            var areaid = _mutualBankContext.Areas.FirstOrDefault(x => x.AreaCity == memberUpdate.City && x.AreaTown == memberUpdate.Town);
            if (areaid == null)
            {
                return "地區有誤";
            }
            else user.UserAreaId = areaid.AreaId;
            if (HttpContext.Request.Form.Files.Count == 0)
            {
                var userHphoto = _mutualBankContext.Users.Where(_x => _x.UserId == userid).Select(x => x.UserHphoto).FirstOrDefault();
                if (userHphoto == null)
                {
                    if (memberUpdate.UserSex == true)
                {
                    memberUpdate.UserPhoto = "Male.PNG";
                }
                else
                {
                    memberUpdate.UserPhoto = "Female.PNG";
                }
                }
                else memberUpdate.UserPhoto = userHphoto;
                
            }
            else
            {
                IFormFile InputFile = HttpContext.Request.Form.Files[0];
                IFormFile InputFile2 = HttpContext.Request.Form.Files[2];
                var UniqueId = Guid.NewGuid().ToString("D");
                var PhotoFormat = InputFile.FileName.Split(".")[1];
                memberUpdate.UserPhoto = $"{userid}_{UniqueId}.{PhotoFormat}";
                var InputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Img", "User", memberUpdate.UserPhoto);
                FileStream fs = new FileStream(InputFilePath, FileMode.Create);
                InputFile2.CopyToAsync(fs);
                fs.Close();
            }
            user.UserHphoto = memberUpdate.UserPhoto;
            _mutualBankContext.SaveChanges();
            return "OK";
        }

    }
}
