using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        //縣市
        [HttpGet]
        public IQueryable<string> GetAreaCity()
        {

            var AreaCity = _mutualBankContext.Areas.Select(x => x.AreaCity).Distinct();
            return AreaCity;
        }
        [HttpGet]
        public IQueryable<Area> GetAreaTown(string AreaCity)
        {
            var AreaTown = _mutualBankContext.Areas.Where(x => x.AreaCity == AreaCity);
            return AreaTown;
        }

    }
}
