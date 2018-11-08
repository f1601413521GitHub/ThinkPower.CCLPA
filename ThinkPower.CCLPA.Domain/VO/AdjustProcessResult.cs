using System.Collections.Generic;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.ICRS;
using ThinkPower.CCLPA.DataAccess.VO;

namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 專案臨調處理結果類別
    /// </summary>
    public class AdjustProcessResult
    {
        /// <summary>
        /// 歸戶基本資訊
        /// </summary>
        public CustomerInfo CustomerInfo { get; set; }
        /// <summary>
        /// 貴賓基本資訊
        /// </summary>
        public VipInfo VipInfo { get; set; }
        /// <summary>
        /// JCIC送查日期回傳
        /// </summary>
        public JcicQueryResultInfo JcicInfo { get; set; }
        /// <summary>
        /// 調高原因代碼檔
        /// </summary>
        public IEnumerable<IncreaseReasonCodeInfo> IncreaseReasonList { get; set; }
        /// <summary>
        /// 專案參數目前生效主檔
        /// </summary>
        public IEnumerable<ParamCurrentlyEffectInfo> CurrentlyEffectList { get; set; }
        /// <summary>
        /// 專案臨調紀錄
        /// </summary>
        public IEnumerable<AdjustInfo> AdjustList { get; set; }
    }
}