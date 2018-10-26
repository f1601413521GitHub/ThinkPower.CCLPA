﻿using System;

namespace ThinkPower.CCLPA.DataAccess.DO.ICRS
{
    /// <summary>
    /// 所得稅卡戶臨調資料物件類別
    /// </summary>
    public class IncomeTaxCardAdjustDO
    {
        /// <summary>
        /// 執行碼 A:新增 U:修改 D:刪除
        /// </summary>
        public string ActionCode { get; set; }

        /// <summary>
        /// 歸戶ID(10碼)
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 歸戶ID序號(1碼; 預設空白)
        /// </summary>
        public string CustomerIdNo { get; set; }

        /// <summary>
        /// 專案代號
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 所得稅臨調金額
        /// </summary>
        public decimal? IncomeTaxAdjustAmount { get; set; }

        /// <summary>
        /// 臨調截止日YYYYMMDD
        /// </summary>
        public string AdjustCloseDate { get; set; }

        /// <summary>
        /// 臨調人員
        /// </summary>
        public string AdjustUserId { get; set; }
    }
}