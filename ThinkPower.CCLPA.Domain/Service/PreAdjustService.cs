using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ThinkPower.CCLPA.DataAccess.Condition;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DAO.ICRS;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.ICRS;
using ThinkPower.CCLPA.Domain.Entity;
using ThinkPower.CCLPA.Domain.Service.Interface;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 臨調預審服務
    /// </summary>
    public class PreAdjustService : BaseService, IPreAdjust
    {
        #region Private Property

        private CampaignService _campaignService;

        /// <summary>
        /// 行銷活動服務
        /// </summary>
        private CampaignService CampaignService
        {
            get
            {
                if (_campaignService == null)
                {
                    _campaignService = new CampaignService();
                }

                return _campaignService;
            }
        }

        /// <summary>
        /// 臨調預審名單狀態列舉
        /// </summary>
        private enum PreAdjustStatus
        {
            /// <summary>
            /// 待生效
            /// </summary>
            NotEffect = 0,

            /// <summary>
            /// 生效中
            /// </summary>
            Effect,

            /// <summary>
            /// 失敗
            /// </summary>
            Fail,

            /// <summary>
            /// 刪除
            /// </summary>
            Delete
        }

        #endregion





        /// <summary>
        /// 檢核臨調預審名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns>預審名單驗證結果</returns>
        public PreAdjustValidateResult Validate(string campaignId)
        {
            PreAdjustValidateResult result = null;
            string errorMsg = null;
            int campaignDetailCount = 0;



            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }



            CampaignEntity campaignEntity = CampaignService.GetCampaign(campaignId);

            if (campaignEntity == null)
            {
                errorMsg = "ILRC行銷活動編碼，輸入錯誤。";
            }
            else if (!DateTime.TryParseExact(campaignEntity.ExpectedCloseDate, "yyyyMMdd", null,
                DateTimeStyles.None, out DateTime tempCloseDate))
            {
                throw new InvalidOperationException("Convert ExpectedCloseDate Fail");
            }
            else if (tempCloseDate < DateTime.Now.Date)
            {
                errorMsg = "此行銷活動已結案，無法進入匯入作業。";
            }



            if (String.IsNullOrEmpty(errorMsg))
            {
                CampaignImportLogEntity importLogEntity = GetImportLog(campaignEntity.CampaignId);

                if (importLogEntity != null)
                {
                    errorMsg = $"此行銷活動已於{importLogEntity.ImportDate}匯入過，無法再進行匯入。";
                }
                else
                {
                    campaignEntity.LoadDetailList();
                    campaignDetailCount = campaignEntity.DetailList.Count();
                }
            }



            result = new PreAdjustValidateResult()
            {
                ErrorMessage = errorMsg,
                CampaignDetailCount = campaignDetailCount,
            };

            return result;
        }

        /// <summary>
        /// 匯入臨調預審名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns></returns>
        public void Import(string campaignId)
        {
            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }
            else if (UserInfo == null)
            {
                throw new ArgumentNullException("UserInfo");
            }



            CampaignEntity campaignEntity = CampaignService.GetCampaign(campaignId);

            if (campaignEntity == null)
            {
                throw new InvalidOperationException("CampaignEntity not found");
            }



            campaignEntity.LoadDetailList();

            if ((campaignEntity.DetailList == null) || !campaignEntity.DetailList.Any())
            {
                throw new InvalidOperationException("CampaignDetailList not found");
            }



            DateTime currentTime = DateTime.Now;

            CampaignImportLogDO importLog = new CampaignImportLogDO()
            {
                CampaignId = campaignEntity.CampaignId,
                ExpectedStartDate = campaignEntity.ExpectedStartDateTime,
                ExpectedEndDate = campaignEntity.ExpectedEndDateTime,
                Count = campaignEntity.DetailList.Count(),
                ImportUserId = UserInfo.Id,
                ImportUserName = UserInfo.Name,
                ImportDate = currentTime.ToString("yyyy/MM/dd"),
            };

            List<PreAdjustDO> importPreAdjustList = new List<PreAdjustDO>();

            foreach (CampaignDetailEntity campaignDetail in campaignEntity.DetailList)
            {
                importPreAdjustList.Add(new PreAdjustDO()
                {
                    CampaignId = campaignEntity.CampaignId,
                    Id = campaignDetail.CustomerId,
                    ProjectName = campaignDetail.Col1,

                    ProjectAmount = String.IsNullOrEmpty(campaignDetail.Col2) ? null :
                        Decimal.TryParse(campaignDetail.Col2, out decimal tempAmount) ?
                        (decimal?)tempAmount :
                        throw new InvalidOperationException("Convert campaignDetail Col2 Fail"),

                    CloseDate = String.IsNullOrEmpty(campaignDetail.Col3) ? null :
                        DateTime.TryParseExact(campaignDetail.Col3, "yyyy/MM/dd", null,
                            DateTimeStyles.None, out DateTime tempCloseDate) ?
                        tempCloseDate.ToString("yyyy/MM/dd") :
                        throw new InvalidOperationException("Convert campaignDetail Col3 Fail"),

                    ImportDate = currentTime.ToString("yyyy/MM/dd"),
                    Kind = campaignDetail.Col4,
                    Status = ConvertPreAdjustStatus(PreAdjustStatus.NotEffect),
                });
            }



            CustomerDAO customerDAO = new CustomerDAO();
            CustomerShortDO tempCustomerDO = null;
            PreAdjustDO tempPreAdjustDO = null;

            foreach (CampaignDetailEntity campaignDetail in campaignEntity.DetailList)
            {
                tempCustomerDO = customerDAO.GetShortData(campaignDetail.CustomerId);

                if (tempCustomerDO == null)
                {
                    throw new InvalidOperationException("CustomerShortData not found");
                }

                tempPreAdjustDO = importPreAdjustList.
                    FirstOrDefault(x => x.Id == campaignDetail.CustomerId);

                if (tempPreAdjustDO == null)
                {
                    throw new InvalidOperationException("PreAdjustDO not found");
                }

                tempPreAdjustDO.ChineseName = tempCustomerDO.ChineseName;
                tempPreAdjustDO.ClosingDay = tempCustomerDO.ClosingDay;
                tempPreAdjustDO.PayDeadline = tempCustomerDO.PayDeadline;
                tempPreAdjustDO.MobileTel = tempCustomerDO.MobileTel;
            }



            using (TransactionScope scope = new TransactionScope())
            {
                new CampaignImportLogDAO().Insert(importLog);

                PreAdjustDAO preAdjustDAO = new PreAdjustDAO();

                foreach (PreAdjustDO preAdjustDO in importPreAdjustList)
                {
                    preAdjustDAO.Insert(preAdjustDO);
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// 查詢臨調預審名單
        /// </summary>
        /// <param name="condition">預審名單資料查詢條件</param>
        /// <returns></returns>
        public IEnumerable<PreAdjustEntity> Query(Condition.PreAdjustCondition condition)
        {
            IEnumerable<PreAdjustEntity> result = null;

            IEnumerable<PreAdjustDO> preAdjustDOList = null;

            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }

            DataAccess.Condition.PreAdjustCondition preAdjustCondition =
                ConvertPreAdjustCondition(condition);

            preAdjustDOList = new PreAdjustDAO().Get(preAdjustCondition);


            result = (preAdjustDOList == null) ? null :
                ConvertPreAdjustEntity(preAdjustDOList);


            return result;
        }

        /// <summary>
        /// 刪除等待的臨調預審名單
        /// </summary>
        /// <param name="preAdjustInfo">來源資料</param>
        /// <returns>刪除預審名單筆數</returns>
        public int DeleteNotEffect(PreAdjustInfo preAdjustInfo)
        {
            int deleteCount = 0;

            if (preAdjustInfo == null)
            {
                throw new ArgumentNullException("preAdjustInfo");
            }
            else if ((preAdjustInfo.PreAdjustList == null) || !preAdjustInfo.PreAdjustList.Any())
            {
                throw new ArgumentNullException("PreAdjustList");
            }
            else if (preAdjustInfo.Condition == null)
            {
                throw new AggregateException("PreAdjustCondition");
            }
            else if (UserInfo == null)
            {
                throw new ArgumentNullException("UserInfo");
            }



            DataAccess.Condition.PreAdjustCondition preAdjustCondition =
                ConvertPreAdjustCondition(preAdjustInfo.Condition);

            List<PreAdjustEntity> preAdjustList = GetPreAdjustEntities(preAdjustCondition,
                preAdjustInfo.PreAdjustList);



            DateTime currentTime = DateTime.Now;

            foreach (PreAdjustEntity entity in preAdjustList)
            {
                if (!String.IsNullOrEmpty(preAdjustInfo.Remark))
                {
                    entity.Remark = preAdjustInfo.Remark;
                }

                entity.DeleteUserId = UserInfo.Id;
                entity.DeleteDateTime = currentTime.ToString("yyyy/MM/dd HH:mm:ss");
                entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Delete);
            }



            try
            {
                foreach (PreAdjustEntity preAdjust in preAdjustList)
                {
                    preAdjust.Update();
                    deleteCount++;
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }


            deleteCount = preAdjustList.Count;


            return deleteCount;
        }

        /// <summary>
        /// 刪除生效的臨調預審名單
        /// </summary>
        /// <param name="preAdjustInfo">來源資料</param>
        /// <returns>刪除預審名單筆數</returns>
        public int DeleteEffect(PreAdjustInfo preAdjustInfo)
        {
            int deleteCount = 0;

            if (preAdjustInfo == null)
            {
                throw new ArgumentNullException("preAdjustInfo");
            }
            else if ((preAdjustInfo.PreAdjustList == null) || !preAdjustInfo.PreAdjustList.Any())
            {
                throw new ArgumentNullException("PreAdjustList");
            }
            else if (preAdjustInfo.Condition == null)
            {
                throw new AggregateException("PreAdjustCondition");
            }
            else if (UserInfo == null)
            {
                throw new ArgumentNullException("UserInfo");
            }



            DataAccess.Condition.PreAdjustCondition preAdjustCondition =
                ConvertPreAdjustCondition(preAdjustInfo.Condition);

            List<PreAdjustEntity> preAdjustList = GetPreAdjustEntities(preAdjustCondition,
                preAdjustInfo.PreAdjustList);



            DateTime currentTime = DateTime.Now;

            foreach (PreAdjustEntity entity in preAdjustList)
            {
                if (!String.IsNullOrEmpty(preAdjustInfo.Remark))
                {
                    entity.Remark = preAdjustInfo.Remark;
                }

                entity.DeleteUserId = UserInfo.Id;
                entity.DeleteDateTime = currentTime.ToString("yyyy/MM/dd HH:mm:ss");
                entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Delete);
            }



            AdjustSystemService adjustSysService = new AdjustSystemService();
            PreAdjustEffectResult effectResult = null;
            int validateFailCount = 0;


            try
            {
                foreach (PreAdjustEntity entity in preAdjustList)
                {
                    effectResult = adjustSysService.PreAdjustEffect(entity.Id);

                    if (effectResult.ResponseCode != "00")
                    {
                        entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Fail);
                        validateFailCount++;
                    }

                    entity.Update();
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }


            deleteCount = (preAdjustList.Count() - validateFailCount);


            return deleteCount;
        }

        /// <summary>
        /// 同意執行臨調預審名單
        /// </summary>
        /// <param name="preAdjustInfo">來源資料</param>
        /// <returns></returns>
        public PreAdjustAgreeResult Agree(PreAdjustInfo preAdjustInfo)
        {
            PreAdjustAgreeResult result = null;

            if (preAdjustInfo == null)
            {
                throw new ArgumentNullException("preAdjustInfo");
            }
            else if ((preAdjustInfo.PreAdjustList == null) || !preAdjustInfo.PreAdjustList.Any())
            {
                throw new ArgumentNullException("PreAdjustList");
            }
            else if (preAdjustInfo.Condition == null)
            {
                throw new AggregateException("PreAdjustCondition");
            }
            else if (UserInfo == null)
            {
                throw new ArgumentNullException("UserInfo");
            }



            DataAccess.Condition.PreAdjustCondition preAdjustCondition =
                ConvertPreAdjustCondition(preAdjustInfo.Condition);

            List<PreAdjustEntity> preAdjustList = GetPreAdjustEntities(preAdjustCondition,
                preAdjustInfo.PreAdjustList);


            preAdjustList = preAdjustList.
                Where(x => x.Status == ConvertPreAdjustStatus(PreAdjustStatus.NotEffect)).ToList();


            DateTime currentTime = DateTime.Now;

            AdjustSystemService adjustSysService = new AdjustSystemService();
            PreAdjustEffectResult effectResult = null;
            int validateFailCount = 0;

            AdjustService adjustService = new AdjustService();
            adjustService.UserInfo = UserInfo;

            IncomeTaxCardAdjustInfo cardAdjustInfo = null;
            CreditSystemService creditSysService = new CreditSystemService();
            string IncomeTaxResultCode = null;


            try
            {
                foreach (PreAdjustEntity entity in preAdjustList)
                {

                    effectResult = adjustSysService.PreAdjustEffect(entity.Id);

                    if (effectResult.ResponseCode != "00")
                    {
                        entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Fail);
                        entity.RejectReasonCode = effectResult.RejectReason;

                        validateFailCount++;
                    }
                    else
                    {

                        cardAdjustInfo = new IncomeTaxCardAdjustInfo()
                        {
                            ActionCode = "A",
                            CustomerId = entity.Id,
                            CustomerIdNo = (entity.Id.Length > 10) ? entity.Id.Substring(10, 1) : String.Empty,
                            ProjectName = entity.ProjectName,
                            IncomeTaxAdjustAmount = entity.ProjectAmount,

                            AdjustCloseDate = DateTime.TryParseExact(entity.CloseDate, "yyyy/MM/dd", null,
                                DateTimeStyles.None, out DateTime tempCloseDate) ?
                                tempCloseDate.ToString("yyyyMMdd") :
                                throw new InvalidOperationException("Convert closeDate fail"),

                            AdjustUserId = adjustService.GetUserAccountByICRS(),
                        };

                        IncomeTaxResultCode = creditSysService.IncomeTaxCardAdjust(cardAdjustInfo);



                        if (String.IsNullOrEmpty(IncomeTaxResultCode) || (IncomeTaxResultCode != "00"))
                        {
                            entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Fail);
                            entity.CcasReplyCode = IncomeTaxResultCode;

                            validateFailCount++;
                        }
                        else
                        {
                            entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Effect);
                        }
                    }

                    entity.DeleteUserId = UserInfo.Id;
                    entity.DeleteDateTime = currentTime.ToString("yyyy/MM/dd HH:mm:ss");

                    entity.Update();
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }



            result = new PreAdjustAgreeResult()
            {
                EffectCount = (preAdjustList.Count - validateFailCount),
                FailCount = validateFailCount,
            };



            return result;
        }

        /// <summary>
        /// 強制同意臨調預審名單
        /// </summary>
        /// <param name="preAdjustInfo">來源資料</param>
        /// <param name="forcedConsentFailCase">強制同意失敗案件</param>
        /// <returns></returns>
        public PreAdjustForcedConsentResult ForcedConsent(PreAdjustInfo preAdjustInfo,
            bool forcedConsentFailCase)
        {
            PreAdjustForcedConsentResult result = null;

            if (preAdjustInfo == null)
            {
                throw new ArgumentNullException("preAdjustInfo");
            }
            else if ((preAdjustInfo.PreAdjustList == null) || !preAdjustInfo.PreAdjustList.Any())
            {
                throw new ArgumentNullException("PreAdjustList");
            }
            else if (preAdjustInfo.Condition == null)
            {
                throw new AggregateException("PreAdjustCondition");
            }
            else if (UserInfo == null)
            {
                throw new ArgumentNullException("UserInfo");
            }



            DataAccess.Condition.PreAdjustCondition preAdjustCondition =
                ConvertPreAdjustCondition(preAdjustInfo.Condition);

            List<PreAdjustEntity> preAdjustList = GetPreAdjustEntities(preAdjustCondition,
                preAdjustInfo.PreAdjustList);



            DateTime currentTime = DateTime.Now;

            AdjustSystemService adjustSysService = new AdjustSystemService();
            PreAdjustEffectResult effectResult = null;

            List<ForcedConsentFailResult> FailResultList = new List<ForcedConsentFailResult>();



            /*
                 檢查案件狀態是否為「失敗」
                     進行「生效狀態檢核」
                        檢核結果:
                            成功(生效中) 
                                >> CALL CCAS 
                                    >> 成功 >> Update 狀態(生效中)
                                    >> 失敗 >> Update 狀態(失敗), 失敗原因, 失敗代碼

                            失敗  >> Update 狀態(失敗), 失敗原因, 失敗代碼
             */

            try
            {
                foreach (PreAdjustEntity entity in preAdjustList)
                {
                    effectResult = adjustSysService.PreAdjustEffect(entity.Id);

                    if (effectResult.ResponseCode != "00")
                    {
                        FailResultList.Add(new ForcedConsentFailResult()
                        {
                            Id = entity.Id,
                            CampaignId = entity.CampaignId,
                            RejectReason = effectResult.RejectReason,
                            ResponseCode = effectResult.ResponseCode,
                        });
                    }
                    else
                    {
                        entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Effect);
                        entity.DeleteUserId = UserInfo.Id;
                        entity.DeleteDateTime = currentTime.ToString("yyyy/MM/dd HH:mm:ss");
                        entity.Update();

                        // TODO CCAS Stored Procedure
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }



            result = new PreAdjustForcedConsentResult()
            {
                EffectCount = (preAdjustList.Count - FailResultList.Count),
                FailResultList = FailResultList,
            };



            return result;
        }


        #region InternalMethod


        /// <summary>
        /// 更新預審名單資料
        /// </summary>
        /// <param name="preAdjust">預審名單資料</param>
        internal void Update(PreAdjustEntity preAdjust)
        {
            PreAdjustDO preAdjustDO = ConvertPreAdjustDO(preAdjust);
            new PreAdjustDAO().Update(preAdjustDO);
        }


        #endregion






        #region PrivateMethod

        /// <summary>
        /// 取得行銷活動匯入紀錄
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns></returns>
        private CampaignImportLogEntity GetImportLog(string campaignId)
        {
            CampaignImportLogEntity campaignImportLogEntity = null;

            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }

            CampaignImportLogDO campaignImportLogDO = new CampaignImportLogDAO().Get(campaignId);

            if (campaignImportLogDO != null)
            {
                campaignImportLogEntity = new CampaignImportLogEntity()
                {
                    CampaignId = campaignImportLogDO.CampaignId,
                    ExpectedStartDate = campaignImportLogDO.ExpectedStartDate,
                    ExpectedEndDate = campaignImportLogDO.ExpectedEndDate,
                    Count = campaignImportLogDO.Count,
                    ImportUserId = campaignImportLogDO.ImportUserId,
                    ImportUserName = campaignImportLogDO.ImportUserName,
                    ImportDate = campaignImportLogDO.ImportDate,
                };
            }

            return campaignImportLogEntity;
        }

        /// <summary>
        /// 取得預審名單
        /// </summary>
        /// <param name="preAdjustCondition">預審名單資料查詢條件</param>
        /// <param name="preAdjustList">預審名單資料</param>
        /// <returns></returns>
        private List<PreAdjustEntity> GetPreAdjustEntities(PreAdjustCondition preAdjustCondition,
            IEnumerable<PreAdjustEntity> preAdjustList)
        {
            List<PreAdjustEntity> preAdjustEntities = null;

            if (preAdjustCondition == null)
            {
                throw new ArgumentNullException("preAdjustCondition");
            }
            else if ((preAdjustList == null) || !preAdjustList.Any())
            {
                throw new ArgumentNullException("preAdjustList");
            }



            PreAdjustDAO preAdjustDAO = new PreAdjustDAO();
            IEnumerable<PreAdjustDO> preAdjustDOList = null;
            PreAdjustDO preAdjustDO = null;
            PreAdjustEntity preAdjustEntity = null;
            preAdjustEntities = new List<PreAdjustEntity>();



            foreach (PreAdjustEntity entity in preAdjustList)
            {
                preAdjustCondition.Id = entity.Id;
                preAdjustCondition.CampaignId = entity.CampaignId;

                preAdjustDOList = preAdjustDAO.Get(preAdjustCondition);

                if (preAdjustDOList == null || !preAdjustDOList.Any())
                {
                    throw new InvalidOperationException("preAdjustDOList not found");
                }
                else if (preAdjustDOList.Count() > 1)
                {
                    throw new InvalidOperationException("preAdjustDOList not the only");
                }

                preAdjustDO = preAdjustDOList.First();
                preAdjustEntity = ConvertPreAdjustEntity(preAdjustDO);
                preAdjustEntities.Add(preAdjustEntity);
            }



            return preAdjustEntities;
        }





        /// <summary>
        /// 轉換臨調預審名單資料
        /// </summary>
        /// <param name="preAdjustDO">臨調預審名單資料</param>
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
                Id = preAdjustDO.Id,
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
                AgreeUserId = preAdjustDO.AgreeUserId,
                MobileTel = preAdjustDO.MobileTel,
                RejectReasonCode = preAdjustDO.RejectReasonCode,
                CcasReplyCode = preAdjustDO.CcasReplyCode,
                CcasReplyStatus = preAdjustDO.CcasReplyStatus,
                CcasReplyDateTime = preAdjustDO.CcasReplyDateTime,
            };
        }

        /// <summary>
        /// 轉換臨調預審名單資料
        /// </summary>
        /// <param name="preAdjustDOList">臨調預審名單資料</param>
        /// <returns></returns>
        private IEnumerable<PreAdjustEntity> ConvertPreAdjustEntity(IEnumerable<PreAdjustDO> preAdjustDOList)
        {
            if (preAdjustDOList == null || !preAdjustDOList.Any())
            {
                throw new ArgumentNullException("preAdjustList");
            }

            return preAdjustDOList.Select(x => new PreAdjustEntity()
            {
                CampaignId = x.CampaignId,
                Id = x.Id,
                ProjectName = x.ProjectName,
                ProjectAmount = x.ProjectAmount,
                CloseDate = x.CloseDate,
                ImportDate = x.ImportDate,
                ChineseName = x.ChineseName,
                Kind = x.Kind,
                SmsCheckResult = x.SmsCheckResult,
                Status = x.Status,
                ProcessingDateTime = x.ProcessingDateTime,
                ProcessingUserId = x.ProcessingUserId,
                DeleteDateTime = x.DeleteDateTime,
                DeleteUserId = x.DeleteUserId,
                Remark = x.Remark,
                ClosingDay = x.ClosingDay,
                PayDeadline = x.PayDeadline,
                AgreeUserId = x.AgreeUserId,
                MobileTel = x.MobileTel,
                RejectReasonCode = x.RejectReasonCode,
                CcasReplyCode = x.CcasReplyCode,
                CcasReplyStatus = x.CcasReplyStatus,
                CcasReplyDateTime = x.CcasReplyDateTime,
            });
        }



        /// <summary>
        /// 轉換臨調預審名單資料
        /// </summary>
        /// <param name="preAdjustEntity">臨調預審名單資料</param>
        /// <returns></returns>
        private PreAdjustDO ConvertPreAdjustDO(PreAdjustEntity preAdjustEntity)
        {
            if (preAdjustEntity == null)
            {
                throw new ArgumentNullException("preAdjust");
            }

            return new PreAdjustDO()
            {
                CampaignId = preAdjustEntity.CampaignId,
                Id = preAdjustEntity.Id,
                ProjectName = preAdjustEntity.ProjectName,
                ProjectAmount = preAdjustEntity.ProjectAmount,
                CloseDate = preAdjustEntity.CloseDate,
                ImportDate = preAdjustEntity.ImportDate,
                ChineseName = preAdjustEntity.ChineseName,
                Kind = preAdjustEntity.Kind,
                SmsCheckResult = preAdjustEntity.SmsCheckResult,
                Status = preAdjustEntity.Status,
                ProcessingDateTime = preAdjustEntity.ProcessingDateTime,
                ProcessingUserId = preAdjustEntity.ProcessingUserId,
                DeleteDateTime = preAdjustEntity.DeleteDateTime,
                DeleteUserId = preAdjustEntity.DeleteUserId,
                Remark = preAdjustEntity.Remark,
                ClosingDay = preAdjustEntity.ClosingDay,
                PayDeadline = preAdjustEntity.PayDeadline,
                AgreeUserId = preAdjustEntity.AgreeUserId,
                MobileTel = preAdjustEntity.MobileTel,
                RejectReasonCode = preAdjustEntity.RejectReasonCode,
                CcasReplyCode = preAdjustEntity.CcasReplyCode,
                CcasReplyStatus = preAdjustEntity.CcasReplyStatus,
                CcasReplyDateTime = preAdjustEntity.CcasReplyDateTime,
            };
        }

        /// <summary>
        /// 轉換臨調預審名單資料
        /// </summary>
        /// <param name="preAdjustEntities">臨調預審名單資料</param>
        /// <returns></returns>
        private IEnumerable<PreAdjustDO> ConvertPreAdjustDO(IEnumerable<PreAdjustEntity> preAdjustEntities)
        {
            if ((preAdjustEntities == null) || !preAdjustEntities.Any())
            {
                throw new ArgumentNullException("preAdjust");
            }

            return preAdjustEntities.Select(x => new PreAdjustDO()
            {
                CampaignId = x.CampaignId,
                Id = x.Id,
                ProjectName = x.ProjectName,
                ProjectAmount = x.ProjectAmount,
                CloseDate = x.CloseDate,
                ImportDate = x.ImportDate,
                ChineseName = x.ChineseName,
                Kind = x.Kind,
                SmsCheckResult = x.SmsCheckResult,
                Status = x.Status,
                ProcessingDateTime = x.ProcessingDateTime,
                ProcessingUserId = x.ProcessingUserId,
                DeleteDateTime = x.DeleteDateTime,
                DeleteUserId = x.DeleteUserId,
                Remark = x.Remark,
                ClosingDay = x.ClosingDay,
                PayDeadline = x.PayDeadline,
                AgreeUserId = x.AgreeUserId,
                MobileTel = x.MobileTel,
                RejectReasonCode = x.RejectReasonCode,
                CcasReplyCode = x.CcasReplyCode,
                CcasReplyStatus = x.CcasReplyStatus,
                CcasReplyDateTime = x.CcasReplyDateTime,
            });
        }





        /// <summary>
        /// 轉換預審名單狀態
        /// </summary>
        /// <param name="status">預審名單狀態</param>
        /// <returns></returns>
        private string ConvertPreAdjustStatus(PreAdjustStatus status)
        {
            string result = null;

            switch (status)
            {
                case PreAdjustStatus.NotEffect:
                    result = "待生效";
                    break;
                case PreAdjustStatus.Effect:
                    result = "生效中";
                    break;
                case PreAdjustStatus.Fail:
                    result = "失敗";
                    break;
                case PreAdjustStatus.Delete:
                    result = "刪除";
                    break;
            }

            return result;
        }

        /// <summary>
        /// 轉換臨調預審名單查詢條件
        /// </summary>
        /// <param name="condition">臨調預審名單查詢條件</param>
        /// <returns></returns>
        private DataAccess.Condition.PreAdjustCondition ConvertPreAdjustCondition(
            Condition.PreAdjustCondition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }

            return new DataAccess.Condition.PreAdjustCondition()
            {
                PageIndex = condition.PageIndex,
                PagingSize = condition.PagingSize,
                CloseDate = condition.CloseDate,
                CcasReplyCode = condition.CcasReplyCode,
                Id = condition.Id,
                CampaignId = condition.CampaignId,
            };
        }




        #endregion
    }
}
