using System;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.Domain.Service.Interface;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 使用者服務
    /// </summary>
    public class UserService : BaseService, IUser
    {
        /// <summary>
        /// 取得使用者臨調權限資訊
        /// </summary>
        /// <returns></returns>
        internal AdjustPermission GetUserPermission()
        {
            AdjustPermission result = null;

            if (UserInfo == null)
            {
                throw new InvalidOperationException("UserInfo");
            }


            AccountCorrespondDO correspondInfo = new AccountCorrespondDAO().Get(UserInfo.Id);

            if (correspondInfo == null)
            {
                throw new InvalidOperationException("AccountCorrespond not found");
            }

            AdjustUserLevelDO userLevelInfo = new AdjustUserLevelDAO().Get(correspondInfo.IcrsId);

            if (userLevelInfo == null)
            {
                throw new InvalidOperationException("AdjustUserLevel not found");
            }

            AdjustLevelPermissionDO permissionInfo = new AdjustLevelPermissionDAO().
                Get(userLevelInfo.LevelCode);

            if (permissionInfo == null)
            {
                throw new InvalidOperationException("AdjustLevelPermission not found");
            }


            result = new AdjustPermission()
            {
                LevelCode = permissionInfo.LevelCode,
                Amount = permissionInfo.Amount,
                AdjustQuery = permissionInfo.AdjustQuery,
                AdjustExecute = permissionInfo.AdjustExecute,
                VerifyNormal = permissionInfo.VerifyNormal,
                VerifySupervisor = permissionInfo.VerifySupervisor,
                SequenceNo = permissionInfo.SequenceNo,
            };


            return result;
        }


        /// <summary>
        /// 取得使用者ICRS帳號
        /// </summary>
        /// <returns></returns>
        internal string GetUserAccountByICRS()
        {
            string result = null;

            if (UserInfo == null)
            {
                throw new InvalidOperationException("UserInfo");
            }

            AccountCorrespondDO correspondInfo = new AccountCorrespondDAO().Get(UserInfo.Id);

            if (correspondInfo == null)
            {
                throw new InvalidOperationException("AccountCorrespond not found");
            }

            result = correspondInfo.IcrsId;

            return result;
        }
    }
}