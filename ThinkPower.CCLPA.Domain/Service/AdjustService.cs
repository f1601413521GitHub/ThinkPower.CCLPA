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
    }
}
