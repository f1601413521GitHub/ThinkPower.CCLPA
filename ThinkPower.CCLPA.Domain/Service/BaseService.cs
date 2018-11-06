﻿using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.Domain.VO;

namespace ThinkPower.CCLPA.Domain.Service
{
    /// <summary>
    /// 服務物件基底類別
    /// </summary>
    public class BaseService
    {
        /// <summary>
        /// Nlog物件
        /// </summary>
        protected Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// 使用者資訊
        /// </summary>
        public UserInfo UserInfo { get; set; }
    }
}