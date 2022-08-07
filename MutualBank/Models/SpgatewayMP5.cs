﻿namespace MutualBank.Models
{
    public class SpgatewayMP5
    {
        /// <summary>
        /// 商店代號
        /// </summary>
        public string MerchantID { get; set; }

        /// <summary>
        /// 交易資料AES 加密
        /// </summary>
        public string TradeInfo { get; set; }

        /// <summary>
        /// 交易資料SHA256 加密
        /// </summary>
        public string TradeSha { get; set; }

        /// <summary>
        /// 串接程式版本
        /// </summary>
        public string Version { get; set; }
    }
}
