using System.Collections.Generic;

namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 強制同意預審名單處理結果類別
    /// </summary>
    public class PreAdjustForcedConsentResult
    {
        /// <summary>
        /// 生效筆數
        /// </summary>
        public int EffectCount { get; set; }

        /// <summary>
        /// 強制同意失敗結果
        /// </summary>
        public IEnumerable<ForcedConsentFailResult> FailResultList { get; set; }
    }
}