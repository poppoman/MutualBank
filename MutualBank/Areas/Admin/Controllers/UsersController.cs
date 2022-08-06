using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MutualBank.Areas.Admin.Models.ViewModel;
using MutualBank.Models;

namespace MutualBank.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class UsersController : Controller
    {
        private readonly MutualBankContext _context;
        

        public UsersController(MutualBankContext context)
        {
            _context = context;
        }

        // GET: Admin/Users/Index
        [HttpGet]
        [Route("Index")]
        [Produces("application/json")]
        public async Task<IActionResult> Index()
        {
            var query = _context.Users.Include(UserNav => UserNav.User1).Select(u => new UserLogin
            {
                LoginName = u.User1.LoginName,
                LoginEmail = u.User1.LoginEmail,
                LoginAddDate = u.User1.LoginAddDate,
                UserId = u.UserId,
                UserFullname = u.UserFname +" "+ u.UserLname,
                UserNname = u.UserNname,
                UserEmail = u.UserEmail,
                UserSex = u.UserSex,
                UserHphoto = u.UserHphoto,
                UserBirthday = u.UserBirthday,
                UserSkillId = u.UserSkillId,
                UserAreaId = u.UserAreaId,
                UserCV = u.UserCv,
                UserResume = u.UserResume,
                UserSchool = u.UserSchool,
                UserFaculty = u.UserFaculty,
                UserPoint = u.UserPoint,
            });
            ViewBag.UserLogin = query;
            return View(query);
        }

        // GET: Admin/Users/getaUser/5
        [HttpGet]
        [Route("getaUser/{id}")]
        public async Task<IActionResult> getaUser([FromRoute(Name ="id")]int? id)
        {
            if (id == null || _context.Users == null || _context.Logins == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            var login = await _context.Logins.FindAsync(id);
            if (user == null || login == null)
            {
                return NotFound();
            }            
            ViewBag.UserId = id;
            ViewBag.aUser = user.UserFname + " " + user.UserLname ?? "null";
            return View();
        }

        //Post: getaUser/{Id}
        //json 物件進來，POST 方式，根據表單name欄位 serialize 成 json (string)
        [HttpPost]
        [Route("getaUser/{id}")]
        [Produces("application/json")]
        public IActionResult getaUser(int id, [FromForm]ApiUserLoginModel json)
        {
            var userModel = _context.Users.Where(u => u.UserId == id).FirstOrDefault();
            var loginModel = _context.Logins.Where(l => l.LoginId == id).FirstOrDefault();
            if (!UserExists(id) || !LoginExists(id) || userModel == null || loginModel == null)
            {
                return NotFound();
            }
            try
            {
                _context.Users.Update(CorrespondTheValue(userModel, json));
                _context.Logins.Update(CorrespondTheValue(loginModel, json));
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                return NotFound(ex);
            }
            return RedirectToAction(nameof(Index));
        }


        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
        private bool LoginExists(int id)
        {
            return (_context.Logins?.Any(e => e.LoginId == id)).GetValueOrDefault();
        }
        private UserApiModel CorrespondTheValue(User user)
        {
            return new UserApiModel
            {
                userId = user.UserId,
                userAreaId = user.UserAreaId,
                userBirthday = user.UserBirthday,
                userEmail = user.UserEmail,
                userCv = user.UserCv,
                userFaculty = user.UserFaculty,
                userFname = user.UserFname,
                userLname = user.UserLname,
                userNname = user.UserNname,
                userPoint = user.UserPoint,
                userResume = user.UserResume,
                userSchool = user.UserSchool,
                userSex = user.UserSex,
                userSkillId = user.UserSkillId
            };
        }
        private LoginApiModel CorrespondTheValue(Login login)
        {
            return new LoginApiModel
            {
                loginId = login.LoginId,
                loginEmail = login.LoginEmail,
                loginName = login.LoginName,
                loginPwd = login.LoginPwd,
            };
        }

        private User CorrespondTheValue(User user, UserApiModel apiModel)
        {
            user.UserAreaId = apiModel.userAreaId;
            user.UserBirthday = apiModel.userBirthday;
            user.UserEmail = apiModel.userEmail;
            user.UserCv = apiModel.userCv;
            user.UserFaculty = apiModel.userFaculty;
            user.UserFname = apiModel.userFname;
            user.UserLname = apiModel.userLname;
            user.UserNname = apiModel.userNname;
            user.UserPoint = apiModel.userPoint;
            user.UserResume = apiModel.userResume;
            user.UserSchool = apiModel.userSchool;
            user.UserSex = apiModel.userSex;
            user.UserSkillId = apiModel.userSkillId;
            return user;
        }
        private User CorrespondTheValue(User user, ApiUserLoginModel apiModel)
        {
            user.UserAreaId = apiModel.userAreaId;
            user.UserBirthday = apiModel.userBirthday;
            user.UserEmail = apiModel.userEmail;
            user.UserCv = apiModel.userCv;
            user.UserFaculty = apiModel.userFaculty;
            user.UserFname = apiModel.userFname;
            user.UserLname = apiModel.userLname;
            user.UserNname = apiModel.userNname;
            user.UserPoint = apiModel.userPoint;
            user.UserResume = apiModel.userResume;
            user.UserSchool = apiModel.userSchool;
            user.UserSex = apiModel.userSex;
            user.UserSkillId = apiModel.userSkillId;
            return user;
        }
        private Login CorrespondTheValue(Login login, ApiUserLoginModel apiModel)
        {
            login.LoginName = apiModel.loginName;
            return login;
        }
    }
}
