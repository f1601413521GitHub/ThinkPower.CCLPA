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

            campaign = new CampaignDAO().Get(campaignId);

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

            importLogInfo = new CampaignImportLogDAO().Get(campaignId);

            return importLogInfo;
        }

        /// <summary>
        /// 取得行銷活動名單數量
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <param name="executionPathway">預估執行通路</param>
        /// <returns></returns>
        public int? GetCampaignDetailCount(string campaignId, decimal? executionPathway)
        {
            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }

            return new CampaignDetailDAO().Count(campaignId, executionPathway);
        }

        /// <summary>
        /// 取得行銷活動名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <param name="executionPathway">預估執行通路</param>
        /// <returns></returns>
        public IEnumerable<CampaignDetailDO> GetDetailList(string campaignId, decimal? executionPathway)
        {
            if (String.IsNullOrEmpty(campaignId))
            {
                throw new ArgumentNullException("campaignId");
            }

            return new CampaignDetailDAO().Get(campaignId, executionPathway);
        }
    }
}