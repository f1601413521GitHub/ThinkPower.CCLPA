using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service.Interface
{
    /// <summary>
    /// CCAS授信系統服務公開介面
    /// </summary>
    public interface ICreditSystem
    {
        /// <summary>
        /// ICRS查詢掛帳金額 (含已授權未清算)、可用額度
        /// </summary>
        /// <param name="customerId">客戶ID</param>
        /// <returns></returns>
        IcrsAmountInfo QueryIcrsAmount(string customerId);

        /// <summary>
        /// 所得稅卡戶臨調
        /// </summary>
        /// <param name="adjustInfo">所得稅卡戶臨調資訊</param>
        /// <returns></returns>
        string IncomeTaxCardAdjust(IncomeTaxCardAdjustInfo adjustInfo);

        /// <summary>
        /// 非所得稅卡戶臨調
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        object NonIncomeTaxCardAdjust(object data);

    }
}