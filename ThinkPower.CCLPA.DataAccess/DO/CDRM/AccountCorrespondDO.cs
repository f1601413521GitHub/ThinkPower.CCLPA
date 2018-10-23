namespace ThinkPower.CCLPA.DataAccess.DO.CDRM
{
    /// <summary>
    /// 帳號對應資料物件類別
    /// </summary>
    public class AccountCorrespondDO
    {
        /// <summary>
        /// 使用者代號
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// ICRS帳號
        /// </summary>
        public string IcrsId { get; set; }
    }
}