using System;
using ThinkPower.CCLPA.Domain.Service;

namespace ThinkPower.CCLPA.Domain.Entity
{
    /// <summary>
    /// 臨調預審名單類別
    /// </summary>
    public class PreAdjustEntity : BaseEntity
    {
        /// <summary>
        /// 行銷活動代號
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// 客戶ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 預審專案
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 預審額度
        /// </summary>
        public Nullable<decimal> ProjectAmount { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        public string CloseDate { get; set; }

        /// <summary>
        /// 匯入日期
        /// </summary>
        public string ImportDate { get; set; }

        /// <summary>
        /// 中文姓名
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// 客戶類型
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// 簡訊比對結果
        /// </summary>
        public string SmsCheckResult { get; set; }

        /// <summary>
        /// 目前狀態
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 處理日期時間
        /// </summary>
        public string ProcessingDateTime { get; set; }

        /// <summary>
        /// 處理人員
        /// </summary>
        public string ProcessingUserId { get; set; }

        /// <summary>
        /// 刪除日期時間
        /// </summary>
        public string DeleteDateTime { get; set; }

        /// <summary>
        /// 刪除人員
        /// </summary>
        public string DeleteUserId { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 關帳日
        /// </summary>
        public string ClosingDay { get; set; }

        /// <summary>
        /// 繳款期限
        /// </summary>
        public string PayDeadline { get; set; }

        /// <summary>
        /// 強制同意人員
        /// </summary>
        public string AgreeUserId { get; set; }

        /// <summary>
        /// 行動電話
        /// </summary>
        public string MobileTel { get; set; }

        /// <summary>
        /// 拒絕原因代碼
        /// </summary>
        public string RejectReasonCode { get; set; }

        /// <summary>
        /// CCAS回覆碼
        /// </summary>
        public string CcasReplyCode { get; set; }

        /// <summary>
        /// CCAS回覆結果
        /// </summary>
        public string CcasReplyStatus { get; set; }

        /// <summary>
        /// CCAS傳送回覆時間
        /// </summary>
        public string CcasReplyDateTime { get; set; }

        /// <summary>
        /// 更新資料
        /// </summary>
        internal void Update()
        {
            new PreAdjustService().Update(this);
        }
    }
}