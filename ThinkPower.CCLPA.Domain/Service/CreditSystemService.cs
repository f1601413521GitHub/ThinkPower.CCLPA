using System;
using ThinkPower.CCLPA.DataAccess.DAO.ICRS;
using ThinkPower.CCLPA.DataAccess.DO.ICRS;
using ThinkPower.CCLPA.DataAccess.VO;
using ThinkPower.CCLPA.Domain.Service.Interface;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// CCAS授信系統服務
    /// </summary>
    public class CreditSystemService : BaseService, ICreditSystem
    {
        /// <summary>
        /// ICRS查詢掛帳金額 (含已授權未清算)、可用額度
        /// </summary>
        /// <param name="data">來源資料</param>
        /// <returns></returns>
        public object QueryAmount(object data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 所得稅卡戶臨調
        /// </summary>
        /// <param name="adjustInfo">所得稅卡戶臨調資訊</param>
        /// <returns></returns>
        public string IncomeTaxCardAdjust(IncomeTaxCardAdjustInfo adjustInfo)
        {
            string responseCode = null;

            if (adjustInfo == null)
            {
                throw new ArgumentNullException("info");
            }

            IncomeTaxCardAdjust adjustInfoDO = ConvertIncomeTaxCardAdjustDO(adjustInfo);

            responseCode = new CreditSystemDAO().IncomeTaxCardAdjust(adjustInfoDO);

            return responseCode;
        }

        /// <summary>
        /// 轉換所得稅卡戶資訊
        /// </summary>
        /// <param name="info">所得稅卡戶資訊</param>
        /// <returns></returns>
        private IncomeTaxCardAdjust ConvertIncomeTaxCardAdjustDO(IncomeTaxCardAdjustInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            return new IncomeTaxCardAdjust()
            {
                ActionCode = info.ActionCode,
                CustomerId = info.CustomerId,
                CustomerIdNo = info.CustomerIdNo,
                ProjectName = info.ProjectName,
                IncomeTaxAdjustAmount = info.IncomeTaxAdjustAmount,
                AdjustCloseDate = info.AdjustCloseDate,
                AdjustUserId = info.AdjustUserId,
            };
        }

        /// <summary>
        /// 非所得稅卡戶臨調
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public object NonIncomeTaxCardAdjust(object data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 轉換所得稅卡戶臨調回傳碼
        /// </summary>
        /// <param name="incomeTaxResultCode">回傳碼</param>
        /// <returns>回傳結果</returns>
        internal string ConvertIncomeTaxResultCode(string incomeTaxResultCode)
        {
            string result = null;

            if (String.IsNullOrEmpty(incomeTaxResultCode))
            {
                throw new ArgumentNullException("incomeTaxResultCode");
            }

            switch (incomeTaxResultCode)
            {
                case "00": result = "更新成功"; break;
                case "01": result = "查無資料"; break;
                case "02": result = "臨調中"; break;
                case "03": result = "非有效活卡"; break;
                case "04": result = "資料已刪除"; break;
                case "98": result = "LOG寫檔錯誤"; break;
                case "99": result = "寫檔錯誤"; break;
            }

            return result;
        }
    }
}