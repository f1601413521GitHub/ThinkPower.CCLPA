using System;
using System.Collections.Generic;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DAO.CMPN;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CMPN;
using ThinkPower.CCLPA.Domain.Service.Interface;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 行銷活動服務
    /// </summary>
    public class CampaignService : ICampaign
    {
        private CampaignDO _campaignInfo;
        private CampaignImportLogDO _importLog;




        public CampaignService() { }

        /// <summary>
        /// 此建構函式可設定：行銷活動檔、行銷活動匯入紀錄檔。
        /// </summary>
        /// <param name="campaignInfo">行銷活動檔</param>
        /// <param name="importLog">行銷活動匯入紀錄檔</param>
        public CampaignService(CampaignDO campaignInfo, CampaignImportLogDO importLog = null)
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
        /// 取得行銷活動資訊
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns></returns>
        public CampaignDO GetCampaign(string campaignId)
        {
            CampaignDO campaign = null;

            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }

            if (_campaignInfo != null)
            {
                campaign = _campaignInfo;
            }
            else
            {
                campaign = new CampaignDAO().Get(campaignId);
            }

            return campaign;
        }

        /// <summary>
        /// 取得行銷活動匯入紀錄
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns></returns>
        public CampaignImportLogDO GetImportLog(string campaignId)
        {
            CampaignImportLogDO importLogInfo = null;

            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }

            if (_importLog != null)
            {
                importLogInfo = _importLog;
            }
            else
            {
                importLogInfo = new CampaignImportLogDAO().Get(campaignId);
            }

            return importLogInfo;
        }

        /// <summary>
        /// 取得行銷活動名單數量
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <param name="executionPathway">預估執行通路</param>
        /// <returns></returns>
        public int? GetCampaignListCount(string campaignId, decimal? executionPathway)
        {
            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }

            return new CampaignListDAO().Count(campaignId, executionPathway);
        }

        /// <summary>
        /// 取得行銷活動名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <param name="executionPathway">預估執行通路</param>
        /// <returns></returns>
        public IEnumerable<CampaignListDO> GetCampaignList(string campaignId, decimal? executionPathway)
        {
            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }

            return new CampaignListDAO().Get(campaignId, executionPathway);
        }
    }
}