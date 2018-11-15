using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service.Interface
{
    /// <summary>
    /// CDRM臨調系統服務公開介面
    /// </summary>
    public interface IAdjustSystem
    {
        /// <summary>
        /// JCIC送查日期回傳
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        JcicSendQueryResult JcicSendQuery(string customerId);

        /// <summary>
        /// 預審生效條件檢核
        /// </summary>
        /// <param name="id">客戶ID</param>
        /// <returns></returns>
        PreAdjustEffectResult ValidatePreAdjustEffectConfition(string id);
    }
}