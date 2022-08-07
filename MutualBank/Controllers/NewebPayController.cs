using Microsoft.AspNetCore.Mvc;
using MutualBank.Extensions;
using MutualBank.Models;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace MutualBank.Controllers
{
    public class NewebPayController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly MutualBankContext _mutualBankContext;
        public NewebPayController(IConfiguration configuration , MutualBankContext mutualBankContext)
        {
            _configuration = configuration;
            _mutualBankContext = mutualBankContext;
        }

        [HttpPost]
        public IActionResult SpgatewayPayBill(SendToNewebPayIn inModel)
        {
            var userid = this.User.GetId();
            DateTimeOffset taipeiStandardTimeOffset = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0));
            var userEmail = _mutualBankContext.Users.Where(x => x.UserId == userid).Select(x => x.UserEmail).FirstOrDefault();
            var HashKey = _configuration.GetValue<string>("NewebPay:HashKey");
            var HashIV = _configuration.GetValue<string>("NewebPay:HashIV");
            TradeInfo tradeInfo = new TradeInfo()
            {
                MerchantID = _configuration.GetValue<string>("NewebPay:MerchantID"),
                RespondType = "String",
                TimeStamp = taipeiStandardTimeOffset.ToUnixTimeSeconds().ToString(),
                Version = _configuration.GetValue<string>("NewebPay:Version"),
                MerchantOrderNo = $"U{userid}T{DateTime.Now.ToString("yyyyMMddHHmm")}",
                Amt = inModel.Amt,
                ItemDesc = $"MutualBank點數{inModel.Amt}點",
                ExpireDate = null,
                ReturnURL = _configuration.GetValue<string>("NewebPay:ReturnURL"),
                NotifyURL = _configuration.GetValue<string>("NewebPay:NotifyURL"),
                CustomerURL = _configuration.GetValue<string>("NewebPay:CustomerURL"),
                ClientBackURL = null,
                Email = null,
                EmailModify = 0,
                OrderComment = "僅限於MutualBank內交易的點數",
                CREDIT = 1,            
            };
            var tradeQueryPara = string.Join("&", tradeInfo.ToKvpList<TradeInfo>().Select(x => $"{x.Key}={x.Value}"));
            var TradeInfo = CryptoUtil.EncryptAESHex(tradeQueryPara, HashKey,HashIV);
            var TradeSha = CryptoUtil.EncryptSHA256($"HashKey={HashKey}&{TradeInfo}&HashIV={HashIV}");

            SpgatewayMP5 spgatewayMP5 = new SpgatewayMP5() 
            {
                MerchantID = _configuration.GetValue<string>("NewebPay:MerchantID"),
                TradeInfo = TradeInfo,
                TradeSha = TradeSha,
                Version = _configuration.GetValue<string>("NewebPay:Version"),
            };
            return Json(spgatewayMP5);
        }

        [HttpPost]
        public IActionResult CallbackReturn()
        {
            var HashKey = _configuration.GetValue<string>("NewebPay:HashKey");
            var HashIV = _configuration.GetValue<string>("NewebPay:HashIV");
            string TradeInfoDecrypt = CryptoUtil.DecryptAESHex(Request.Form["TradeInfo"], HashKey,HashIV);
            NameValueCollection decryptTradeCollection = HttpUtility.ParseQueryString(TradeInfoDecrypt);
            var Status = decryptTradeCollection["Status"];
            var Amt = decryptTradeCollection["Amt"];
            var OrderID = decryptTradeCollection["MerchantOrderNo"];
            var TradeNo = decryptTradeCollection["TradeNo"];
            var PayTime = decryptTradeCollection["PayTime"];
            HttpContext.Session.SetString("OrderNo", OrderID);
            if (Status != "SUCCESS")
            {
                return View();
            }
            var userid = Convert.ToInt32(OrderID.Split('T')[0].Split('U')[1]);
            var userPoint = _mutualBankContext.Users.FirstOrDefault(x => x.UserId == userid);
            userPoint.UserPoint+= Convert.ToInt32(Amt);
            _mutualBankContext.SaveChanges();
            return Redirect("/Home/Index");
        }
    }
}
