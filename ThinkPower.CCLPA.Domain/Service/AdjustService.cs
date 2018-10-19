using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DAO.CMPN;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CMPN;
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
        public CampaignService CampaignService
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

        public AdjustService() { }

        /// <summary>
        /// 此建構函示可設定行銷活動服務來源。
        /// </summary>
        /// <param name="campaignService"></param>
        public AdjustService(CampaignService campaignService)
        {
            if (campaignService != null)
            {
                _campaignService = campaignService;
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
                campaignListCount = CampaignService.CampaignListCount(campaignInfo.CampaignId, 
                    campaignInfo.ExecutionPathway);
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
        public object ImportPreAdjust(string campaignId)
        {
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
