using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service.Interface
{
    /// <summary>
    /// 條件檢核服務公開介面
    /// </summary>
    public interface IConditionValidate
    {
        /// <summary>
        /// JCIC送查日期回傳
        /// </summary>
        /// <returns></returns>
        object JcicSendDate();

        /// <summary>
        /// 專案臨調條件檢核
        /// </summary>
        /// <returns></returns>
        object Adjust();

        /// <summary>
        /// 預審生效條件檢核
        /// </summary>
        /// <param name="id">客戶ID</param>
        /// <returns></returns>
        PreAdjustEffectResult PreAdjustEffect(string id);
    }
}