using System.Collections.Generic;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;

namespace ThinkPower.CCLPA.Domain.Entity
{
    /// <summary>
    /// 臨調預審名單資訊類別
    /// </summary>
    public class PreAdjustInfoEntity
    {
        /// <summary>
        /// 等待區臨調預審名單
        /// </summary>
        public IEnumerable<PreAdjustEntity> WaitZone { get; set; }

        /// <summary>
        /// 生效區臨調預審名單
        /// </summary>
        public IEnumerable<PreAdjustEntity> EffectZone { get; set; }

        /// <summary>
        /// 刪除備註說明
        /// </summary>
        public string Remark { get; set; }
    }
}