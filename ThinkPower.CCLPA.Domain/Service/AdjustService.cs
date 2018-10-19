using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DAO.ICRS;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CMPN;
using ThinkPower.CCLPA.DataAccess.DO.ICRS;
using ThinkPower.CCLPA.Domain.DTO;
using ThinkPower.CCLPA.Domain.Service.Interface;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 專案臨調服務
    /// </summary>
    public class AdjustService : IAdjust
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
        /// 檢核預審名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns>檢核結果</returns>
        public ValidatePreAdjustResultDTO ValidatePreAdjust(string campaignId)
        {
            ValidatePreAdjustResultDTO result = null;
            string validateMsg = null;
            int? campaignListCount = null;



            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }

            CampaignDO campaignInfo = CampaignService.GetCampaign(campaignId);

            if (campaignInfo == null)
            {
                validateMsg = "ILRC行銷活動編碼，輸入錯誤。";
            }
            else if (!DateTime.TryParseExact(campaignInfo.ExpectedCloseDate, "yyyyMMdd", null,
                System.Globalization.DateTimeStyles.None, out DateTime tempCloseDate))
            {
                throw new InvalidOperationException("Convert ExpectedCloseDate Fail");
            }
            else if (tempCloseDate < DateTime.Now.Date)
            {
                validateMsg = "此行銷活動已結案，無法進入匯入作業。";
            }
            else
            {
                CampaignImportLogDO importLogInfo = CampaignService.GetImportLog(campaignInfo.CampaignId);

                if (importLogInfo != null)
                {
                    validateMsg = $"此行銷活動已於{importLogInfo.ImportDate}匯入過，無法再進行匯入。";
                }
            }



            if (String.IsNullOrEmpty(validateMsg))
            {
                campaignListCount = CampaignService.GetCampaignListCount(campaignInfo.CampaignId,
                    campaignInfo.ExecutionPathway);
            }


            result = new ValidatePreAdjustResultDTO()
            {
                ErrorMessage = validateMsg,
                CampaignListCount = campaignListCount,
            };

            return result;
        }

        /// <summary>
        /// 匯入預審名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <param name="userId">登入帳號</param>
        /// <param name="userName">登入姓名</param>
        /// <returns>處理結果</returns>
        public ImportPreAdjustResultDTO ImportPreAdjust(string campaignId, string userId,
            string userName)
        {
            ImportPreAdjustResultDTO result = null;

            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }

            ValidatePreAdjustResultDTO validateResult = ValidatePreAdjust(campaignId);

            if (validateResult == null)
            {
                var e = new InvalidOperationException("ValidateResult not found");
                e.Data["campaignId"] = campaignId;
                throw e;
            }
            else if (!String.IsNullOrEmpty(validateResult.ErrorMessage))
            {
                result.ValidateMessage = validateResult.ErrorMessage;
                return result;
            }


            //新增到行銷活動匯入紀錄檔(LOG_RG_ILRC)、臨調預審處理檔(RG_PADJUST)
            CampaignDO campaignInfo = CampaignService.GetCampaign(campaignId);

            if (campaignInfo == null)
            {
                var e = new InvalidOperationException("CampaignInfo not found");
                e.Data["campaignId"] = campaignId;
                throw e;
            }



            IEnumerable<CampaignListDO> campaignList = CampaignService.GetCampaignList(
                campaignInfo.CampaignId, campaignInfo.ExecutionPathway);

            if ((campaignList == null) || (campaignList.Count() == 0))
            {
                var e = new InvalidOperationException("CampaignList not found");
                e.Data["CampaignId"] = campaignInfo.CampaignId;
                e.Data["ExecutionPathway"] = campaignInfo.ExecutionPathway;
                throw e;
            }


            DateTime currentTime = DateTime.Now;

            CampaignImportLogDO importLog = new CampaignImportLogDO()
            {
                CampaignId = campaignInfo.CampaignId,
                ExpectedStartDate = campaignInfo.ExpectedStartDateTime,
                ExpectedEndDate = campaignInfo.ExpectedEndDateTime,
                Count = campaignList.Count(),
                ImportUserId = userId,
                ImportUserName = userName,
                ImportDate = currentTime.ToString("yyyy/MM/dd"),
            };

            List<PreAdjustDO> preAdjustList = new List<PreAdjustDO>();
            PreAdjustDO preAdjust = null;

            foreach (CampaignListDO item in campaignList)
            {
                preAdjust = null;
                preAdjust = new PreAdjustDO()
                {
                    CampaignId = campaignInfo.CampaignId,
                    Id = item.CustomerId,
                    ProjectName = item.Col1,
                    ProjectAmount = Convert.ToDecimal(item.Col2),
                    CloseDate = item.Col3,
                    ImportDate = currentTime.ToString("yyyy/MM/dd"),
                    Kind = item.Col4,
                    Status = "待生效",
                };

                preAdjustList.Add(preAdjust);
            }

            if (preAdjustList.Count == 0)
            {
                throw new InvalidOperationException("preAdjustList not found");
            }

            AboutDataDAO aboutDataDAO = new AboutDataDAO();
            AboutDataDO aboutData = null;
            PreAdjustDO tempPreAdjust = null;

            foreach (CampaignListDO item in campaignList)
            {
                aboutData = null;
                aboutData = aboutDataDAO.GetPreAdjustNeeded(item.CustomerId);

                if (aboutData == null)
                {
                    var e = new InvalidOperationException("AboutData not found");
                    e.Data["CustomerId"] = item.CustomerId;
                    throw e;
                }

                tempPreAdjust = null;
                tempPreAdjust = preAdjustList.FirstOrDefault(x => x.Id == item.CustomerId);

                if (tempPreAdjust == null)
                {
                    var e = new InvalidOperationException("tempPreAdjust not found");
                    e.Data["CustomerId"] = item.CustomerId;
                    throw e;
                }

                tempPreAdjust.ChineseName = aboutData.ChineseName;
                tempPreAdjust.ClosingDay = aboutData.ClosingDay;
                tempPreAdjust.PayDeadline = aboutData.PayDeadline;
                tempPreAdjust.MobileTel = aboutData.MobileTel;
            }


            SaveCampaignData(importLog, preAdjustList);


            return result;
        }

        /// <summary>
        /// 處理預審名單
        /// </summary>
        public void PreAdjustProcessing()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 臨調處理
        /// </summary>
        public void AdjustProcessing()
        {
            throw new NotImplementedException();
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
                new PreAdjustDAO().Insert(preAdjustList);

                scope.Complete();
            }
        }
    }
}
