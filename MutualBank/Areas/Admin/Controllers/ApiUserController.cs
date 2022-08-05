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

        //GET: api/ApiUser/getAll
        [HttpGet]
        [Route("getAll")]
        public List<User> getUserAll()
        {
            List<User> users = _context.Users.ToList();
            ViewBag.UserAll = users;
            return users;
        }

        //Get: getaUser/{id}
        //getaUser by id(from Route)
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
                return this.StatusCode(200, query);
            }
            return this.StatusCode(400, error);
        }


        //Post: updateaUser/rawdata/{Id}
        //id from Route, json 格式 from body
        [HttpPost]
        [Route("updateUser/rawdata/{Id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult updateUserRawdata([FromRoute(Name ="Id")]int userId,[FromBody]UserApiModel jsonUser)
        {
            var userModel = _context.Users.Where(u => u.UserId == userId).FirstOrDefault();
            if (!UserExists(userId) || userModel == null)
            {
                return NotFound();
            }
            try
            {
                _context.Users.Update(CorrespondTheValue(userModel, jsonUser));
                _context.SaveChanges();
                return Ok(userModel);
            }
            catch (DbUpdateException ex)
            {
                return NotFound(ex);
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
        private bool LoginExists(int id)
        {
            return (_context.Logins?.Any(e => e.LoginId == id)).GetValueOrDefault();
        }
        private Login CorrespondTheValue(Login login, ApiUserLoginModel apiModel)
        {
            login.LoginName = apiModel.loginName;
            login.LoginPwd = apiModel.loginPwd;
            login.LoginEmail = apiModel.loginEmail;
            return login;
        }
    }
}
