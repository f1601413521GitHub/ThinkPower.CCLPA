using System.Collections.Generic;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;

namespace ThinkPower.CCLPA.Domain.Entity
{
    /// <summary>
    /// 臨調預審名單類別
    /// </summary>
    public class PreAdjustEntity
    {
        /// <summary>
        /// 等待區臨調預審名單
        /// </summary>
        public IEnumerable<PreAdjustDO> WaitZone { get; set; }

        /// <summary>
        /// 生效區臨調預審名單
        /// </summary>
        public IEnumerable<PreAdjustDO> EffectZone { get; set; }
    }
}