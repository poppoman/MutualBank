using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualBank.Areas.Admin.Models;
using MutualBank.Areas.Admin.Models.ViewModel;
using MutualBank.Models;

namespace MutualBank.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiLoginController : Controller
    {
        private readonly MutualBankContext _context;

        public ApiLoginController (MutualBankContext context)
        {
            _context = context;
        }

        //GET: api/ApiLogin/getAll
        [HttpGet]
        [Route("getAll")]
        public List<Login> getLoginAll()
        {
            List<Login> logins = _context.Logins.ToList();
            ViewBag.LoginAll = logins;
            return logins;
        }

        //Get: api/ApiLogin/getaLogin/{id}
        //getaLogin by id(from Route)
        [HttpGet]
        [Route("getaLogin/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Login), 200)]
        [ProducesResponseType(typeof(ApiMsg), 400)]
        public IActionResult getaLogin([FromRoute(Name = "id")] int id)
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
            var query = _context.Logins.Where(L => L.LoginId == id).Select(l => new LoginApiModel
            {
                loginId = l.LoginId,
                loginEmail = l.LoginEmail,
                loginName = l.LoginName,
                loginPwd = l.LoginPwd
            }).FirstOrDefault();

            if (query != null)
            {
                return this.StatusCode(200, query);
            }
            return this.StatusCode(400, error);
        }

        //Post: json 物件進來，POST 方式，根據表單name欄位 serialize 成 json (string)
        [HttpPost]
        [Route("updateLogin/{Id}")]
        [Produces("application/json")]
        public IActionResult updateLogin([FromRoute(Name = "Id")] int userId, [FromForm] LoginApiModel jsonLogin)
        {
            Login loginModel = _context.Logins.Where(l => l.LoginId == userId).FirstOrDefault();
            if (!LoginExists(userId) || loginModel == null)
            {
                return NotFound();
            }
            try
            {
                _context.Logins.Update(CorrespondTheValue(loginModel, jsonLogin));
                _context.SaveChanges();
                return Ok(loginModel);
            }
            catch (DbUpdateException ex)
            {
                return NotFound(ex);
            }
        }

        private bool LoginExists(int id)
        {
            return (_context.Logins?.Any(e => e.LoginId == id)).GetValueOrDefault();
        }
        private Login CorrespondTheValue(Login login, LoginApiModel apiModel)
        {
            login.LoginName = apiModel.loginName;
            login.LoginPwd = apiModel.loginPwd;
            login.LoginEmail = apiModel.loginEmail;
            return login;
        }
    }
}
