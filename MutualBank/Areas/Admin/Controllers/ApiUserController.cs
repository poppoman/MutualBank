using Microsoft.AspNetCore.Mvc;
using MutualBank.Areas.Admin.Models;
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
        public IActionResult getaUser(int id)
        {
            var error = new ApiMsg
            {
                code = 400,
                msg = "查無此筆資料"
            };
            var query = ((from u in _context.Users
                          where u.UserId == id
                          select u) as User);
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

    }
}
