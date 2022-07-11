using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MutualBank.Models;

namespace MutualBank.Controllers.api
{
    [Route("api/Confirm/{action}")]
    [ApiController]
    public class ConfirmController : ControllerBase
    {
        private readonly MutualBankContext _mutualBankContext;

        public ConfirmController(MutualBankContext mutualBankContext)
        {
            _mutualBankContext = mutualBankContext;
        }

        [HttpGet]
        public async Task<string> ConfirmAccount(string id)
        {
            var user = _mutualBankContext.Logins.FirstOrDefault(u => u.LoginName == id);
            if (user == null) return "OK";
            else return "有人使用";
        }
    }
}
