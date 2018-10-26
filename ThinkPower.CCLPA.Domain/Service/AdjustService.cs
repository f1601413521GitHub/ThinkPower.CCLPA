using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using ThinkPower.CCLPA.DataAccess.DAO.ICRS;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.DO.CMPN;
using ThinkPower.CCLPA.DataAccess.DO.ICRS;
using ThinkPower.CCLPA.Domain.Entity;
using ThinkPower.CCLPA.Domain.Service.Interface;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 臨調服務
    /// </summary>
    public class AdjustService : BaseService, IAdjust
    {
        /// <summary>
        /// 臨調處理
        /// </summary>
        public void AdjustProcessing()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 取得使用者臨調權限資訊
        /// </summary>
        /// <returns></returns>
        public AdjustPermission GetUserPermission()
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
        public string GetUserAccountByICRS()
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
