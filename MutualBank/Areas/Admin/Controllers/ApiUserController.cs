using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualBank.Areas.Admin.Models;
using MutualBank.Areas.Admin.Models.ViewModel;
using MutualBank.Models;

namespace MutualBank.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUserController : Controller
    {
        private readonly MutualBankContext _context;

        public ApiUserController(MutualBankContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("getUserAll")]
        public List<User> getUserAll()
        {
            List<User> users = _context.Users.ToList();
            ViewBag.UserAll = users;
            return users;
        }

        [HttpGet]
        [Route("getLoginAll")]
        public List<Login> getLoginAll()
        {
            List<Login> logins = _context.Logins.ToList();
            ViewBag.LoginAll = logins;
            return logins;
        }

        [HttpGet]
        [Route("getaUser/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(ApiMsg),400)]
        public IActionResult getaUser([FromRoute(Name ="id")]int id)
        {
            var error = new ApiMsg
            {
                code = 400,
                msg = "查無此筆資料"
            };
            var query = _context.Users.Where(u => u.UserId == id).Select(u => new UserApiModel
            {
                userId = id,
                userAreaId = u.UserAreaId,
                userBirthday = u.UserBirthday,
                userCv = u.UserCv,
                userEmail = u.UserEmail,
                userFaculty = u.UserFaculty,
                userFname = u.UserFname,
                userLname = u.UserLname,
                userNname = u.UserNname,
                userPoint = u.UserPoint,
                userResume = u.UserResume,
                userSchool = u.UserSchool,
                userSex = u.UserSex,
                userSkillId = u.UserSkillId,                
            }).FirstOrDefault();

            if (query != null)
            {
                ViewBag.aUser = query;
                return this.StatusCode(200, query);
            }
            else return this.StatusCode(400, error);
        }

        [HttpGet]
        [Route("getaLogin/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Login), 200)]
        [ProducesResponseType(typeof(ApiMsg), 400)]
        public IActionResult getaLogin(int? id)
        {
            var error = new ApiMsg
            {
                code = 400,
                msg = "查無此筆資料"
            };
            if (_context.Logins == null)
            {
                return this.StatusCode(400, error);
            }
            else
            {
                var query = _context.Logins.Where(L => L.LoginId == id).FirstOrDefault();

                if (query == null || query.ToString() == "")
                {
                    return this.StatusCode(400, error);
                }
                else
                {
                    ViewBag.aLogin = query;
                    return this.StatusCode(200, query);
                }
            }
        }

        [HttpPost]
        [Route("updateaUser/{Id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult updateUser([FromRoute(Name ="Id")]int userId,[FromBody]UserApiModel jsonUser)
        {
            var userModel = _context.Users.Where(u => u.UserId == userId).FirstOrDefault();

            if (!UserExists(userId) || userModel == null)
            {
                return NotFound();
            }
            ;
            try
            {
                _context.Users.Update(CorrespondTheValue(userModel, jsonUser));
                _context.SaveChanges();
                return Ok(userModel);
            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }
        }

        //物件進來，POST 方式，根據表單 name
        [HttpPost]
        [Route("updateaUser2/{Id}")]
        [Produces("application/json")]
        public IActionResult updateUser2([FromRoute(Name = "Id")] int userId, [FromForm]UserApiModel jsonUser)
        {
            var userModel = _context.Users.Where(u => u.UserId == userId).FirstOrDefault();
            if (!UserExists(userId) || userModel == null)
            {
                return NotFound();
            }
            ;
            try
            {
                _context.Users.Update(CorrespondTheValue(userModel, jsonUser));
                _context.SaveChanges();
                return Ok(userModel);
            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
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
    }
}
