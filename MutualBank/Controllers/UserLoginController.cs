using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MutualBank.Models;
using MutualBank.Models.ViewModels;
using System.Security.Claims;

namespace MutualBank.Controllers
{
    public class UserLoginController : Controller
    {
        private MutualBankContext _mutualBankContext;

        public UserLoginController(MutualBankContext mutualBankContext)
        {
            _mutualBankContext = mutualBankContext;
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
            if (ModelState.IsValid && user == null)
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
        #region 忘記密碼
        [HttpPost]
        public IActionResult forgetPassword(string Password)
        {
            var update = _mutualBankContext.Logins.Find(User.Identity.Name);
            if (Password != null)
            {
                update.LoginPwd = Password;
                _mutualBankContext.SaveChanges();
                return RedirectToAction("Login", "UserLogin");
            }
            else
            {
                TempData["ChangePassword"] = "請填入在按確認!";
                return RedirectToAction("Index", "Home");
            }


        }
        #endregion
    }
}
