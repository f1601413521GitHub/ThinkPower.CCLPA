using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DAO.CMPN;
using ThinkPower.CCLPA.DataAccess.DO;
using ThinkPower.CCLPA.Domain.DTO;
using ThinkPower.CCLPA.Domain.Entity;
using ThinkPower.CCLPA.Domain.Service.Interface;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 專案臨調服務
    /// </summary>
    public class AdjustService : IAdjust
    {
        private CampaignDO _campaignInfo;
        private CampaignImportLogDO _importLog;



        public AdjustService() { }

        /// <summary>
        /// 此建構函式可設定：行銷活動預估結案日期、行銷活動匯入紀錄檔。
        /// </summary>
        /// <param name="campaignInfo">行銷活動檔</param>
        /// <param name="importLog">行銷活動匯入紀錄檔</param>
        public AdjustService(CampaignDO campaignInfo, CampaignImportLogDO importLog = null)
        {
            if (campaignInfo != null)
            {
                _campaignInfo = campaignInfo;
            }

            if (importLog != null)
            {
                _importLog = importLog;
            }
        }

        /// <summary>
        /// 檢核預審名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns></returns>
        public ValidatePreAdjustResultDTO ValidatePreAdjust(string campaignId)
        {
            ValidatePreAdjustResultDTO result = null;
            string validateMsg = null;
            int? campaignListCount = null;



            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }

            CampaignDO campaignInfo = new CampaignDAO().Get(campaignId);

            if (_campaignInfo != null)
            {
                campaignInfo = _campaignInfo;
            }

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
                CampaignImportLogDO importLogInfo = new CampaignImportLogDAO().
                    Get(campaignInfo.CampaignId);

                if (_importLog != null)
                {
                    importLogInfo = _importLog;
                }

                if (importLogInfo != null)
                {
                    validateMsg = $"此行銷活動已於{importLogInfo.ImportDate}匯入過，無法再進行匯入。";
                }
            }


            if (String.IsNullOrEmpty(validateMsg))
            {
                campaignListCount = new CampaignListDAO().Count(campaignInfo.CampaignId);
            }


            result = new ValidatePreAdjustResultDTO()
            {
                ErrorMessage = validateMsg,
                CampaignListCount = campaignListCount,
            };

            if (campaignInfo != null)
            {
                result.CampaignId = campaignInfo.CampaignId;
            }

            return result;
        }

        /// <summary>
        /// 匯入預審名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns></returns>
        public void ImportPreAdjust(string campaignId)
        {
            //Entity: 新增到行銷活動匯入紀錄檔(LOG_RG_ILRC)、臨調預審處理檔(RG_PADJUST)
            //MarketingActivitiesRecordEntity
            //TemporaryAdjustmentPreTrialProcessingEntity
            throw new NotImplementedException();
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
    }
}
