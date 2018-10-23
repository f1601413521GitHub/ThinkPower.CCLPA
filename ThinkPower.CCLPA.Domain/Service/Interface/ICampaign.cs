using System.Collections.Generic;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CMPN;
using ThinkPower.CCLPA.Domain.Entity;

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
        CampaignEntity GetCampaign(string campaignId);
    }
}