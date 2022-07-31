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
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly MutualBankContext _context;
        

        public UsersController(MutualBankContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Index")]
        // GET: Admin/Users
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
                UserFullname = u.UserFname+ u.UserLname,
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

        
        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Id = id;
            return View(user);
        }

        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserLname,UserFname,UserNname,UserSex,UserHphoto,UserEmail,UserBirthday,UserSkillId,UserAreaId,UserCv,UserResume,UserSchool,UserFaculty,UserPoint")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpGet]
        [Route("Edit/{id}")]
        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit([FromRoute(Name ="id")]int? id)
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
            var result = _context.Users.Include(UserNav => UserNav.User1).Where(u => u.UserId == id).Select(u => new UserLogin
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

            return View(result);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

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

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'MutualBankContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
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
    }
}
