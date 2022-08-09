using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualBank.Extensions;
using MutualBank.Models;
using MutualBank.Models.ViewModels;

namespace MutualBank.Controllers
{
    [Route("api/UsersController/{action}")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MutualBankContext _context;
        private static string _filePath = Path.Combine("/Img", "User");

        public UsersController(MutualBankContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public UserViewModel GetUsers()
        {
            var a = _context.Users.Include("UserNavigation").Where(s => s.UserId == User.GetId())
                .FirstOrDefault();
            var area = _context.Areas.FirstOrDefault(x => x.AreaId == a.UserAreaId);
            UserViewModel v = new UserViewModel
            {
                UserId = a.UserId,
                UserLname = a.UserLname,
                UserFname = a.UserFname,
                UserNname = a.UserNname,
                UserSex = a.UserSex == true ? "男" : "女",
                UserEmail = a.UserEmail,
                UserBirthday = a.UserBirthday.Value.ToShortDateString(),
                UserAreaId = a.UserAreaId,
                UserCv = a.UserCv,
                UserResume = a.UserResume,
                UserArea = $"{area.AreaCity}{area.AreaTown}",
                UserHphoto = a.UserHphoto == null ? a.UserSex == true ? Path.Combine(_filePath, "Male.PNG") : Path.Combine(_filePath, "Female.PNG") : Path.Combine(_filePath, a.UserHphoto)
            };
            return v;
        }

        [HttpPost]
        public ActionResult<Error> UpdateUsers(Update UpdateU)
        {
            Error err = new Error();
            int id = 11 ;
            var upd = _context.Users.FirstOrDefault(u => u.UserId == id);
            upd.UserLname = UpdateU.UserLname;
            upd.UserFname = UpdateU.UserFname;
            upd.UserNname = UpdateU.UserNname;
            upd.UserSex =  UpdateU.UserSex == "男" ? true: false   ;
            upd.UserBirthday = Convert.ToDateTime(UpdateU.UserBirthday);  //UpdateU.UserBirthday; /*
            upd.UserAreaId = UpdateU.UserAreaId;
            upd.UserCv = UpdateU.UserCv;
            upd.UserResume = UpdateU.UserResume;

            _context.SaveChanges();
            err.Message = "OK";
            return err;
        }



        // GET: api/Users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(int id)
        //{
        //  if (_context.Users == null)
        //  {
        //      return NotFound();
        //  }
        //    var user = await _context.Users.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id, User user)
        //{
        //    if (id != user.UserId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Users
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //  if (_context.Users == null)
        //  {
        //      return Problem("Entity set 'MutualBankContext.Users'  is null.");
        //  }
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        //}

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    if (_context.Users == null)
        //    {
        //        return NotFound();
        //    }
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool UserExists(int id)
        //{
        //    return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        //}
    }
}
