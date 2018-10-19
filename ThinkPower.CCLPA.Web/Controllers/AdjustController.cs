﻿using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThinkPower.CCLPA.Domain.DTO;
using ThinkPower.CCLPA.Domain.Service;
using ThinkPower.CCLPA.Web.ActionModels;
using ThinkPower.CCLPA.Web.ViewModels;

namespace ThinkPower.CCLPA.Web.Controllers
{
    /// <summary>
    /// 專案臨調
    /// </summary>
    public class AdjustController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private AdjustService _adjustService;

        /// <summary>
        /// 系統錯誤提示訊息
        /// </summary>
        private readonly string _systemErrorMsg = "系統發生錯誤，請於上班時段來電客服中心0800-123-456，造成不便敬請見諒。";

        /// <summary>
        /// 專案臨調服務
        /// </summary>
        public AdjustService AdjService
        {
            get
            {
                if (_adjustService == null)
                {
                    _adjustService = new AdjustService();
                }

                return _adjustService;
            }
        }

        /// <summary>
        /// 預設首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 進行預審名單檢核動作
        /// </summary>
        /// <param name="actionModel">來源資料</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ValidatePreAdjust(ValidatePreAdjustActionModel actionModel)
        {
            ValidatePreAdjustViewModel viewModel = null;

            try
            {
                if (actionModel == null)
                {
                    throw new ArgumentNullException("actionModel");
                }

                ValidatePreAdjustResultDTO validatePreAdjustResult = AdjService.
                    ValidatePreAdjust(actionModel.CampaignId);

                viewModel = new ValidatePreAdjustViewModel()
                {
                    ValidatePreAdjustResult = validatePreAdjustResult ??
                            throw new InvalidOperationException("ValidatePreAdjustResult not found")
                };
            }
            catch (Exception e)
            {
                logger.Error(e);
                ModelState.AddModelError("", _systemErrorMsg);
            }

            return View(viewModel);
        }
    }
}