using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MutualBank.Extensions;
using MutualBank.Models;

namespace MutualBank.Controllers
{
    [Route("Nav/[controller]/[action]")]
    [ApiController]
    public class _LayoutApiController : ControllerBase
    {
        private readonly MutualBankContext _mutualBankContext;
        public _LayoutApiController(MutualBankContext mutualBankContext) 
        {
            _mutualBankContext = mutualBankContext;
        }
        [HttpGet]
        public IQueryable<string> GetAreaCity()
        {
            var AreaCity = _mutualBankContext.Areas.Select(x => x.AreaCity).Distinct();
            return AreaCity;
        }
        [HttpGet]
        public List<String> GetAreaTown(string AreaCity)
        {
            var AreaTown = _mutualBankContext.Areas.Where(x => x.AreaCity == AreaCity).Select(x=>x.AreaTown).ToList();
            return AreaTown;
        }

        [HttpGet]
        public String GetAreaTownWithId(string AreaCity)
        {
            var AreaTown= _mutualBankContext.Areas.Where(x => x.AreaCity == AreaCity)
                .Select(x =>new {
                    AreaId=x.AreaId,
                    AreaTown=x.AreaTown
                })
                .ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(AreaTown);
        }
        public string GetUserPoint()
        {
            var points = "null";
            if (User.Identity.IsAuthenticated)
            {
                var UserId = this.User.GetId();
                points = Convert.ToString(_mutualBankContext.Users.Where(x => x.UserId == UserId).Select(x => x.UserPoint).FirstOrDefault());
            }
            return points;
        }
    }
}
