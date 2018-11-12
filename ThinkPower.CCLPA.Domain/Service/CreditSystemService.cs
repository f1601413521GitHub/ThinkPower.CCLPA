using System;
using System.Collections.Generic;
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
        #region Public Method

        /// <summary>
        /// ICRS查詢掛帳金額 (含已授權未清算)、可用額度
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public IcrsAmountInfo QueryIcrsAmount(string customerId)
        {
            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(customerId);
            }

            string serialNo = (customerId.Length > 10) ? customerId.Substring(10, 1) : null;

            IcrsAmount icrsAmount = new CreditSystemDAO().QueryIcrsAmount(customerId, serialNo);

            if (icrsAmount == null)
            {
                throw new InvalidOperationException($"{nameof(icrsAmount)} not found");
            }

            return ConvertIcrsAmountInfo(icrsAmount);
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
                throw new ArgumentNullException(nameof(adjustInfo));
            }

            IncomeTaxCardAdjust adjustInfoDO = ConvertIncomeTaxCardAdjustDO(adjustInfo);

            responseCode = new CreditSystemDAO().IncomeTaxCardAdjust(adjustInfoDO);

            return responseCode;
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


        #endregion


        #region Convert

        /// <summary>
        /// 轉換所得稅卡戶資訊
        /// </summary>
        /// <param name="info">所得稅卡戶資訊</param>
        /// <returns></returns>
        private IncomeTaxCardAdjust ConvertIncomeTaxCardAdjustDO(IncomeTaxCardAdjustInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
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
        /// 轉換所得稅卡戶臨調回傳碼
        /// </summary>
        /// <param name="incomeTaxResultCode">回傳碼</param>
        /// <returns>回傳結果</returns>
        internal string ConvertIncomeTaxResultCode(string incomeTaxResultCode)
        {
            string status = null;

            if (String.IsNullOrEmpty(incomeTaxResultCode))
            {
                throw new ArgumentNullException(nameof(incomeTaxResultCode));
            }

            switch (incomeTaxResultCode)
            {
                case "00":
                    status = "更新成功";
                    break;
                case "01":
                    status = "查無資料";
                    break;
                case "02":
                    status = "臨調中";
                    break;
                case "03":
                    status = "非有效活卡";
                    break;
                case "04":
                    status = "資料已刪除";
                    break;
                case "98":
                    status = "LOG寫檔錯誤";
                    break;
                case "99":
                    status = "寫檔錯誤";
                    break;
            }

            return status;
        }

        /// <summary>
        /// 轉換ICRS掛帳金額資料
        /// </summary>
        /// <param name="icrsAmount">ICRS掛帳金額資料</param>
        /// <returns></returns>
        private IcrsAmountInfo ConvertIcrsAmountInfo(IcrsAmount icrsAmount)
        {
            return (icrsAmount == null) ? null : new IcrsAmountInfo()
            {
                Amount = icrsAmount.Amount,
                AvailableCredit = icrsAmount.AvailableCredit,
                Flag = icrsAmount.Flag,
                Level = icrsAmount.Level,
                ResponseCode = icrsAmount.ResponseCode,
            };
        }

        #endregion
    }
}