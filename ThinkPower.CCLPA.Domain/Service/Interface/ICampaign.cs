using System.Collections.Generic;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CMPN;

namespace ThinkPower.CCLPA.Domain.Service.Interface
{
    /// <summary>
    /// 行銷活動服務公開介面
    /// </summary>
    public interface ICampaign
    {
        /// <summary>
        /// 取得行銷活動資訊
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns></returns>
        CampaignDO GetCampaign(string campaignId);

        /// <summary>
        /// 取得行銷活動匯入紀錄
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns></returns>
        CampaignImportLogDO GetImportLog(string campaignId);

        /// <summary>
        /// 取得行銷活動名單數量
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <param name="executionPathway">預估執行通路</param>
        /// <returns></returns>
        int? GetCampaignListCount(string campaignId, decimal? executionPathway);

        /// <summary>
        /// 取得行銷活動名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <param name="executionPathway">預估執行通路</param>
        /// <returns></returns>
        IEnumerable<CampaignDetailDO> GetCampaignList(string campaignId, decimal? executionPathway);
    }
}