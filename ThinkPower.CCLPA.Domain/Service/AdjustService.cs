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



            IEnumerable<AdjustDO> adjustInfoList = new AdjustDAO().Get(customerId).
                Where(x => ((x.ChiefFlag == "Y") || (x.PendingFlag == "Y")) &&
                            (x.ProjectStatus == "Y"));

            if (adjustInfoList.Any())
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
        private CustomerInfo HasAdjustEffecting(string customerId)
        {
            CustomerInfo customer = null;

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
                    throw new InvalidOperationException("Convert AdjustStartDate fail");

                DateTime endDate = DateTime.TryParseExact(customerInfo.AdjustEndDate, "yyyyMMdd", null,
                    DateTimeStyles.None, out DateTime tempEndDate) ? tempEndDate :
                    throw new InvalidOperationException("Convert AdjustEndDate fail");

                if ((currentTime >= startDate) &&
                    (currentTime <= endDate))
                {
                    customer = ConvertCustomerInfo(customerInfo);
                }
            }

            return customer;
        }

        /// <summary>
        /// 檢核臨調流程
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public VerifiedResult Verifiy(string customerId)
        {
            VerifiedResult verifiedResult = null;
            Dictionary<string, string> errorInfo = null;
            PreAdjustEntity tempPreAdjust = null;
            CustomerInfo tempCustomer = null;


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

            CustomerInfo customer = HasAdjustEffecting(customerId);

            if (customer != null)
            {
                errorInfo.Add("04", "Dialog");
                tempCustomer = customer;
                // TODO "此歸戶已有生效中的臨調...您現在是否要繼續做專案臨調申請?";
            }



            verifiedResult = new VerifiedResult()
            {
                ErrorInfo = errorInfo,
                PreAdjustInfo = tempPreAdjust,
                CustomerInfo = tempCustomer,
            };

            return verifiedResult;
        }


        /// <summary>
        /// 申請臨調處理
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        public AdjustProcessResult ApplyForAdjustProcess(string customerId)
        {
            AdjustProcessResult result = null;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("customerId");
            }





            VerifiedResult verifiedResult = Verifiy(customerId);

            if (verifiedResult == null || verifiedResult.ErrorInfo.Any())
            {
                throw new InvalidOperationException("Verified fail");
                // TODO if else 拆解?
            }





            string serialNo = (customerId.Length > 10) ? customerId.Substring(10, 1) : null;

            QueryIcrsAmountResult icrsAmountInfo = new CreditSystemDAO().
                QueryIcrsAmount(customerId, serialNo);

            if (icrsAmountInfo == null || icrsAmountInfo.ResponseCode != "00")
            {
                throw new InvalidOperationException("QueryIcrsAmount fail");
                // TODO Check ResponseCode!="00" is fail? / if else 拆解?
            }


            JcicQueryResult jcicQueryInfo = new AdjustSystemDAO().
                QueryJcicDate(customerId, UserInfo.Id, UserInfo.Name);

            if (icrsAmountInfo == null || icrsAmountInfo.ResponseCode != "00")
            {
                throw new InvalidOperationException("QueryJcicDate fail");
                // TODO Check ResponseCode!="00" is fail? / if else 拆解?
            }





            CustomerDO customerInfo = new CustomerDAO().Get(customerId);

            if (customerInfo == null)
            {
                throw new InvalidOperationException("Customer not found");
            }


            VipDO vipInfo = new VipDAO().Get(customerId, DateTime.Today);

            if (vipInfo == null)
            {
                throw new InvalidOperationException("VipData not found");
            }


            IEnumerable<IncreaseReasonCodeDO> increaseReasonList = new IncreaseReasonCodeDAO().GetAll().
                Where(x => x.UseFlag == "Y");

            if (!increaseReasonList.Any())
            {
                throw new InvalidOperationException("IncreaseReason not found");
            }


            IEnumerable<ParamCurrentlyEffectDO> currentlyEffectList = new ParamCurrentlyEffectDAO().
                Get(increaseReasonList.Select(x => x.Code));

            if (!currentlyEffectList.Any())
            {
                throw new ArgumentNullException("CurrentlyEffect not found");
            }
            else
            {
                DateTime currentTime = DateTime.Today;
                DateTime tempStartTime;
                DateTime tempEndTime;
                List<ParamCurrentlyEffectDO> tempCurrentlyEffect = new List<ParamCurrentlyEffectDO>();

                foreach (ParamCurrentlyEffectDO effectItem in currentlyEffectList)
                {
                    if (!DateTime.TryParseExact(effectItem.AdjustDateStart, "yyyy/MM/dd", null,
                            DateTimeStyles.None, out tempStartTime))
                    {
                        throw new InvalidOperationException("Convert AdjustDateStart fail");
                    }

                    if (!DateTime.TryParseExact(effectItem.AdjustDateEnd, "yyyy/MM/dd", null,
                            DateTimeStyles.None, out tempEndTime))
                    {
                        throw new InvalidOperationException("Convert AdjustDateEnd fail");
                    }

                    if ((currentTime >= tempStartTime) &&
                        (currentTime <= tempEndTime))
                    {
                        tempCurrentlyEffect.Add(effectItem);
                    }
                }

                currentlyEffectList = tempCurrentlyEffect;
            }


            IEnumerable<AdjustDO> adjustList = new AdjustDAO().Get(customerId).
                OrderByDescending(x => x.ProcessDate).ThenByDescending(x => x.ProcessTime).Take(5);


            result = new AdjustProcessResult()
            {
                CustomerInfo = ConvertCustomerInfo(customerInfo),
                VipInfo = ConvertVipInfo(vipInfo),
                JcicInfo = ConvertJcicQueryResultInfo(jcicQueryInfo),
                IncreaseReasonList = ConvertIncreaseReasonCodeInfo(increaseReasonList),
                CurrentlyEffectList = ConvertParamCurrentlyEffectInfo(currentlyEffectList),
                AdjustList = ConvertAdjustInfo(adjustList),
            };



            return result;
        }













        #region Private Method

        /// <summary>
        /// 轉換歸戶基本資料
        /// </summary>
        /// <param name="customerInfo">歸戶基本資料</param>
        /// <returns></returns>
        private CustomerInfo ConvertCustomerInfo(CustomerDO customerInfo)
        {
            if (customerInfo == null)
            {
                throw new ArgumentNullException("customerInfo");
            }

            return new CustomerInfo()
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

        /// <summary>
        /// 轉換臨調處理資訊
        /// </summary>
        /// <param name="adjustList">臨調處理資訊</param>
        /// <returns></returns>
        private IEnumerable<AdjustInfo> ConvertAdjustInfo(IEnumerable<AdjustDO> adjustList)
        {
            if (adjustList == null)
            {
                throw new ArgumentNullException("adjustList");
            }

            return adjustList.Select(x => new AdjustInfo()
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

        /// <summary>
        /// 轉換參數目前生效資訊
        /// </summary>
        /// <param name="currentlyEffectList">參數目前生效資訊</param>
        /// <returns></returns>
        private IEnumerable<ParamCurrentlyEffectInfo> ConvertParamCurrentlyEffectInfo(
            IEnumerable<ParamCurrentlyEffectDO> currentlyEffectList)
        {
            if (currentlyEffectList == null)
            {
                throw new ArgumentNullException("currentlyEffectList");
            }

            return currentlyEffectList.Select(x => new ParamCurrentlyEffectInfo()
            {
                Reason = x.Reason,
                EffectDate = x.EffectDate,
                AdjustDateStart = x.AdjustDateStart,
                AdjustDateEnd = x.AdjustDateEnd,
                ApproveAmountMax = x.ApproveAmountMax,
                Remark = x.Remark,
                VerifiyCondition = x.VerifiyCondition,
                ApproveScaleMax = x.ApproveScaleMax,
            });
        }

        /// <summary>
        /// 轉換調高原因代碼資訊
        /// </summary>
        /// <param name="increaseReasonList">調高原因代碼資訊</param>
        /// <returns></returns>
        private IEnumerable<IncreaseReasonCodeInfo> ConvertIncreaseReasonCodeInfo(
            IEnumerable<IncreaseReasonCodeDO> increaseReasonList)
        {
            if (increaseReasonList == null || !increaseReasonList.Any())
            {
                throw new ArgumentNullException("increaseReasonList");
            }

            return increaseReasonList.Select(x => new IncreaseReasonCodeInfo()
            {
                Code = x.Code,
                Name = x.Name,
                UseFlag = x.UseFlag,
            });
        }

        /// <summary>
        /// 轉換JCIC查詢結果
        /// </summary>
        /// <param name="jcicQueryInfo">JCIC查詢結果</param>
        /// <returns></returns>
        private JcicQueryResultInfo ConvertJcicQueryResultInfo(JcicQueryResult jcicQueryInfo)
        {
            if (jcicQueryInfo == null)
            {
                throw new ArgumentNullException("jcicQueryInfo");
            }

            return new JcicQueryResultInfo()
            {
                JcicQueryDate = jcicQueryInfo.JcicQueryDate,
                ResponseCode = jcicQueryInfo.ResponseCode,
            };
        }

        /// <summary>
        /// 轉換貴賓資訊
        /// </summary>
        /// <param name="vipInfo">貴賓資訊</param>
        /// <returns></returns>
        private VipInfo ConvertVipInfo(VipDO vipInfo)
        {
            if (vipInfo == null)
            {
                throw new ArgumentNullException("vipInfo");
            }

            return new VipInfo()
            {
                CustomerId = vipInfo.CustomerId,
                DataDate = vipInfo.DataDate,
                DataChangeDate = vipInfo.DataChangeDate,
                ApplicableStarLevel = vipInfo.ApplicableStarLevel,
                ApplicableStarValidityPeriod = vipInfo.ApplicableStarValidityPeriod,
                MonthStarLevel = vipInfo.MonthStarLevel,
                MonthStarValidityPeriod = vipInfo.MonthStarValidityPeriod,
                BusinessBalance = vipInfo.BusinessBalance,
                AverageBalance = vipInfo.AverageBalance,
                InventotyBalance = vipInfo.InventotyBalance,
                PremiunsPaid = vipInfo.PremiunsPaid,
                AUM = vipInfo.AUM,
                NearlyYearSwipeAmount = vipInfo.NearlyYearSwipeAmount,
                MortgageBalance = vipInfo.MortgageBalance,
                ForexMargin = vipInfo.ForexMargin,
                ConvertiblePrincipal = vipInfo.ConvertiblePrincipal,
                ReDelegate = vipInfo.ReDelegate,
                CardVipFlag = vipInfo.CardVipFlag,
                HouseholdExceptionStars = vipInfo.HouseholdExceptionStars,
                LegalExceptionStars = vipInfo.LegalExceptionStars,
            };
        } 

        #endregion
    }
}
