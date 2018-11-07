using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
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
        /// <summary>
        /// 處理代碼
        /// </summary>
        private readonly string _progressCode = "B1";

        /// <summary>
        /// 是否已申請專案臨調
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns>檢核結果</returns>
        private bool HasApplyForAdjustment(string customerId)
        {
            bool hasApply = false;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }



            AdjustDO adjustInfo = new AdjustDAO().Get(customerId);

            if (adjustInfo != null &&
                (adjustInfo.ChiefFlag == "Y" || adjustInfo.PendingFlag == "Y") &&
                adjustInfo.ProjectStatus == "Y")
            {
                hasApply = true;
            }
            else
            {
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

            return hasApply;
        }

        /// <summary>
        /// 是否存在於歸戶基本資料
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns>檢核結果</returns>
        private bool HasExistCustomer(string customerId)
        {
            bool hasExist = false;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }



            CustomerDO customerInfo = new CustomerDAO().Get(customerId);

            if (customerInfo != null)
            {
                hasExist = true;
            }

            return hasExist;
        }

        /// <summary>
        /// 是否存在生效中的預審專案
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        private PreAdjustEntity HasExistEffectPreAdjust(string customerId)
        {
            PreAdjustEntity preAdjust = null;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }



            IEnumerable<PreAdjustDO> preAdjustList = new PreAdjustDAO().GetById(customerId).
                Where(x => x.CcasReplyCode == "00");

            if (preAdjustList.Any())
            {
                preAdjust = ConvertPreAdjustEntity(preAdjustList.First());
            }

            return preAdjust;
        }

        /// <summary>
        /// 是否為臨調生效中
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        private CustomerEntity HasAdjustEffecting(string customerId)
        {
            CustomerEntity customer = null;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }



            CustomerDO customerInfo = new CustomerDAO().Get(customerId);

            if (customerInfo != null)
            {
                DateTime currentTime = DateTime.Today;

                DateTime startDate = DateTime.TryParseExact(customerInfo.AdjustStartDate, "yyyyMMdd", null,
                    DateTimeStyles.None, out DateTime tempStartDate) ? tempStartDate :
                    throw new InvalidOperationException("Convert adjustStartDate fail");

                DateTime endDate = DateTime.TryParseExact(customerInfo.AdjustEndDate, "yyyyMMdd", null,
                    DateTimeStyles.None, out DateTime tempEndDate) ? tempEndDate :
                    throw new InvalidOperationException("Convert adjustEndDate fail");

                if ((currentTime >= startDate) &&
                    (currentTime <= endDate))
                {
                    customer = ConvertCustomerEntity(customerInfo);
                }
            }

            return customer;
        }

        /// <summary>
        /// 檢核臨調流程
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public VerifiedResult Verified(string customerId)
        {
            VerifiedResult verifiedResult = null;
            Dictionary<string, string> errorInfo = null;
            PreAdjustEntity tempPreAdjust = null;
            CustomerEntity tempCustomer = null;


            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }



            errorInfo = new Dictionary<string, string>();

            bool hasApply = HasApplyForAdjustment(customerId);

            if (hasApply)
            {
                errorInfo.Add("01", "此歸戶ID，目前專案臨調PENDING處理中，無法再做申請動作。");
            }

            bool hasExist = HasExistCustomer(customerId);

            if (!hasExist)
            {
                errorInfo.Add("02", "查無相關資料，請確認是否有輸入錯誤。");
            }

            PreAdjustEntity preAdjust = HasExistEffectPreAdjust(customerId);

            if (preAdjust != null)
            {
                errorInfo.Add("03", "Dialog");
                tempPreAdjust = preAdjust;
                // TODO "此歸戶已有生效中的預審專案...";
            }

            CustomerEntity customer = HasAdjustEffecting(customerId);

            if (customer != null)
            {
                errorInfo.Add("04", "Dialog");
                tempCustomer = customer;
                // TODO "此歸戶已有生效中的臨調...您現在是否要繼續做專案臨調申請?";
            }



            verifiedResult = new VerifiedResult()
            {
                ErrorInfo = errorInfo,
                PreAdjustInfo =  tempPreAdjust,
                CustomerInfo = tempCustomer,
            };

            return verifiedResult;
        }


        /// <summary>
        /// 申請臨調處理
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public object ApplyForAdjustProcess(string customerId)
        {
            object result = null;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }



            VerifiedResult verifiedResult = Verified(customerId);

            if (verifiedResult == null || verifiedResult.ErrorInfo.Any())
            {
                throw new InvalidOperationException("Verified fail");
            }


            string serialNo = (customerId.Length > 10) ? customerId.Substring(10, 1) : null;

            QueryIcrsAmountResult icrsAmountInfo = new CreditSystemDAO().
                QueryIcrsAmount(customerId, serialNo);

            if (icrsAmountInfo == null || icrsAmountInfo.ResponseCode != "00")
            {
                throw new InvalidOperationException("QueryIcrsAmount fail");
            }


            JcicQueryResult jcicQueryInfo = new AdjustSystemDAO().
                QueryJcicDate(customerId, UserInfo.Id, UserInfo.Name);

            if (icrsAmountInfo == null || icrsAmountInfo.ResponseCode != "00")
            {
                throw new InvalidOperationException("QueryJcicDate fail");
            }

            return result;
        }
















        /// <summary>
        /// 轉換歸戶基本資料
        /// </summary>
        /// <param name="customerInfo">歸戶基本資料</param>
        /// <returns></returns>
        private CustomerEntity ConvertCustomerEntity(CustomerDO customerInfo)
        {
            if (customerInfo == null)
            {
                throw new ArgumentNullException("customerInfo");
            }

            return new CustomerEntity()
            {
                AccountId = customerInfo.AccountId,
                ChineseName = customerInfo.ChineseName,
                BirthDay = customerInfo.BirthDay,
                RiskLevel = customerInfo.RiskLevel,
                RiskRating = customerInfo.RiskRating,
                CreditLimit = customerInfo.CreditLimit,
                AboutDataStatus = customerInfo.AboutDataStatus,
                IssueDate = customerInfo.IssueDate,
                LiveCardCount = customerInfo.LiveCardCount,
                Status = customerInfo.Status,
                Vocation = customerInfo.Vocation,
                BillAddr = customerInfo.BillAddr,
                TelOffice = customerInfo.TelOffice,
                TelHome = customerInfo.TelHome,
                MobileTel = customerInfo.MobileTel,
                Latest1Mnth = customerInfo.Latest1Mnth,
                Latest2Mnth = customerInfo.Latest2Mnth,
                Latest3Mnth = customerInfo.Latest3Mnth,
                Latest4Mnth = customerInfo.Latest4Mnth,
                Latest5Mnth = customerInfo.Latest5Mnth,
                Latest6Mnth = customerInfo.Latest6Mnth,
                Latest7Mnth = customerInfo.Latest7Mnth,
                Latest8Mnth = customerInfo.Latest8Mnth,
                Latest9Mnth = customerInfo.Latest9Mnth,
                Latest10Mnth = customerInfo.Latest10Mnth,
                Latest11Mnth = customerInfo.Latest11Mnth,
                Latest12Mnth = customerInfo.Latest12Mnth,
                Consume1 = customerInfo.Consume1,
                Consume2 = customerInfo.Consume2,
                Consume3 = customerInfo.Consume3,
                Consume4 = customerInfo.Consume4,
                Consume5 = customerInfo.Consume5,
                Consume6 = customerInfo.Consume6,
                Consume7 = customerInfo.Consume7,
                Consume8 = customerInfo.Consume8,
                Consume9 = customerInfo.Consume9,
                Consume10 = customerInfo.Consume10,
                Consume11 = customerInfo.Consume11,
                Consume12 = customerInfo.Consume12,
                PreCash1 = customerInfo.PreCash1,
                PreCash2 = customerInfo.PreCash2,
                PreCash3 = customerInfo.PreCash3,
                PreCash4 = customerInfo.PreCash4,
                PreCash5 = customerInfo.PreCash5,
                PreCash6 = customerInfo.PreCash6,
                PreCash7 = customerInfo.PreCash7,
                PreCash8 = customerInfo.PreCash8,
                PreCash9 = customerInfo.PreCash9,
                PreCash10 = customerInfo.PreCash10,
                PreCash11 = customerInfo.PreCash11,
                PreCash12 = customerInfo.PreCash12,
                CreditRating1 = customerInfo.CreditRating1,
                CreditRating2 = customerInfo.CreditRating2,
                CreditRating3 = customerInfo.CreditRating3,
                CreditRating4 = customerInfo.CreditRating4,
                CreditRating5 = customerInfo.CreditRating5,
                CreditRating6 = customerInfo.CreditRating6,
                CreditRating7 = customerInfo.CreditRating7,
                CreditRating8 = customerInfo.CreditRating8,
                CreditRating9 = customerInfo.CreditRating9,
                CreditRating10 = customerInfo.CreditRating10,
                CreditRating11 = customerInfo.CreditRating11,
                CreditRating12 = customerInfo.CreditRating12,
                ClosingDay = customerInfo.ClosingDay,
                PayDeadline = customerInfo.PayDeadline,
                ClosingAmount = customerInfo.ClosingAmount,
                MinimumAmountPayable = customerInfo.MinimumAmountPayable,
                RecentPaymentAmount = customerInfo.RecentPaymentAmount,
                RecentPaymentDate = customerInfo.RecentPaymentDate,
                OfferAmount = customerInfo.OfferAmount,
                UnpaidTotal = customerInfo.UnpaidTotal,
                AuthorizedAmountNotAccount = customerInfo.AuthorizedAmountNotAccount,
                AdjustReason = customerInfo.AdjustReason,
                AdjustArea = customerInfo.AdjustArea,
                AdjustStartDate = customerInfo.AdjustStartDate,
                AdjustEndDate = customerInfo.AdjustEndDate,
                AdjustEffectAmount = customerInfo.AdjustEffectAmount,
                VintageMonths = customerInfo.VintageMonths,
                StatusFlag = customerInfo.StatusFlag,
                GutrFlag = customerInfo.GutrFlag,
                DelayCount = customerInfo.DelayCount,
                CcasUnderpaidAmount = customerInfo.CcasUnderpaidAmount,
                CcasUsabilityAmount = customerInfo.CcasUsabilityAmount,
                CcasUnderpaidRate = customerInfo.CcasUnderpaidRate,
                DataDate = customerInfo.DataDate,
                EligibilityForWithdrawal = customerInfo.EligibilityForWithdrawal,
                SystemAdjustRevFlag = customerInfo.SystemAdjustRevFlag,
                AutomaticDebit = customerInfo.AutomaticDebit,
                DebitBankCode = customerInfo.DebitBankCode,
                EtalStatus = customerInfo.EtalStatus,
                TelResident = customerInfo.TelResident,
                SendType = customerInfo.SendType,
                ElectronicBillingCustomerNote = customerInfo.ElectronicBillingCustomerNote,
                Email = customerInfo.Email,
                Industry = customerInfo.Industry,
                JobTitle = customerInfo.JobTitle,
                ResidentAddr = customerInfo.ResidentAddr,
                MailingAddr = customerInfo.MailingAddr,
                CompanyAddr = customerInfo.CompanyAddr,
                AnnualIncome = customerInfo.AnnualIncome,
                In1 = customerInfo.In1,
                In2 = customerInfo.In2,
                In3 = customerInfo.In3,
                ResidentAddrPostalCode = customerInfo.ResidentAddrPostalCode,
                MailingAddrPostalCode = customerInfo.MailingAddrPostalCode,
                CompanyAddrPostalCode = customerInfo.CompanyAddrPostalCode,
            };
        }


        /// <summary>
        /// 轉換臨調預審名單資訊
        /// </summary>
        /// <param name="preAdjustDO">臨調預審名單資訊</param>
        /// <returns></returns>
        private PreAdjustEntity ConvertPreAdjustEntity(PreAdjustDO preAdjustDO)
        {
            if (preAdjustDO == null)
            {
                throw new ArgumentNullException("preAdjustDO");
            }

            return new PreAdjustEntity()
            {
                CampaignId = preAdjustDO.CampaignId,
                CustomerId = preAdjustDO.CustomerId,
                ProjectName = preAdjustDO.ProjectName,
                ProjectAmount = preAdjustDO.ProjectAmount,
                CloseDate = preAdjustDO.CloseDate,
                ImportDate = preAdjustDO.ImportDate,
                ChineseName = preAdjustDO.ChineseName,
                Kind = preAdjustDO.Kind,
                SmsCheckResult = preAdjustDO.SmsCheckResult,
                Status = preAdjustDO.Status,
                ProcessingDateTime = preAdjustDO.ProcessingDateTime,
                ProcessingUserId = preAdjustDO.ProcessingUserId,
                DeleteDateTime = preAdjustDO.DeleteDateTime,
                DeleteUserId = preAdjustDO.DeleteUserId,
                Remark = preAdjustDO.Remark,
                ClosingDay = preAdjustDO.ClosingDay,
                PayDeadline = preAdjustDO.PayDeadline,
                ForceAgreeUserId = preAdjustDO.ForceAgreeUserId,
                MobileTel = preAdjustDO.MobileTel,
                RejectReasonCode = preAdjustDO.RejectReasonCode,
                CcasReplyCode = preAdjustDO.CcasReplyCode,
                CcasReplyStatus = preAdjustDO.CcasReplyStatus,
                CcasReplyDateTime = preAdjustDO.CcasReplyDateTime,
            };
        }
    }
}
