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
        public List<string> GetAreaCity()
        {

            var AreaCity = _mutualBankContext.Areas.Select(x => x.AreaCity).Distinct().ToList();
            return AreaCity;
        }
        [HttpGet]
        public List<string> GetAreaTown(string AreaCity)
        {
            var AreaTown = _mutualBankContext.Areas.Where(x => x.AreaCity == AreaCity).Select(x => x.AreaTown).ToList();
            return AreaTown;
        }


    }
}
