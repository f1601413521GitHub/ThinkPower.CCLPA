using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using ThinkPower.CCLPA.DataAccess.Condition;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DAO.ICRS;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CMPN;
using ThinkPower.CCLPA.DataAccess.DO.ICRS;
using ThinkPower.CCLPA.DataAccess.VO;
using ThinkPower.CCLPA.Domain.Entity;
using ThinkPower.CCLPA.Domain.Service.Interface;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 臨調服務
    /// </summary>
    public class AdjustService : BaseService, IAdjust
    {
        #region Private Property

        /// <summary>
        /// 處理代碼
        /// </summary>
        private readonly string _progressCode = "B1";

        #endregion



        #region Public Method

        /// <summary>
        /// 檢核歸戶ID
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public AdjustVerifyResult Verify(string customerId)
        {
            List<string> errorCodeList = null;
            PreAdjustEntity tempPreAdjust = null;
            CustomerInfo tempCustomer = null;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }



            errorCodeList = new List<string>();

            IEnumerable<AdjustEntity> adjustList = GetApplyForAdjustment(customerId);

            if (adjustList.Any())
            {
                // TODO ClientSide ErrorMsg: 此歸戶ID，目前專案臨調PENDING處理中，無法再做申請動作。
                errorCodeList.Add("01");
            }
            else
            {
                InsertLogProgress(customerId);
            }

            CustomerInfo customer = GetCustomer(customerId);

            if (customer == null)
            {
                // TODO ClientSide ErrorMsg: 查無相關資料，請確認是否有輸入錯誤。
                errorCodeList.Add("02");
            }

            IEnumerable<PreAdjustEntity> preAdjustList = GetEffectPreAdjust(customerId);

            if (preAdjustList != null && preAdjustList.Any())
            {
                // TODO ClientSide ErrorMsg: 此歸戶已有生效中的預審專案...。
                errorCodeList.Add("03");
                tempPreAdjust = preAdjustList.First();
            }

            if (customer != null && HasAdjustEffecting(customer))
            {
                // TODO ClientSide ErrorMsg: 此歸戶已有生效中的臨調...您現在是否要繼續做專案臨調申請?
                errorCodeList.Add("04");
                tempCustomer = customer;
            }

            return new AdjustVerifyResult()
            {
                ErrorCodeList = errorCodeList,
                PreAdjustInfo = tempPreAdjust,
                CustomerInfo = tempCustomer,
            };
        }

        /// <summary>
        /// 取得臨調申請資料
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public AdjustApplicationInfo GetApplicationData(string customerId)
        {
            AdjustApplicationInfo adjustApplicationInfo = null;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }



            IcrsAmountInfo icrsAmountInfo = new CreditSystemService().QueryIcrsAmount(customerId);

            if ((icrsAmountInfo == null) ||
                (icrsAmountInfo.ResponseCode != "00"))
            {
                throw new InvalidOperationException($"{nameof(icrsAmountInfo)} not found or query fail");
            }

            JcicDateInfo jcicDateInfo = new AdjustSystemService() { UserInfo = UserInfo }.QueryJcicDate(customerId);

            if ((icrsAmountInfo == null) ||
                (icrsAmountInfo.ResponseCode != "00") ||
                (icrsAmountInfo.ResponseCode != "72") ||
                (icrsAmountInfo.ResponseCode != "73"))
            {
                throw new InvalidOperationException($"{nameof(jcicDateInfo)} not found or query fail");
            }


            CustomerService customerSerivce = new CustomerService();

            CustomerInfo customerInfo = customerSerivce.Get(customerId);

            if (customerInfo == null)
            {
                throw new InvalidOperationException($"{nameof(customerInfo)} not found");
            }


            VipInfo vipInfo = customerSerivce.GetVip(customerId, DateTime.Today);

            if (vipInfo == null)
            {
                throw new InvalidOperationException($"{nameof(vipInfo)} not found");
            }

            IEnumerable<AdjustEntity> adjustList = QueryAdjust(new AdjustCondition()
            {
                PageIndex = 1,
                PagingSize = 5,
                CustomerId = customerId,
                OrderBy = AdjustCondition.OrderByKind.ProcessDateByDescendingAndProcessTimeByDescending
            });


            adjustApplicationInfo = new AdjustApplicationInfo()
            {
                Customer = customerInfo,
                Vip = vipInfo,
                JcicDate = jcicDateInfo,
                AdjustLogList = adjustList,
            };

            return adjustApplicationInfo;
        }

        public void Application()
        {
            // TODO 申請
            throw new NotImplementedException();
        }

        public void Approved()
        {
            // TODO 核准
            throw new NotImplementedException();
        }

        public void Refused()
        {
            // TODO 拒絕
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            // TODO 取消
            throw new NotImplementedException();
        }

        #endregion



        #region Private Method

        #region Verify

        /// <summary>
        /// 取得已申請的臨調資料
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns>檢核結果</returns>
        private IEnumerable<AdjustEntity> GetApplyForAdjustment(string customerId)
        {
            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            IEnumerable<AdjustDO> adjustList = new AdjustDAO().Query(new AdjustCondition
            {
                CustomerId = customerId,
                ChiefFlag = "Y",
                PendingFlag = "Y",
                ProjectStatus = "Y"
            });

            return ConvertAdjustEntity(adjustList);
        }

        /// <summary>
        /// 取得歸戶基本資料
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns>檢核結果</returns>
        private CustomerInfo GetCustomer(string customerId)
        {
            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            return new CustomerService().Get(customerId);
        }

        /// <summary>
        /// 取得生效的預審資料
        /// 是否存在
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        private IEnumerable<PreAdjustEntity> GetEffectPreAdjust(string customerId)
        {
            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            return new PreAdjustService().GetEffectPreAdjust(customerId);
        }

        /// <summary>
        /// 是否為臨調生效中
        /// </summary>
        /// <param name="customer">客戶ID</param>
        /// <returns></returns>
        private bool HasAdjustEffecting(CustomerInfo customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            DateTime currentDate = DateTime.Today;

            DateTime startDate = DateTime.TryParseExact(customer.AdjustStartDate, "yyyyMMdd", null,
                DateTimeStyles.None, out DateTime tempStartDate) ? tempStartDate :
                throw new InvalidOperationException("Convert AdjustStartDate fail");

            DateTime endDate = DateTime.TryParseExact(customer.AdjustStartDate, "yyyyMMdd", null,
                DateTimeStyles.None, out DateTime tempEndDate) ? tempEndDate :
                throw new InvalidOperationException("Convert AdjustEndDate fail");

            return ((currentDate >= startDate) && (currentDate <= endDate));
        }

        #endregion

        #region Insert/Update/Delete

        /// <summary>
        /// 新增臨調紀錄
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        private void InsertLogProgress(string customerId)
        {
            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            DateTime currentTime = DateTime.Now;

            new LogProgressDAO().Insert(new LogProgressDO()
            {
                ApplicationNo = customerId,
                ApplicationKind = null,
                ProgressCode = _progressCode,
                ProgressId = UserInfo.Id,
                ProgressName = UserInfo.Name,
                ProgressDate = currentTime.ToString("yyyyMMdd"),
                ProgressTime = currentTime.ToString("HHmmss"),
                SerialNo = null,
                Memo = null,
            });
        }

        /// <summary>
        /// 查詢臨調資料
        /// </summary>
        /// <param name="condition">臨調資料查詢條件</param>
        /// <returns></returns>
        private IEnumerable<AdjustEntity> QueryAdjust(AdjustCondition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            IEnumerable<AdjustDO> adjustList = new AdjustDAO().Query(condition);

            return ConvertAdjustEntity(adjustList);
        }

        #endregion

        #region Convert

        /// <summary>
        /// 轉換臨調資料
        /// </summary>
        /// <param name="adjustList">臨調資料</param>
        /// <returns></returns>
        private IEnumerable<AdjustEntity> ConvertAdjustEntity(IEnumerable<AdjustDO> adjustList)
        {
            return (adjustList == null) ? null : adjustList.Select(x => new AdjustEntity()
            {
                Id = x.Id,
                ApplyDate = x.ApplyDate,
                ApplyTime = x.ApplyTime,
                CustomerId = x.CustomerId,
                CustomerName = x.CustomerName,
                CreditLimit = x.CreditLimit,
                ApplyAmount = x.ApplyAmount,
                UseSite = x.UseSite,
                Place = x.Place,
                AdjustDateStart = x.AdjustDateStart,
                AdjustDateEnd = x.AdjustDateEnd,
                Reason1 = x.Reason1,
                Reason2 = x.Reason2,
                Reason3 = x.Reason3,
                Reason = x.Reason,
                Remark = x.Remark,
                ForceAuthenticate = x.ForceAuthenticate,
                ApproveAmountMax = x.ApproveAmountMax,
                UsabilityAmount = x.UsabilityAmount,
                OverpayAmountPro = x.OverpayAmountPro,
                ApproveAmount = x.ApproveAmount,
                OverpayAmount = x.OverpayAmount,
                EstimateResult = x.EstimateResult,
                RejectReason = x.RejectReason,
                ApproveResult = x.ApproveResult,
                ChiefFlag = x.ChiefFlag,
                ChiefRemark = x.ChiefRemark,
                PendingFlag = x.PendingFlag,
                UserId = x.UserId,
                UserName = x.UserName,
                ChiefId = x.ChiefId,
                ChiefName = x.ChiefName,
                JcicDate = x.JcicDate,
                Type = x.Type,
                CcasCode = x.CcasCode,
                CcasStatus = x.CcasStatus,
                CcasDateTime = x.CcasDateTime,
                ProcessDate = x.ProcessDate,
                ProcessTime = x.ProcessTime,
                IcareStatus = x.IcareStatus,
                ProjectStatus = x.ProjectStatus,
                ProjectAdjustResult = x.ProjectAdjustResult,
                ProjectAdjustRejectReason = x.ProjectAdjustRejectReason,
                CreditAmount = x.CreditAmount,
            });
        }

        #endregion

        #endregion
    }
}
