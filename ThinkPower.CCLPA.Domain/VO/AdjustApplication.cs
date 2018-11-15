using System.Collections.Generic;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.ICRS;
using ThinkPower.CCLPA.DataAccess.VO;
using ThinkPower.CCLPA.Domain.Entity;

namespace ThinkPower.CCLPA.Domain.VO
{
    /// <summary>
    /// 專案臨調申請資料類別
    /// </summary>
    public class AdjustApplication
    {
        /// <summary>
        /// 歸戶基本資料
        /// </summary>
        public Customer Customer { get; set; }
        /// <summary>
        /// 貴賓資料
        /// </summary>
        public VipInfo Vip { get; set; }
        /// <summary>
        /// JCIC送查日期
        /// </summary>
        public JcicSendQueryResult JcicSendQuery { get; set; }
        /// <summary>
        /// 專案臨調紀錄
        /// </summary>
        public IEnumerable<AdjustEntity> AdjustLogList { get; set; }
        /// <summary>
        /// 專案臨調條件檢核結果
        /// </summary>
        public AdjustConditionValidateResult AdjustValidateResult { get; set; }
    }
}