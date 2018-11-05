using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service.Interface
{
    /// <summary>
    /// 使用者服務公開介面
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// 取得使用者臨調權限資訊
        /// </summary>
        /// <returns></returns>
        AdjustPermission GetUserPermission();
    }
}