namespace ThinkPower.CCLPA.DataAccess.DO.ICRS
{
    /// <summary>
    /// 歸戶基本資料精簡資料物件類別
    /// </summary>
    public class CustomerPartialInfoDO
    {
        /// <summary>
        /// 中文姓名 
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// 行動電話 
        /// </summary>
        public string MobileTel { get; set; }

        /// <summary>
        /// 關帳日 
        /// </summary>
        public string ClosingDay { get; set; }

        /// <summary>
        /// 繳款期限 
        /// </summary>
        public string PayDeadline { get; set; }

    }
}