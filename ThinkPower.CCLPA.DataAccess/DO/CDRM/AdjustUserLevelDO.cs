namespace ThinkPower.CCLPA.DataAccess.DO.CDRM
{
    /// <summary>
    /// 臨調人員權限資料物件類別
    /// </summary>
    public class AdjustUserLevelDO
    {
        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 權限等級
        /// </summary>
        public string LevelCode { get; set; }
    }
}