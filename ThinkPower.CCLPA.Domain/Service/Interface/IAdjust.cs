using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.Domain.Entity;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service.Interface
{
    /// <summary>
    /// 臨調服務公開介面
    /// </summary>
    interface IAdjust
    {
        /// <summary>
        /// 臨調處理
        /// </summary>
        void AdjustProcessing();

        /// <summary>
        /// 取得使用者臨調權限資訊
        /// </summary>
        /// <returns></returns>
        AdjustPermission GetUserPermission();

        /// <summary>
        /// 取得使用者ICRS帳號
        /// </summary>
        /// <returns></returns>
        string GetUserAccountByICRS();
    }
}
