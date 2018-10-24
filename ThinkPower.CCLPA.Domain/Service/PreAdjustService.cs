using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DAO.ICRS;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CMPN;
using ThinkPower.CCLPA.DataAccess.DO.ICRS;
using ThinkPower.CCLPA.Domain.Entity;
using ThinkPower.CCLPA.Domain.Service.Interface;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 臨調預審服務
    /// </summary>
    public class PreAdjustService : IPreAdjust
    {
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
        /// 使用者資訊
        /// </summary>
        public UserInfoVO UserInfo { get; set; }

        /// <summary>
        /// 預審名單狀態列舉
        /// </summary>
        private enum PreAdjustStatus { 待生效, 生效中, 失敗, 刪除 }




        /// <summary>
        /// 匯入臨調預審名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <param name="executeImport">是否執行匯入</param>
        /// <returns>行銷活動名單數量</returns>
        public int? Import(string campaignId, bool executeImport = false)
        {
            int? campaignDetailCount = null;

            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }




            CampaignEntity campaignEntity = CampaignService.GetCampaign(campaignId);

            if (campaignEntity == null)
            {
                var e = new InvalidOperationException("Campaign not found");
                e.Data["ErrorMsg"] = "ILRC行銷活動編碼，輸入錯誤。";
                throw e;

            }
            else if (!DateTime.TryParseExact(campaignEntity.ExpectedCloseDate, "yyyyMMdd", null,
                DateTimeStyles.None, out DateTime tempCloseDate))
            {
                throw new InvalidOperationException("Convert ExpectedCloseDate Fail");
            }
            else if (tempCloseDate < DateTime.Now.Date)
            {
                var e = new InvalidOperationException("Campaign closed, not can't import data");
                e.Data["ErrorMsg"] = "此行銷活動已結案，無法進入匯入作業。";
                throw e;
            }




            CampaignImportLogEntity importLogEntity = GetImportLog(campaignEntity.CampaignId);

            if (importLogEntity != null)
            {
                var e = new InvalidOperationException("Campaign imported, not can't again import");
                e.Data["ErrorMsg"] = $"此行銷活動已於{importLogEntity.ImportDate}匯入過，無法再進行匯入。";
                throw e;
            }




            if (!executeImport)
            {
                campaignDetailCount = campaignEntity.GetDetailCount();

                return campaignDetailCount;
            }



            campaignEntity.LoadDetailList();

            IEnumerable<CampaignDetailEntity> campaignDetailList = campaignEntity.DetailList;

            if ((campaignDetailList == null) || (campaignDetailList.Count() == 0))
            {
                throw new InvalidOperationException("CampaignDetailList not found");
            }




            DateTime currentTime = DateTime.Now;

            if (UserInfo == null)
            {
                throw new InvalidOperationException("UserInfo not found");
            }

            CampaignImportLogDO importLog = new CampaignImportLogDO()
            {
                CampaignId = campaignEntity.CampaignId,
                ExpectedStartDate = campaignEntity.ExpectedStartDateTime,
                ExpectedEndDate = campaignEntity.ExpectedEndDateTime,
                Count = campaignDetailList.Count(),
                ImportUserId = UserInfo.Id,
                ImportUserName = UserInfo.Name,
                ImportDate = currentTime.ToString("yyyy/MM/dd"),
            };

            List<PreAdjustDO> preAdjustList = new List<PreAdjustDO>();

            foreach (CampaignDetailEntity campaignDetail in campaignDetailList)
            {
                preAdjustList.Add(new PreAdjustDO()
                {
                    CampaignId = campaignEntity.CampaignId,
                    Id = campaignDetail.CustomerId,
                    ProjectName = campaignDetail.Col1,
                    ProjectAmount = Convert.ToDecimal(campaignDetail.Col2),
                    CloseDate = DateTime.TryParseExact(campaignDetail.Col3, "yyyyMMdd", null,
                        DateTimeStyles.None, out DateTime temp) ?
                        temp.ToString("yyyy/MM/dd") :
                        throw new InvalidOperationException("Convert campaignDetail Col3 Fail"),
                    ImportDate = currentTime.ToString("yyyy/MM/dd"),
                    Kind = campaignDetail.Col4,
                    Status = Enum.GetName(typeof(PreAdjustStatus), PreAdjustStatus.待生效),
                });
            }




            CustomerDAO customerDAO = new CustomerDAO();
            CustomerShortDO customerShortData = null;
            PreAdjustDO tempPreAdjust = null;

            foreach (CampaignDetailEntity campaignDetail in campaignDetailList)
            {
                customerShortData = customerDAO.GetShortData(campaignDetail.CustomerId);

                if (customerShortData == null)
                {
                    throw new InvalidOperationException("CustomerShortData not found");
                }

                tempPreAdjust = preAdjustList.FirstOrDefault(x => x.Id == campaignDetail.CustomerId);

                if (tempPreAdjust == null)
                {
                    throw new InvalidOperationException("tempPreAdjust not found");
                }

                tempPreAdjust.ChineseName = customerShortData.ChineseName;
                tempPreAdjust.ClosingDay = customerShortData.ClosingDay;
                tempPreAdjust.PayDeadline = customerShortData.PayDeadline;
                tempPreAdjust.MobileTel = customerShortData.MobileTel;
            }



            SaveCampaignData(importLog, preAdjustList);



            return campaignDetailCount;
        }


        /// <summary>
        /// 查詢臨調預審名單
        /// </summary>
        /// <param name="id">身分證字號</param>
        /// <returns></returns>
        public PreAdjustInfoEntity Search(string id)
        {
            PreAdjustInfoEntity result = null;

            IEnumerable<PreAdjustDO> waitZoneData = null;
            IEnumerable<PreAdjustDO> effectZoneData = null;

            if (String.IsNullOrEmpty(id))
            {
                waitZoneData = new PreAdjustDAO().GetAllWaitData();
                effectZoneData = new PreAdjustDAO().GetAllEffectData();
            }
            else
            {
                waitZoneData = new PreAdjustDAO().GetWaitData(id);
                effectZoneData = new PreAdjustDAO().GetEffectData(id);

                if (((waitZoneData == null) || (waitZoneData.Count() == 0)) &&
                    ((effectZoneData == null) || (effectZoneData.Count() == 0)))
                {
                    var e = new InvalidOperationException("PreAdjust not found");
                    e.Data["ErrorMsg"] = "查無相關資料，請確認是否有輸入錯誤。";
                    throw e;
                }
            }


            result = new PreAdjustInfoEntity()
            {
                WaitZone = ConvertPreAdjustEntity(waitZoneData),
                EffectZone = ConvertPreAdjustEntity(effectZoneData),
            };


            return result;
        }


        /// <summary>
        /// 刪除臨調預審名單
        /// </summary>
        /// <param name="preAdjustInfo">來源資料</param>
        /// <param name="isWaitZone">是否為等待區</param>
        /// <returns>刪除預審名單筆數</returns>
        public int? Delete(PreAdjustInfoEntity preAdjustInfo, bool isWaitZone)
        {
            int? result = null;

            if (preAdjustInfo == null)
            {
                throw new ArgumentNullException("data");
            }


            if (isWaitZone)
            {
                if ((preAdjustInfo.WaitZone == null) || (preAdjustInfo.WaitZone.Count() == 0))
                {
                    var e = new InvalidOperationException("PreAdjustWaitZone not found");
                    e.Data["ErrorMsg"] = "請先於《等待區中》勾選資料後，再進行後續作業。";
                    throw e;
                }

                PreAdjustDAO preAdjustDAO = new PreAdjustDAO();
                List<PreAdjustEntity> preAdjustList = new List<PreAdjustEntity>();
                PreAdjustEntity preAdjustEntity = null;
                PreAdjustDO preAdjustDO = null;

                foreach (PreAdjustEntity waitItem in preAdjustInfo.WaitZone)
                {
                    preAdjustDO = preAdjustDAO.GetWaitData(waitItem.CampaignId, waitItem.Id);
                    preAdjustEntity = ConvertPreAdjustEntity(preAdjustDO);

                    preAdjustList.Add(preAdjustEntity);
                }

                if (UserInfo == null)
                {
                    throw new InvalidOperationException("UserInfo not found");
                }

                DateTime currentTime = DateTime.Now;

                foreach (PreAdjustEntity waitItem in preAdjustList)
                {
                    if (!String.IsNullOrEmpty(preAdjustInfo.Remark))
                    {
                        waitItem.Remark = preAdjustInfo.Remark;
                    }

                    waitItem.DeleteUserId = UserInfo.Id;
                    waitItem.DeleteDateTime = currentTime.ToString("yyyy/MM/dd HH:mm:ss");
                    waitItem.Status = Enum.GetName(typeof(PreAdjustStatus), PreAdjustStatus.刪除);
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (PreAdjustEntity preAdjust in preAdjustList)
                    {
                        preAdjust.Update();
                    }

                    scope.Complete();
                }

                result = preAdjustList.Count;
            }
            else
            {
                if ((preAdjustInfo.EffectZone == null) || (preAdjustInfo.EffectZone.Count() == 0))
                {
                    var e = new InvalidOperationException("PreAdjustEffectZone not found");
                    e.Data["ErrorMsg"] = "請先於《生效區中》勾選資料後，再進行後續作業。";
                    throw e;
                }

                PreAdjustDAO preAdjustDAO = new PreAdjustDAO();
                List<PreAdjustEntity> preAdjustList = new List<PreAdjustEntity>();
                PreAdjustEntity preAdjustEntity = null;
                PreAdjustDO preAdjustDO = null;

                foreach (PreAdjustEntity effectItem in preAdjustInfo.EffectZone)
                {
                    preAdjustDO = preAdjustDAO.GetEffectData(effectItem.CampaignId, effectItem.Id);
                    preAdjustEntity = ConvertPreAdjustEntity(preAdjustDO);

                    preAdjustList.Add(preAdjustEntity);
                }

                if (UserInfo == null)
                {
                    throw new InvalidOperationException("UserInfo not found");
                }

                DateTime currentTime = DateTime.Now;

                foreach (PreAdjustEntity effectItem in preAdjustList)
                {
                    if (!String.IsNullOrEmpty(preAdjustInfo.Remark))
                    {
                        effectItem.Remark = preAdjustInfo.Remark;
                    }

                    effectItem.DeleteUserId = UserInfo.Id;
                    effectItem.DeleteDateTime = currentTime.ToString("yyyy/MM/dd HH:mm:ss");
                    effectItem.Status = Enum.GetName(typeof(PreAdjustStatus), PreAdjustStatus.刪除);
                }



                ConditionValidateService validateService = new ConditionValidateService();
                PreAdjustEffectEntity preAdjustEffect = null;

                int validateFailCount = 0;
                foreach (PreAdjustEntity preAdjust in preAdjustList)
                {
                    preAdjustEffect = validateService.PreAdjustEffect(preAdjust.Id);

                    if (preAdjustEffect.ResponseCode != "00")
                    {
                        validateFailCount++;
                        preAdjust.Status = Enum.GetName(typeof(PreAdjustStatus), PreAdjustStatus.失敗);
                    }
                }


                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (PreAdjustEntity preAdjust in preAdjustList)
                    {
                        preAdjust.Update();
                    }

                    scope.Complete();
                }

                result = (preAdjustList.Count() - validateFailCount);
            }


            return result;
        }

        /// <summary>
        /// 同意執行臨調預審名單
        /// </summary>
        /// <param name="data">來源資料</param>
        /// <param name="forceConsent">是否強制同意</param>
        /// <returns></returns>
        public object Agree(object data, bool forceConsent = false)
        {
            // TODO Agree
            throw new NotImplementedException();
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
        /// 紀錄行銷活動匯入紀錄與臨調預審處理檔
        /// </summary>
        /// <param name="importLog">行銷活動匯入紀錄</param>
        /// <param name="preAdjustList">臨調預審處理檔資料集合</param>
        /// <returns></returns>
        private void SaveCampaignData(CampaignImportLogDO importLog, List<PreAdjustDO> preAdjustList)
        {
            if (importLog == null)
            {
                throw new ArgumentNullException("importLog");
            }
            else if ((preAdjustList == null) || (preAdjustList.Count == 0))
            {
                throw new ArgumentNullException("preAdjustList");
            }

            using (TransactionScope scope = new TransactionScope())
            {
                new CampaignImportLogDAO().Insert(importLog);

                PreAdjustDAO preAdjustDAO = new PreAdjustDAO();
                foreach (PreAdjustDO preAdjust in preAdjustList)
                {
                    preAdjustDAO.Insert(preAdjust);
                }

                scope.Complete();
            }
        }




        /// <summary>
        /// 轉換臨調預審名單資料
        /// </summary>
        /// <param name="preAdjustList">臨調預審名單資料</param>
        /// <returns></returns>
        private IEnumerable<PreAdjustEntity> ConvertPreAdjustEntity(IEnumerable<PreAdjustDO> preAdjustList)
        {
            return preAdjustList.Select(x => new PreAdjustEntity()
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
        /// <param name="preAdjustDO">臨調預審名單資料</param>
        /// <returns></returns>
        private PreAdjustEntity ConvertPreAdjustEntity(PreAdjustDO preAdjustDO)
        {
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
        /// <param name="preAdjust">臨調預審名單資料</param>
        /// <returns></returns>
        private IEnumerable<PreAdjustDO> ConvertPreAdjustDO(IEnumerable<PreAdjustEntity> preAdjust)
        {
            return preAdjust.Select(x => new PreAdjustDO()
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
        /// <param name="preAdjust">臨調預審名單資料</param>
        /// <returns></returns>
        private PreAdjustDO ConvertPreAdjustDO(PreAdjustEntity preAdjust)
        {
            return new PreAdjustDO()
            {
                CampaignId = preAdjust.CampaignId,
                Id = preAdjust.Id,
                ProjectName = preAdjust.ProjectName,
                ProjectAmount = preAdjust.ProjectAmount,
                CloseDate = preAdjust.CloseDate,
                ImportDate = preAdjust.ImportDate,
                ChineseName = preAdjust.ChineseName,
                Kind = preAdjust.Kind,
                SmsCheckResult = preAdjust.SmsCheckResult,
                Status = preAdjust.Status,
                ProcessingDateTime = preAdjust.ProcessingDateTime,
                ProcessingUserId = preAdjust.ProcessingUserId,
                DeleteDateTime = preAdjust.DeleteDateTime,
                DeleteUserId = preAdjust.DeleteUserId,
                Remark = preAdjust.Remark,
                ClosingDay = preAdjust.ClosingDay,
                PayDeadline = preAdjust.PayDeadline,
                AgreeUserId = preAdjust.AgreeUserId,
                MobileTel = preAdjust.MobileTel,
                RejectReasonCode = preAdjust.RejectReasonCode,
                CcasReplyCode = preAdjust.CcasReplyCode,
                CcasReplyStatus = preAdjust.CcasReplyStatus,
                CcasReplyDateTime = preAdjust.CcasReplyDateTime,
            };
        }


        #endregion
    }
}
