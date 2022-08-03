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
        public async Task<IActionResult> Index()
        {
            var query = _context.Users.Include(UserNav => UserNav.User1).Select(u => new UserLogin
            {
                LoginId = u.User1.LoginId,
                LoginName = u.User1.LoginName,
                LoginPwd = u.User1.LoginPwd,
                LoginEmail = u.User1.LoginEmail,
                LoginLevel = u.User1.LoginLevel,
                LoginAddDate = u.User1.LoginAddDate,
                LoginActive = u.User1.LoginActive,
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

            return View(query);

        }

        // GET: Admin/Users/getaUser/5
        [HttpGet]
        [Route("getaUser/{id}")]        
        public async Task<IActionResult> getaUser([FromRoute(Name ="id")]int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var LoginQuery = _context.Logins.Where(l => l.LoginId == user.UserId);
            var result = (_context.Users.Where(u => u.UserId == id).FirstOrDefault() as User);
            ViewBag.aUser = CorrespondTheValue(result);
            ViewBag.UserId = id;
            return View(result);
        }

        [HttpPost]
        [Route("getaUser/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult getaUser([FromRoute(Name = "id")]int userId)
        {
            return View();
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute(Name ="id")]int id, [Bind("UserId,UserLname,UserFname,UserNname,UserSex,UserHphoto,UserEmail,UserBirthday,UserSkillId,UserAreaId,UserCv,UserResume,UserSchool,UserFaculty,UserPoint")] User user, [Bind("LoginId,LoginName,LoginPwd,LoginEmail,LoginLevel,LoginAddDate,LoginActive")] Login login)
        {
            if (id != user.UserId || id != login.LoginId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    _context.Update(login);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId) || !LoginExists(login.LoginId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
               

            }
            var query = _context.Users.Include(UserNav => UserNav.User1).Select(u => new UserLogin
            {
                LoginId = u.User1.LoginId,
                LoginName = u.User1.LoginName,
                LoginPwd = u.User1.LoginPwd,
                LoginEmail = u.User1.LoginEmail,
                LoginLevel = u.User1.LoginLevel,
                LoginAddDate = u.User1.LoginAddDate,
                LoginActive = u.User1.LoginActive,
                UserId = u.UserId,
                UserLname = u.UserLname,
                UserFname = u.UserFname,
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
            return View(query);
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
    }
}
