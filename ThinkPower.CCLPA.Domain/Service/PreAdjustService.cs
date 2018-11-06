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

        #region Public Property

        #endregion



        #region Public Method


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



            PreAdjustValidateResult validateResult = Validate(campaignId);

            if (!String.IsNullOrEmpty(validateResult.ErrorMessage))
            {
                throw new InvalidOperationException("Import validate fail");
            }
            else if (validateResult.CampaignDetailCount == 0)
            {
                throw new InvalidOperationException("CampaignDetail not found");
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



            CampaignImportLogDO importLog = new CampaignImportLogDO()
            {
                CampaignId = campaignEntity.CampaignId,
                ExpectedStartDate = campaignEntity.ExpectedStartDateTime,
                ExpectedEndDate = campaignEntity.ExpectedEndDateTime,
                Count = campaignEntity.DetailList.Count(),
                ImportUserId = UserInfo.Id,
                ImportUserName = UserInfo.Name,
                ImportDate = DateTime.Now.ToString("yyyy/MM/dd"),
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

                    ImportDate = DateTime.Now.ToString("yyyy/MM/dd"),
                    Kind = campaignDetail.Col4,
                    Status = ConvertPreAdjustStatus(PreAdjustStatus.NotEffect),
                });
            }



            CustomerDAO customerDAO = new CustomerDAO();
            CustomerPartialInfoDO tempCustomerDO = null;
            PreAdjustDO tempPreAdjustDO = null;

            foreach (CampaignDetailEntity campaignDetail in campaignEntity.DetailList)
            {
                tempCustomerDO = customerDAO.GetPartialData(campaignDetail.CustomerId);

                if (tempCustomerDO == null)
                {
                    throw new InvalidOperationException("CustomerData not found");
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
            IEnumerable<PreAdjustEntity> preAdjustEntities = null;

            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }

            DataAccess.Condition.PreAdjustCondition preAdjustCondition =
                ConvertPreAdjustCondition(condition);

            IEnumerable<PreAdjustDO> preAdjustDOList = new PreAdjustDAO().Query(preAdjustCondition);

            preAdjustEntities = !preAdjustDOList.Any() ? new List<PreAdjustEntity>() :
                ConvertPreAdjustEntity(preAdjustDOList);

            return preAdjustEntities;
        }

        /// <summary>
        /// 查詢臨調預審名單筆數
        /// </summary>
        /// <param name="condition">預審名單資料查詢條件</param>
        /// <returns></returns>
        public int Count(Condition.PreAdjustCondition condition)
        {
            int result = 0;

            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }

            DataAccess.Condition.PreAdjustCondition preAdjustCondition =
                ConvertPreAdjustCondition(condition);

            result = new PreAdjustDAO().Count(preAdjustCondition);

            return result;
        }

        /// <summary>
        /// 刪除等待的臨調預審名單
        /// </summary>
        /// <param name="preAdjustInfo">來源資料</param>
        /// <returns>刪除預審名單筆數</returns>
        public PreAdjustResult DeleteNotEffect(PreAdjustInfo preAdjustInfo)
        {
            PreAdjustResult preAdjustResult = null;

            if (preAdjustInfo == null)
            {
                throw new ArgumentNullException("preAdjustInfo");
            }
            else if ((preAdjustInfo.PreAdjustList == null) || !preAdjustInfo.PreAdjustList.Any())
            {
                throw new ArgumentNullException("PreAdjustList");
            }
            else if (UserInfo == null)
            {
                throw new ArgumentNullException("UserInfo");
            }



            List<PreAdjustEntity> preAdjustList = GetPreAdjustEntities(preAdjustInfo.PreAdjustList);

            preAdjustList = preAdjustList.
                Where(x => x.Status != ConvertPreAdjustStatus(PreAdjustStatus.Effect)).ToList();

            if (preAdjustList.Count == 0)
            {
                throw new InvalidOperationException("PreAdjust not found");
            }



            List<PreAdjustResultInfo> preAdjustResultInfoList = new List<PreAdjustResultInfo>();

            foreach (PreAdjustEntity entity in preAdjustList)
            {
                try
                {
                    if (!String.IsNullOrEmpty(preAdjustInfo.Remark))
                    {
                        entity.Remark = preAdjustInfo.Remark;
                    }

                    entity.DeleteUserId = UserInfo.Id;
                    entity.DeleteDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Delete);

                    entity.Update();

                    preAdjustResultInfoList.Add(new PreAdjustResultInfo()
                    {
                        CustomerId = entity.CustomerId,
                        CampaignId = entity.CampaignId,
                        Status = entity.Status,
                    });
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }



            preAdjustResult = new PreAdjustResult()
            {
                PreAdjustList = preAdjustResultInfoList
            };

            return preAdjustResult;
        }

        /// <summary>
        /// 刪除生效的臨調預審名單
        /// </summary>
        /// <param name="preAdjustInfo">來源資料</param>
        /// <returns>刪除預審名單筆數</returns>
        public PreAdjustResult DeleteEffect(PreAdjustInfo preAdjustInfo)
        {
            PreAdjustResult preAdjustResult = null;

            if (preAdjustInfo == null)
            {
                throw new ArgumentNullException("preAdjustInfo");
            }
            else if ((preAdjustInfo.PreAdjustList == null) || !preAdjustInfo.PreAdjustList.Any())
            {
                throw new ArgumentNullException("PreAdjustList");
            }
            else if (UserInfo == null)
            {
                throw new ArgumentNullException("UserInfo");
            }



            List<PreAdjustEntity> preAdjustList = GetPreAdjustEntities(preAdjustInfo.PreAdjustList);

            preAdjustList = preAdjustList.
                Where(x => x.Status == ConvertPreAdjustStatus(PreAdjustStatus.Effect)).ToList();

            if (preAdjustList.Count == 0)
            {
                throw new InvalidOperationException("PreAdjust not found");
            }



            UserService userSeivice = new UserService() { UserInfo = UserInfo };
            string AccountByICRS = userSeivice.GetUserAccountByICRS();

            CreditSystemService creditSysService = new CreditSystemService();
            IncomeTaxCardAdjustInfo incomeTaxInfo = null;
            string incomeTaxResultCode = null;

            List<PreAdjustResultInfo> preAdjustResultInfoList = new List<PreAdjustResultInfo>();



            foreach (PreAdjustEntity entity in preAdjustList)
            {
                try
                {
                    incomeTaxInfo = new IncomeTaxCardAdjustInfo()
                    {
                        ActionCode = "D",
                        CustomerId = entity.CustomerId,
                        CustomerIdNo = (entity.CustomerId.Length > 10) ?
                                entity.CustomerId.Substring(10, 1) : String.Empty,

                        ProjectName = entity.ProjectName,
                        IncomeTaxAdjustAmount = entity.ProjectAmount,

                        AdjustCloseDate = DateTime.TryParseExact(entity.CloseDate, "yyyy/MM/dd",
                                null, DateTimeStyles.None, out DateTime tempCloseDate) ?
                                tempCloseDate.ToString("yyyyMMdd") :
                                throw new InvalidOperationException("Convert closeDate fail"),

                        AdjustUserId = AccountByICRS,
                    };

                    incomeTaxResultCode = creditSysService.IncomeTaxCardAdjust(incomeTaxInfo);

                    if (!String.IsNullOrEmpty(incomeTaxResultCode) && (incomeTaxResultCode == "00"))
                    {
                        entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Delete);

                        entity.CcasReplyDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        entity.CcasReplyCode = incomeTaxResultCode;
                        entity.CcasReplyStatus = creditSysService.
                            ConvertIncomeTaxResultCode(incomeTaxResultCode);

                        if (!String.IsNullOrEmpty(preAdjustInfo.Remark))
                        {
                            entity.Remark = preAdjustInfo.Remark;
                        }

                        entity.DeleteUserId = UserInfo.Id;
                        entity.DeleteDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                        entity.Update();
                    }

                    preAdjustResultInfoList.Add(new PreAdjustResultInfo()
                    {
                        CustomerId = entity.CustomerId,
                        CampaignId = entity.CampaignId,
                        Status = entity.Status,
                    });
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }



            preAdjustResult = new PreAdjustResult()
            {
                PreAdjustList = preAdjustResultInfoList,
            };

            return preAdjustResult;
        }

        /// <summary>
        /// 同意執行臨調預審名單
        /// </summary>
        /// <param name="preAdjustInfo">來源資料</param>
        /// <returns></returns>
        public PreAdjustResult Agree(PreAdjustInfo preAdjustInfo)
        {
            PreAdjustResult preAdjustResult = null;

            if (preAdjustInfo == null)
            {
                throw new ArgumentNullException("preAdjustInfo");
            }
            else if ((preAdjustInfo.PreAdjustList == null) || !preAdjustInfo.PreAdjustList.Any())
            {
                throw new ArgumentNullException("PreAdjustList");
            }
            else if (UserInfo == null)
            {
                throw new ArgumentNullException("UserInfo");
            }



            List<PreAdjustEntity> preAdjustList = GetPreAdjustEntities(preAdjustInfo.PreAdjustList);

            preAdjustList = preAdjustList.
                Where(x => x.Status == ConvertPreAdjustStatus(PreAdjustStatus.NotEffect)).ToList();

            if (preAdjustList.Count == 0)
            {
                throw new InvalidOperationException("preAdjustList not found");
            }



            AdjustSystemService adjustSysService = new AdjustSystemService();
            PreAdjustEffectResult effectResult = null;

            UserService customerService = new UserService() { UserInfo = UserInfo };
            string AccountByICRS = customerService.GetUserAccountByICRS();

            CreditSystemService creditSysService = new CreditSystemService();
            IncomeTaxCardAdjustInfo incomeTaxInfo = null;
            string incomeTaxResultCode = null;

            List<PreAdjustResultInfo> preAdjustResultInfoList = new List<PreAdjustResultInfo>();



            foreach (PreAdjustEntity entity in preAdjustList)
            {
                try
                {
                    effectResult = adjustSysService.PreAdjustEffect(entity.CustomerId);

                    if (effectResult.ResponseCode != "00")
                    {
                        entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Fail);
                        entity.RejectReasonCode = effectResult.RejectReason;
                    }
                    else
                    {
                        incomeTaxInfo = new IncomeTaxCardAdjustInfo()
                        {
                            ActionCode = "A",
                            CustomerId = entity.CustomerId,
                            CustomerIdNo = (entity.CustomerId.Length > 10) ?
                                entity.CustomerId.Substring(10, 1) : String.Empty,

                            ProjectName = entity.ProjectName,
                            IncomeTaxAdjustAmount = entity.ProjectAmount,

                            AdjustCloseDate = DateTime.TryParseExact(entity.CloseDate, "yyyy/MM/dd", null,
                                DateTimeStyles.None, out DateTime tempCloseDate) ?
                                tempCloseDate.ToString("yyyyMMdd") :
                                throw new InvalidOperationException("Convert closeDate fail"),

                            AdjustUserId = AccountByICRS,
                        };

                        incomeTaxResultCode = creditSysService.IncomeTaxCardAdjust(incomeTaxInfo);


                        if (String.IsNullOrEmpty(incomeTaxResultCode) || (incomeTaxResultCode != "00"))
                        {
                            entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Fail);
                        }
                        else
                        {
                            entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Effect);
                        }

                        entity.CcasReplyDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        entity.CcasReplyCode = incomeTaxResultCode;
                        entity.CcasReplyStatus = creditSysService.
                            ConvertIncomeTaxResultCode(incomeTaxResultCode);
                    }


                    entity.ProcessingUserId = UserInfo.Id;
                    entity.ProcessingDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    entity.Update();

                    preAdjustResultInfoList.Add(new PreAdjustResultInfo()
                    {
                        CustomerId = entity.CustomerId,
                        CampaignId = entity.CampaignId,
                        Status = entity.Status,
                    });
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }



            preAdjustResult = new PreAdjustResult()
            {
                PreAdjustList = preAdjustResultInfoList,
            };

            return preAdjustResult;
        }

        /// <summary>
        /// 強制同意臨調預審名單
        /// </summary>
        /// <param name="preAdjustInfo">來源資料</param>
        /// <param name="needValidate">是否需要驗證</param>
        /// <returns></returns>
        public PreAdjustResult ForceAgree(PreAdjustInfo preAdjustInfo, bool needValidate)
        {
            PreAdjustResult preAdjustResult = null;

            if (preAdjustInfo == null)
            {
                throw new ArgumentNullException("preAdjustInfo");
            }
            else if ((preAdjustInfo.PreAdjustList == null) || !preAdjustInfo.PreAdjustList.Any())
            {
                throw new ArgumentNullException("PreAdjustList");
            }
            else if (UserInfo == null)
            {
                throw new ArgumentNullException("UserInfo");
            }



            List<PreAdjustEntity> preAdjustList = GetPreAdjustEntities(preAdjustInfo.PreAdjustList);

            preAdjustList = preAdjustList.
                Where(x => x.Status == ConvertPreAdjustStatus(PreAdjustStatus.Fail)).ToList();

            if (preAdjustList.Count == 0)
            {
                throw new InvalidOperationException("preAdjustList not found");
            }



            AdjustSystemService adjustSysService = new AdjustSystemService();
            PreAdjustEffectResult effectResult = null;

            UserService customerService = new UserService() { UserInfo = UserInfo };
            string AccountByICRS = customerService.GetUserAccountByICRS();

            CreditSystemService creditSysService = new CreditSystemService();
            IncomeTaxCardAdjustInfo incomeTaxInfo = null;
            string incomeTaxResultCode = null;

            PreAdjustResultInfo resultInfo = null;

            List<PreAdjustResultInfo> preAdjustResultInfoList = new List<PreAdjustResultInfo>();


            bool executeCCAS = true;
            foreach (PreAdjustEntity entity in preAdjustList)
            {
                try
                {
                    executeCCAS = true;
                    if (needValidate)
                    {
                        effectResult = adjustSysService.PreAdjustEffect(entity.CustomerId);

                        if (effectResult.ResponseCode != "00")
                        {
                            entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Fail);
                            entity.RejectReasonCode = effectResult.RejectReason;

                            executeCCAS = false;
                        }
                    }

                    if (executeCCAS)
                    {
                        incomeTaxInfo = new IncomeTaxCardAdjustInfo()
                        {
                            ActionCode = "A",
                            CustomerId = entity.CustomerId,
                            CustomerIdNo = (entity.CustomerId.Length > 10) ?
                                entity.CustomerId.Substring(10, 1) : String.Empty,

                            ProjectName = entity.ProjectName,
                            IncomeTaxAdjustAmount = entity.ProjectAmount,

                            AdjustCloseDate = DateTime.TryParseExact(entity.CloseDate, "yyyy/MM/dd",
                                null, DateTimeStyles.None, out DateTime tempCloseDate) ?
                                tempCloseDate.ToString("yyyyMMdd") :
                                throw new InvalidOperationException("Convert closeDate fail"),

                            AdjustUserId = AccountByICRS,
                        };

                        incomeTaxResultCode = creditSysService.IncomeTaxCardAdjust(incomeTaxInfo);


                        if (String.IsNullOrEmpty(incomeTaxResultCode) || (incomeTaxResultCode != "00"))
                        {
                            entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Fail);
                        }
                        else
                        {
                            entity.Status = ConvertPreAdjustStatus(PreAdjustStatus.Effect);
                        }

                        entity.CcasReplyDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        entity.CcasReplyCode = incomeTaxResultCode;
                        entity.CcasReplyStatus = creditSysService.
                            ConvertIncomeTaxResultCode(incomeTaxResultCode);
                    }



                    entity.ForceAgreeUserId = UserInfo.Id;
                    entity.ProcessingDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    entity.Update();

                    resultInfo = new PreAdjustResultInfo()
                    {
                        CustomerId = entity.CustomerId,
                        CampaignId = entity.CampaignId,
                        Status = entity.Status,
                    };

                    if (needValidate && effectResult.ResponseCode != "00")
                    {
                        resultInfo.RejectReasonCode = entity.RejectReasonCode;
                    }

                    preAdjustResultInfoList.Add(resultInfo);
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }


            preAdjustResult = new PreAdjustResult()
            {
                PreAdjustList = preAdjustResultInfoList,
            };


            return preAdjustResult;
        }




        #endregion



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
        /// <param name="preAdjustList">預審名單資料</param>
        /// <returns></returns>
        private List<PreAdjustEntity> GetPreAdjustEntities(IEnumerable<PreAdjustResultInfo> preAdjustList)
        {
            List<PreAdjustEntity> preAdjustEntities = null;

            if ((preAdjustList == null) || !preAdjustList.Any())
            {
                throw new ArgumentNullException("preAdjustList");
            }


            PreAdjustDAO preAdjustDAO = new PreAdjustDAO();
            PreAdjustDO preAdjustDO = null;
            PreAdjustEntity preAdjustEntity = null;
            preAdjustEntities = new List<PreAdjustEntity>();


            foreach (PreAdjustResultInfo resultInfo in preAdjustList)
            {
                preAdjustDO = preAdjustDAO.Get(resultInfo.CustomerId, resultInfo.CampaignId);
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
                CustomerId = preAdjustDO.Id,
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
                CustomerId = x.Id,
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
                ForceAgreeUserId = x.ForceAgreeUserId,
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
                Id = preAdjustEntity.CustomerId,
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
                ForceAgreeUserId = preAdjustEntity.ForceAgreeUserId,
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
                Id = x.CustomerId,
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
                ForceAgreeUserId = x.ForceAgreeUserId,
                MobileTel = x.MobileTel,
                RejectReasonCode = x.RejectReasonCode,
                CcasReplyCode = x.CcasReplyCode,
                CcasReplyStatus = x.CcasReplyStatus,
                CcasReplyDateTime = x.CcasReplyDateTime,
            });
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
                CustomerId = condition.CustomerId,
                CampaignId = condition.CampaignId,
            };
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
        /// 轉換預審名單狀態
        /// </summary>
        /// <param name="status">預審名單狀態</param>
        /// <returns></returns>
        private PreAdjustStatus ConvertPreAdjustStatus(string status)
        {
            PreAdjustStatus? result = null;

            if (String.IsNullOrEmpty(status))
            {
                throw new ArgumentNullException("status");
            }


            switch (status)
            {
                case "待生效":
                    result = PreAdjustStatus.NotEffect;
                    break;
                case "生效中":
                    result = PreAdjustStatus.Effect;
                    break;
                case "失敗":
                    result = PreAdjustStatus.Fail;
                    break;
                case "刪除":
                    result = PreAdjustStatus.Delete;
                    break;
            }

            if (result == null)
            {
                throw new InvalidOperationException("PreAdjustStatus not found");
            }

            return result.Value;
        }

        #endregion
    }
}
