using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.Domain.Condition;
using ThinkPower.CCLPA.Domain.Entity;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service.Interface
{
    /// <summary>
    /// 臨調預審服務公開介面
    /// </summary>
    interface IPreAdjust
    {
        /// <summary>
        /// 檢核臨調預審名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns>預審名單驗證結果</returns>
        PreAdjustValidateResult Validate(string campaignId);

        /// <summary>
        /// 匯入臨調預審名單
        /// </summary>
        /// <param name="campaignId">行銷活動代號</param>
        /// <returns></returns>
        void Import(string campaignId);

        /// <summary>
        /// 查詢臨調預審名單
        /// </summary>
        /// <param name="searchInfo">預審名單搜尋條件</param>
        /// <returns>臨調預審名單</returns>
        IEnumerable<PreAdjustEntity> Query(PreAdjustCondition searchInfo);

        /// <summary>
        /// 刪除等待的臨調預審名單
        /// </summary>
        /// <param name="preAdjustInfo">來源資料</param>
        /// <returns>刪除預審名單筆數</returns>
        int DeleteNotEffect(PreAdjustInfo preAdjustInfo);

        /// <summary>
        /// 刪除生效的臨調預審名單
        /// </summary>
        /// <param name="preAdjustInfo">來源資料</param>
        /// <returns>刪除預審名單筆數</returns>
        int DeleteEffect(PreAdjustInfo preAdjustInfo);

        /// <summary>
        /// 同意執行臨調預審名單
        /// </summary>
        /// <param name="data">來源資料</param>
        /// <param name="forceConsent">是否強制同意</param>
        /// <returns></returns>
        object Agree(object data, bool forceConsent = false);
    }
}
