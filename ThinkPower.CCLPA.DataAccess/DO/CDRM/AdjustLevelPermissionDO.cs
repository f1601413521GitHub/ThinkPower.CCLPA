using System;

namespace ThinkPower.CCLPA.DataAccess.DO.CDRM
{
    /// <summary>
    /// 臨調權限設定檔資料物件類別
    /// </summary>
    public class AdjustLevelPermissionDO
    {
        /// <summary> 
        /// 權限等級  
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary> 
        /// 額度  
        /// </summary>
        public string Amount { get; set; }

        /// <summary> 
        /// 臨時額度調整預審查詢  
        /// </summary>
        public string AdjustQuery { get; set; }

        /// <summary> 
        /// 臨時額度調整案件處理  
        /// </summary>
        public string AdjustExecute { get; set; }

        /// <summary> 
        /// 一般PENDING案件處理  
        /// </summary>
        public string VerifyNormal { get; set; }

        /// <summary> 
        /// 轉授信主管PENDING案件處理  
        /// </summary>
        public string VerifySupervisor { get; set; }

        /// <summary> 
        /// 序號  
        /// </summary>
        public Nullable<decimal> SequenceNo { get; set; }

    }
}