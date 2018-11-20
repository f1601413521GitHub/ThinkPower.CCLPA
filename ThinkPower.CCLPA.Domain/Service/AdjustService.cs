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
        /// 處理代碼列舉
        /// </summary>
        private enum ProgressCode
        {
            None = 0,
            /// <summary>
            /// 檢核歸戶ID
            /// </summary>
            Verify,
            /// <summary>
            /// 轉授信主管
            /// </summary>
            ForwardingSupervisor,
        }

        /// <summary>
        /// 處理類型列舉
        /// </summary>
        private enum ProgressType
        {
            None = 0,
            /// <summary>
            /// 預審臨調
            /// </summary>
            PreAdjust,
            /// <summary>
            /// 臨調
            /// </summary>
            Adjust,
            /// <summary>
            /// 專案臨調
            /// </summary>
            ProjectAdjust
        }

        /// <summary>
        /// 使用地點列舉
        /// </summary>
        private enum UseSite
        {
            None = 0,
            /// <summary>
            /// 國外
            /// </summary>
            Foreign,
            /// <summary>
            /// 國內
            /// </summary>
            Domestic,
            /// <summary>
            /// 國內外
            /// </summary>
            DomesticAndForeign
        }

        /// <summary>
        /// 調整原因(12)綜所稅專款專用
        /// </summary>
        private readonly string _adjustReasonCodeByComprehensiveIncomeTax = "12";

        /// <summary>
        /// 刷卡金額(不含額度)最小資料長度
        /// </summary>
        private readonly int _creaditAmountMinLength = 5;
        /// <summary>
        /// 刷卡金額(不含額度)最大資料長度
        /// </summary>
        private readonly int _creaditAmountMaxLength = 9;
        /// <summary>
        /// 申請金額最小資料長度
        /// </summary>
        private readonly int _applyAmountMinLength = 5;
        /// <summary>
        /// 申請金額最大資料長度
        /// </summary>
        private readonly int _applyAmountMaxLength = 9;
        /// <summary>
        /// 額度上限
        /// </summary>
        private readonly decimal _scale = 100;

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
            Customer tempCustomer = null;

            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }
            else if (UserInfo == null)
            {
                throw new ArgumentNullException(nameof(UserInfo));
            }



            errorCodeList = new List<string>();

            IEnumerable<AdjustEntity> adjustList = GetApplyForAdjustment(customerId);

            if (adjustList.Any())
            {
                errorCodeList.Add("01");
            }
            else
            {
                DateTime currentTime = DateTime.Now;
                var logProgress = new LogProgress()
                {
                    ApplicationNo = customerId,
                    ApplicationKind = null,
                    ProgressCode = ConvertProgressCode(ProgressCode.Verify),
                    ProgressId = UserInfo.Id,
                    ProgressName = UserInfo.Name,
                    ProgressDate = currentTime.ToString("yyyyMMdd"),
                    ProgressTime = currentTime.ToString("HHmmss"),
                    SerialNo = null,
                    Memo = null,
                };

                InsertLogProgress(logProgress);
            }

            Customer customer = GetCustomer(customerId);

            if (customer == null)
            {
                errorCodeList.Add("02");
            }

            IEnumerable<PreAdjustEntity> preAdjustList = GetEffectPreAdjust(new PreAdjustCondition()
            {
                PageIndex = 1,
                PagingSize = 1,
                CustomerId = customerId,
                CcasReplyCode = "00"
            });

            if (preAdjustList != null && preAdjustList.Any())
            {
                errorCodeList.Add("03");
                tempPreAdjust = preAdjustList.First();
            }

            if (customer != null && HasAdjustEffecting(customer))
            {
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
        public AdjustApplication GetApplicationData(string customerId)
        {
            AdjustApplication adjustApplicationInfo = null;

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


            AdjustSystemService adjustSysService = new AdjustSystemService() { UserInfo = UserInfo };

            JcicSendQueryResult jcicDateInfo = adjustSysService.JcicSendQuery(customerId);

            if ((jcicDateInfo == null) ||
                (jcicDateInfo.ResponseCode != "00") &&
                (jcicDateInfo.ResponseCode != "72") &&
                (jcicDateInfo.ResponseCode != "73"))
            {
                throw new InvalidOperationException($"{nameof(jcicDateInfo)} not found or query fail");
            }


            CustomerService customerSerivce = new CustomerService();

            Customer customerInfo = customerSerivce.Get(customerId);

            if (customerInfo == null)
            {
                throw new InvalidOperationException($"{nameof(customerInfo)} not found");
            }


            VipInfo vipInfo = customerSerivce.GetVip(customerId, DateTime.Today);


            AdjustConditionValidateResult adjustValidateResult = adjustSysService.
                ValidateAdjustCondition(customerId, jcicDateInfo.JcicQueryDate, customerInfo.AdjustReason);


            IEnumerable<AdjustEntity> adjustList = QueryAdjust(new AdjustCondition()
            {
                PageIndex = 1,
                PagingSize = 5,
                CustomerId = customerId,
                Type = new string[] { "2", "3" },
                OrderBy = AdjustCondition.OrderByKind.ApplyDateByDescendingAndApplyTimeByDescending
            });


            adjustApplicationInfo = new AdjustApplication()
            {
                JcicSendQuery = jcicDateInfo,
                Customer = customerInfo,
                Vip = vipInfo,
                AdjustValidateResult = adjustValidateResult,
                AdjustLogList = adjustList,
            };

            return adjustApplicationInfo;
        }

        /// <summary>
        /// 轉授信主管
        /// </summary>
        /// <param name="forwardData">轉授信主管資料</param>
        public void ForwardingSupervisor(ForwardingSupervisor forwardData)
        {
            if (forwardData == null)
            {
                throw new ArgumentNullException(nameof(forwardData));
            }
            else if (UserInfo == null)
            {
                throw new ArgumentNullException(nameof(UserInfo));
            }



            ForwardingSupervisor newForwardData = VerifyForwardingSupervisorData(forwardData);

            Customer customerInfo = new CustomerService().Get(newForwardData.CustomerId);

            if (customerInfo == null)
            {
                throw new InvalidOperationException($"{nameof(customerInfo)} not found");
            }
            else
            {
                decimal? creditLimit = customerInfo.CreditLimit;
                if ((creditLimit != null) &&
                    (newForwardData.ApplyAmount < creditLimit))
                {
                    throw new InvalidOperationException($"{newForwardData.ApplyAmount}" +
                        $"Must be greater than or equal {customerInfo.CreditLimit}");
                }
            }

            AdjustReason effectiveReason = new ParamterService().GetEffectiveAdjustReason().
                Where(x => x.ReasonEffectInfo.Reason.Equals(newForwardData.Reason)).FirstOrDefault();

            if (effectiveReason == null)
            {
                throw new InvalidOperationException($"{nameof(effectiveReason)} not found");
            }

            DateTime currentTime = DateTime.Now;
            decimal? effectReasonApproveAmountMax = effectiveReason.ReasonEffectInfo?.ApproveAmountMax;
            decimal? effectReasonApproveScaleMax = effectiveReason.ReasonEffectInfo?.ApproveScaleMax;
            decimal? tempApproveAmountMax = null;

            if (effectReasonApproveAmountMax != null)
            {
                if (effectReasonApproveAmountMax.Value == 0)
                {
                    tempApproveAmountMax = null;
                }
                else
                {
                    tempApproveAmountMax = effectReasonApproveAmountMax.Value;
                }
            }
            else if (effectReasonApproveScaleMax != null)
            {
                tempApproveAmountMax = ((customerInfo.CreditLimit ?? 0) * (effectReasonApproveScaleMax.Value / _scale));
            }



            AdjustProcess adjustProcess = new AdjustProcess()
            {
                Id = customerInfo.AccountId,
                ApplyDate = currentTime.ToString("yyyy/MM/dd"),
                ApplyTime = currentTime.ToString("HH:mm:ss"),
                CustomerId = customerInfo.AccountId,
                CustomerName = customerInfo.ChineseName,
                CreditLimit = customerInfo.CreditLimit,
                ApplyAmount = newForwardData.ApplyAmount,
                UseSite = newForwardData.UseSite,
                Place = newForwardData.Place,
                AdjustDateStart = newForwardData.AdjustDateStart,
                AdjustDateEnd = newForwardData.AdjustDateEnd,
                Reason1 = newForwardData.Reason,
                Reason = newForwardData.Reason,
                Remark = newForwardData.Remark,
                ForceAuthenticate = newForwardData.ForceAuthenticate,
                ApproveAmountMax = tempApproveAmountMax,
                EstimateResult = newForwardData.EstimateResult,
                RejectReason = newForwardData.RejectReason,
                ApproveResult = "轉授信主管Pending",
                ChiefFlag = "Y",
                ChiefRemark = newForwardData.ChiefRemark,
                UserId = UserInfo.Id,
                UserName = UserInfo.Name,
                JcicDate = newForwardData.JcicDate,
                Type = "3",
                ProcessDate = currentTime.ToString("yyyy/MM/dd"),
                ProcessTime = currentTime.ToString("HH:mm:ss"),
                ProjectStatus = "Y",
                ProjectAdjustResult = newForwardData.ProjectAdjustResult,
                ProjectAdjustRejectReason = newForwardData.ProjectAdjustRejectReason,
                CreditAmount = newForwardData.CreditAmount,
            };

            currentTime = DateTime.Now;
            LogProgress logProgress = new LogProgress()
            {
                ApplicationNo = customerInfo.AccountId,
                ProgressCode = ConvertProgressCode(ProgressCode.ForwardingSupervisor),
                ProgressId = UserInfo.Id,
                ProgressName = UserInfo.Name,
                ProgressDate = currentTime.ToString("yyyyMMdd"),
                ProgressTime = currentTime.ToString("HHmmss"),
                Memo = ConvertProgressType(ProgressType.ProjectAdjust),

            };



            using (TransactionScope scope = new TransactionScope())
            {
                InsertAdjust(adjustProcess);
                InsertLogProgress(logProgress);
                scope.Complete();
            }
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
        private Customer GetCustomer(string customerId)
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
        /// <param name="preAdjustCondition">預審名單查詢條件</param>
        /// <returns></returns>
        private IEnumerable<PreAdjustEntity> GetEffectPreAdjust(PreAdjustCondition preAdjustCondition)
        {
            if (preAdjustCondition == null)
            {
                throw new ArgumentNullException(nameof(preAdjustCondition));
            }

            return new PreAdjustService().GetEffectPreAdjust(preAdjustCondition);
        }

        /// <summary>
        /// 是否為臨調生效中
        /// </summary>
        /// <param name="customer">客戶ID</param>
        /// <returns></returns>
        private bool HasAdjustEffecting(Customer customer)
        {
            bool hasEffect = false;

            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }


            if (customer.AdjustStartDate != null && customer.AdjustEndDate != null)
            {
                DateTime currentDate = DateTime.Today;

                DateTime startDate = DateTime.TryParseExact(customer.AdjustStartDate, "yyyyMMdd", null,
                    DateTimeStyles.None, out DateTime tempStartDate) ? tempStartDate :
                    throw new InvalidOperationException("Convert AdjustStartDate fail");

                DateTime endDate = DateTime.TryParseExact(customer.AdjustEndDate, "yyyyMMdd", null,
                    DateTimeStyles.None, out DateTime tempEndDate) ? tempEndDate :
                    throw new InvalidOperationException("Convert AdjustEndDate fail");

                hasEffect = ((currentDate >= startDate) && (currentDate <= endDate));
            }

            return hasEffect;
        }

        /// <summary>
        /// 檢核轉授信主管資料
        /// </summary>
        /// <param name="forwardData">轉授信主管資料</param>
        /// <returns></returns>
        private ForwardingSupervisor VerifyForwardingSupervisorData(ForwardingSupervisor forwardData)
        {
            if (forwardData == null)
            {
                throw new ArgumentNullException(nameof(forwardData));
            }



            DateTime currentDate = DateTime.Today;
            DateTime tempAdjustDateStart;
            DateTime tempAdjustDateEnd;

            if (String.IsNullOrEmpty(forwardData.CustomerId))
            {
                throw new InvalidOperationException($"{nameof(forwardData.CustomerId)} not found");
            }

            if (String.IsNullOrEmpty(forwardData.Reason))
            {
                throw new InvalidOperationException($"{nameof(forwardData.Reason)} not found");
            }
            else if (forwardData.Reason == _adjustReasonCodeByComprehensiveIncomeTax)
            {
                if (forwardData.CreditAmount == null)
                {
                    throw new InvalidOperationException($"{nameof(forwardData.CreditAmount)} not found");
                }
                else
                {
                    int creditAmountLength = forwardData.CreditAmount.Value.ToString().Length;

                    if ((creditAmountLength < _creaditAmountMinLength) ||
                        (creditAmountLength > _creaditAmountMaxLength))
                    {
                        throw new InvalidOperationException($"{nameof(forwardData.CreditAmount)}" +
                            $" Must enter {_creaditAmountMinLength}~{_creaditAmountMaxLength} digits amount");
                    }
                }
            }

            if (String.IsNullOrEmpty(forwardData.Remark))
            {
                throw new InvalidOperationException($"{nameof(forwardData.Remark)} not found");
            }

            if (String.IsNullOrEmpty(forwardData.JcicDate))
            {
                throw new InvalidOperationException($"{nameof(forwardData.JcicDate)} not found");
            }

            if (forwardData.ApplyAmount == null)
            {
                throw new InvalidOperationException($"{nameof(forwardData.ApplyAmount)} not found");
            }
            else
            {
                int applyAmount = forwardData.ApplyAmount.Value.ToString().Length;

                if ((applyAmount < _applyAmountMinLength) ||
                    (applyAmount > _applyAmountMaxLength))
                {
                    throw new InvalidOperationException($"{nameof(forwardData.ApplyAmount)}" +
                        $" Must enter {_applyAmountMinLength}~{_applyAmountMaxLength} digits amount");
                }
                else if (forwardData.ApplyAmount < forwardData.CreditAmount)
                {
                    throw new InvalidOperationException($"{forwardData.ApplyAmount}" +
                        $"Must be greater than or equal {forwardData.CreditAmount}");
                }
            }

            if (String.IsNullOrEmpty(forwardData.AdjustDateStart))
            {
                throw new InvalidOperationException($"{nameof(forwardData.AdjustDateStart)} not found");
            }
            else if (!DateTime.TryParse(forwardData.AdjustDateStart, out tempAdjustDateStart))
            {
                throw new InvalidOperationException($"Convert {nameof(forwardData.AdjustDateStart)} fail");
            }
            else if (tempAdjustDateStart < currentDate)
            {
                throw new InvalidOperationException($"{nameof(forwardData.AdjustDateStart)}" +
                    $" Must not be less than the system date");
            }

            if (String.IsNullOrEmpty(forwardData.AdjustDateEnd))
            {
                throw new InvalidOperationException($"{nameof(forwardData.AdjustDateEnd)} not found");
            }
            else if (!DateTime.TryParse(forwardData.AdjustDateEnd, out tempAdjustDateEnd))
            {
                throw new InvalidOperationException($"Convert {nameof(forwardData.AdjustDateEnd)} fail");
            }
            else if (tempAdjustDateStart > tempAdjustDateEnd)
            {
                throw new InvalidOperationException($"{nameof(forwardData.AdjustDateStart)}" +
                    $"No more than {nameof(forwardData.AdjustDateEnd)}");
            }

            if (String.IsNullOrEmpty(forwardData.ForceAuthenticate))
            {
                throw new InvalidOperationException($"{nameof(forwardData.ForceAuthenticate)} not found");
            }

            if (String.IsNullOrEmpty(forwardData.UseSite))
            {
                throw new InvalidOperationException($"{nameof(forwardData.UseSite)} not found");
            }
            else if (forwardData.UseSite != Enum.GetName(typeof(UseSite), UseSite.Domestic))
            {
                if (String.IsNullOrEmpty(forwardData.Place))
                {
                    throw new InvalidOperationException($"{nameof(forwardData.Place)} not found");
                }
            }

            if (String.IsNullOrEmpty(forwardData.ChiefRemark))
            {
                throw new InvalidOperationException($"{nameof(forwardData.ChiefRemark)} not found");
            }

            return new ForwardingSupervisor()
            {
                CustomerId = forwardData.CustomerId,
                ApplyAmount = forwardData.ApplyAmount,
                UseSite = forwardData.UseSite,
                Place = forwardData.Place,
                AdjustDateStart = tempAdjustDateStart.ToString("yyyy/MM/dd"),
                AdjustDateEnd = tempAdjustDateEnd.ToString("yyyy/MM/dd"),
                Reason = forwardData.Reason,
                Remark = forwardData.Remark,
                ForceAuthenticate = forwardData.ForceAuthenticate,
                EstimateResult = forwardData.EstimateResult,
                RejectReason = forwardData.RejectReason,
                ChiefRemark = forwardData.ChiefRemark,
                JcicDate = forwardData.JcicDate,
                ProjectAdjustResult = forwardData.ProjectAdjustResult,
                ProjectAdjustRejectReason = forwardData.ProjectAdjustRejectReason,
                CreditAmount = forwardData.CreditAmount,
            };
        }

        #endregion

        #region Insert/Update/Delete

        /// <summary>
        /// 新增臨調紀錄
        /// </summary>
        /// <param name="logProgress">臨調紀錄</param>
        private void InsertLogProgress(LogProgress logProgress)
        {
            if (logProgress == null)
            {
                throw new ArgumentNullException(nameof(logProgress));
            }

            LogProgressDO logProgressDO = ConvertLogProgressDO(logProgress);

            new LogProgressDAO().Insert(logProgressDO);
        }

        /// <summary>
        /// 新增臨調處理資訊
        /// </summary>
        /// <param name="adjustProcess">臨調處理資訊</param>
        private void InsertAdjust(AdjustProcess adjustProcess)
        {
            if (adjustProcess == null)
            {
                throw new ArgumentNullException(nameof(adjustProcess));
            }

            AdjustDO adjustDO = ConvertAdjustDO(adjustProcess);

            new AdjustDAO().Insert(adjustDO);
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
            return adjustList?.Select(x => new AdjustEntity()
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
        /// 轉換臨調Log紀錄
        /// </summary>
        /// <param name="logProgress">臨調Log紀錄</param>
        /// <returns></returns>
        private LogProgressDO ConvertLogProgressDO(LogProgress logProgress)
        {
            return (logProgress == null) ? null : new LogProgressDO()
            {
                ApplicationNo = logProgress.ApplicationNo,
                ApplicationKind = logProgress.ApplicationKind,
                ProgressCode = logProgress.ProgressCode,
                ProgressId = logProgress.ProgressId,
                ProgressName = logProgress.ProgressName,
                ProgressDate = logProgress.ProgressDate,
                ProgressTime = logProgress.ProgressTime,
                SerialNo = logProgress.SerialNo,
                Memo = logProgress.Memo,
            };
        }

        /// <summary>
        /// 轉換臨調處理資訊
        /// </summary>
        /// <param name="adjustProcess">臨調處理資訊</param>
        /// <returns></returns>
        private AdjustDO ConvertAdjustDO(AdjustProcess adjustProcess)
        {
            return (adjustProcess == null) ? null : new AdjustDO()
            {
                Id = adjustProcess.Id,
                ApplyDate = adjustProcess.ApplyDate,
                ApplyTime = adjustProcess.ApplyTime,
                CustomerId = adjustProcess.CustomerId,
                CustomerName = adjustProcess.CustomerName,
                CreditLimit = adjustProcess.CreditLimit,
                ApplyAmount = adjustProcess.ApplyAmount,
                UseSite = adjustProcess.UseSite,
                Place = adjustProcess.Place,
                AdjustDateStart = adjustProcess.AdjustDateStart,
                AdjustDateEnd = adjustProcess.AdjustDateEnd,
                Reason1 = adjustProcess.Reason1,
                Reason2 = adjustProcess.Reason2,
                Reason3 = adjustProcess.Reason3,
                Reason = adjustProcess.Reason,
                Remark = adjustProcess.Remark,
                ForceAuthenticate = adjustProcess.ForceAuthenticate,
                ApproveAmountMax = adjustProcess.ApproveAmountMax,
                UsabilityAmount = adjustProcess.UsabilityAmount,
                OverpayAmountPro = adjustProcess.OverpayAmountPro,
                ApproveAmount = adjustProcess.ApproveAmount,
                OverpayAmount = adjustProcess.OverpayAmount,
                EstimateResult = adjustProcess.EstimateResult,
                RejectReason = adjustProcess.RejectReason,
                ApproveResult = adjustProcess.ApproveResult,
                ChiefFlag = adjustProcess.ChiefFlag,
                ChiefRemark = adjustProcess.ChiefRemark,
                PendingFlag = adjustProcess.PendingFlag,
                UserId = adjustProcess.UserId,
                UserName = adjustProcess.UserName,
                ChiefId = adjustProcess.ChiefId,
                ChiefName = adjustProcess.ChiefName,
                JcicDate = adjustProcess.JcicDate,
                Type = adjustProcess.Type,
                CcasCode = adjustProcess.CcasCode,
                CcasStatus = adjustProcess.CcasStatus,
                CcasDateTime = adjustProcess.CcasDateTime,
                ProcessDate = adjustProcess.ProcessDate,
                ProcessTime = adjustProcess.ProcessTime,
                IcareStatus = adjustProcess.IcareStatus,
                ProjectStatus = adjustProcess.ProjectStatus,
                ProjectAdjustResult = adjustProcess.ProjectAdjustResult,
                ProjectAdjustRejectReason = adjustProcess.ProjectAdjustRejectReason,
                CreditAmount = adjustProcess.CreditAmount,
            };
        }

        /// <summary>
        /// 轉換處理代碼
        /// </summary>
        /// <param name="progressCode">處理代碼</param>
        /// <returns></returns>
        private string ConvertProgressCode(ProgressCode progressCode)
        {
            string resultCode = null;

            switch (progressCode)
            {
                default:
                case ProgressCode.None:
                    throw new InvalidOperationException($"{nameof(progressCode)} not found");
                    break;
                case ProgressCode.Verify:
                    resultCode = "B1";
                    break;
                case ProgressCode.ForwardingSupervisor:
                    resultCode = "B5";
                    break;
            }

            return resultCode;
        }

        /// <summary>
        /// 轉換處理類型
        /// </summary>
        /// <param name="progressType">處理類型</param>
        /// <returns></returns>
        private string ConvertProgressType(ProgressType progressType)
        {
            string resultType = null;

            switch (progressType)
            {
                default:
                case ProgressType.None:
                    throw new InvalidOperationException($"{nameof(progressType)} not found");
                    break;
                case ProgressType.PreAdjust:
                    resultType = "預審臨調";
                    break;
                case ProgressType.Adjust:
                    resultType = "臨調";
                    break;
                case ProgressType.ProjectAdjust:
                    resultType = "專案臨調";
                    break;
            }

            return resultType;
        }

        #endregion

        #endregion
    }
}
