using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ThinkPower.CCLPA.Domain.Entity;
using ThinkPower.CCLPA.Domain.VO;
using ThinkPower.CCLPA.Web.Models;

namespace ThinkPower.CCLPA.Web.ViewModels
{
    /// <summary>
    /// 專案臨調檢視模型類別
    /// </summary>
    public class AdjustProcessViewModel
    {
        #region DropDownList

        /// <summary>
        /// 調高原因選項清單
        /// </summary>
        public IEnumerable<SelectListItem> AdjustReasonSelectListItem { get; set; }
        /// <summary>
        /// 使用地點選項清單
        /// </summary>
        public IEnumerable<SelectListItem> UseLocationSelectListItem { get; set; }
        /// <summary>
        /// 人工授權選項清單
        /// </summary>
        public IEnumerable<SelectListItem> ManualAuthorizationSelectListItem { get; set; }

        #endregion



        #region InputField

        /// <summary>
        /// 歸戶ID
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// 調高原因備註
        /// </summary>
        public string AdjustReasonRemark { get; set; }
        /// <summary>
        /// 刷卡金額(不含額度)
        /// </summary>
        public string SwipeAmount { get; set; }
        /// <summary>
        /// 臨調後額度
        /// </summary>
        public string AfterAdjustAmount { get; set; }
        /// <summary>
        /// 有效日期(起)
        /// </summary>
        public string ValidDateStart { get; set; }
        /// <summary>
        /// 有效日期(迄)
        /// </summary>
        public string ValidDateEnd { get; set; }
        /// <summary>
        /// 出國地點
        /// </summary>
        public string PlaceOfGoingAbroad { get; set; }
        /// <summary>
        /// 轉授信主管原因
        /// </summary>
        public string TransferSupervisorReason { get; set; }

        #endregion



        #region DisplayData

        /// <summary>
        /// 調額上限
        /// </summary>
        public string AdjustmentAmountCeiling { get; set; }

        /// <summary>
        /// JCIC日期
        /// </summary>
        public string JcicQueryDate { get; set; }
        /// <summary>
        /// 專案臨調結果
        /// </summary>
        public string ProjectAdjustResult { get; set; }
        /// <summary>
        /// 拒絕原因(專案)
        /// </summary>
        public string ProjectRejectReason { get; set; }
        /// <summary>
        /// 一般臨調結果
        /// </summary>
        public string GeneralAdjustResult { get; set; }
        /// <summary>
        /// 拒絕原因(一般)
        /// </summary>
        public string GeneralRejectReason { get; set; }

        /// <summary>
        /// 貴賓資訊
        /// </summary>
        public AdjustProcessVip Vip { get; set; }
        /// <summary>
        /// 歸戶資料
        /// </summary>
        public AdjustProcessCustomer Customer { get; set; }
        /// <summary>
        /// 臨調紀錄資料
        /// </summary>
        public AdjustLogModel AdjustLog { get; set; }
        /// <summary>
        /// 近年評等紀錄
        /// </summary>
        public NearlyYearRatingModel NearlyYearRating { get; set; }

        #endregion



        #region HiddenField

        /// <summary>
        /// 調整原因生效資訊
        /// </summary>
        public IEnumerable<ParamCurrentlyEffect> ReasonEffectInfoList { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }

        #endregion
    }
}