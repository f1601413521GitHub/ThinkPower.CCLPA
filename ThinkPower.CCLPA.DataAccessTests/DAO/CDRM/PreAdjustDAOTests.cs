using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;
using ThinkPower.CCLPA.DataAccess.Condition;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM.Tests
{
    [TestClass()]
    public class PreAdjustDAOTests
    {
        [TestMethod()]
        public void UpdateTest()
        {
            // Arrange
            IEnumerable<PreAdjustDO> preAdjustList = new List<PreAdjustDO>()
            {
                new PreAdjustDO(){
                    CampaignId              = "AA20991022X99Y99Z99A",
                    Id                      = "A177842053",
                    ProjectName             = "等待區測試資料Update",
                    ProjectAmount           = 30000,
                    CloseDate               = "2099/10/22",
                    ImportDate              = "2018/10/23",
                    ChineseName             = "王小明",
                    Kind                    = "AA",
                    SmsCheckResult          = null,
                    Status                  = "待生效",
                    ProcessingDateTime      = null,
                    ProcessingUserId        = null,
                    DeleteDateTime          = null,
                    DeleteUserId            = null,
                    Remark                  = null,
                    ClosingDay              = "10",
                    PayDeadline             = "25",
                    ForceAgreeUserId             = null,
                    MobileTel               = "0933113885",
                    RejectReasonCode        = null,
                    CcasReplyCode           = null,
                    CcasReplyStatus         = null,
                    CcasReplyDateTime       = null,
                },
                new PreAdjustDO(){
                    CampaignId              = "AA20991022X99Y99Z99A",
                    Id                      = "B142357855",
                    ProjectName             = "等待區測試資料Update",
                    ProjectAmount           = 40000,
                    CloseDate               = "2099/10/22",
                    ImportDate              = "2018/10/23",
                    ChineseName             = "李大同",
                    Kind                    = "BB",
                    SmsCheckResult          = null,
                    Status                  = "待生效",
                    ProcessingDateTime      = null,
                    ProcessingUserId        = null,
                    DeleteDateTime          = null,
                    DeleteUserId            = null,
                    Remark                  = null,
                    ClosingDay              = "20",
                    PayDeadline             = "05",
                    ForceAgreeUserId             = null,
                    MobileTel               = "0916987456",
                    RejectReasonCode        = null,
                    CcasReplyCode           = null,
                    CcasReplyStatus         = null,
                    CcasReplyDateTime       = null,
                }
            };
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            foreach (var item in preAdjustList)
            {
                dao.Update(item);
            }
            var actual = true;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void InsertTest()
        {
            // Arrange
            PreAdjustDO preAdjust = new PreAdjustDO()
            {
                CampaignId = "ZZ20190101X99Y99Z99A",
                Id = "Z177842053",
                ProjectName = "等待區測試資料Insert",
                ProjectAmount = 30000,
                CloseDate = "2019/01/01",
                ImportDate = "2019/01/01",
                ChineseName = "王小明",
                Kind = "AA",
                SmsCheckResult = null,
                Status = "待生效",
                ProcessingDateTime = null,
                ProcessingUserId = null,
                DeleteDateTime = null,
                DeleteUserId = null,
                Remark = null,
                ClosingDay = "10",
                PayDeadline = "25",
                ForceAgreeUserId = null,
                MobileTel = "0933113885",
                RejectReasonCode = null,
                CcasReplyCode = null,
                CcasReplyStatus = null,
                CcasReplyDateTime = null,
            };
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var actual = true;
            try
            {
                dao.Insert(preAdjust);
            }
            catch (Exception e)
            {
                actual = false;
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetTest_When_CcasCodeIs00_Then_ResultEffectCase()
        {
            // Arrange
            var condition = new PreAdjustCondition()
            {
                PageIndex = null,
                PagingSize = null,
                CloseDate = null,
                CcasReplyCode = "00",
                CustomerId = null,
                CampaignId = null,

            };
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.Query(condition);
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetTest_When_CcasCodeIsNull_Then_ResultNotEffectCase()
        {
            // Arrange
            var condition = new PreAdjustCondition()
            {
                PageIndex = null,
                PagingSize = null,
                CloseDate = null,
                CcasReplyCode = null,
                CustomerId = null,
                CampaignId = null,
            };
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.Query(condition);
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetTest_When_CcasCodeIsNull_CloseDate20160525_Then_ResultNotEffectCase()
        {
            // Arrange
            var condition = new PreAdjustCondition()
            {
                PageIndex = null,
                PagingSize = null,
                CloseDate = new DateTime(2016, 5, 25),
                CcasReplyCode = null,
                CustomerId = null,
                CampaignId = null,
            };
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.Query(condition);
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetTest_When_CcasCodeIsNull_OrderByCustomerId_PageIndexIs0_PageSizeIs99_Then_ResultNotEffectCase()
        {
            // Arrange
            var condition = new PreAdjustCondition()
            {
                PageIndex = 0,
                PagingSize = 99,
                CloseDate = null,
                CcasReplyCode = null,
                CustomerId = null,
                CampaignId = null,
                OrderBy = PreAdjustCondition.OrderByKind.CustomerId,
            };
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.Query(condition);
            var actual = (result != null);

            Console.Write(String.Join(",", result.Select(x => new { x.CampaignId, x.Id })));

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetTest_When_CcasCodeIsNull_OrderByCustomerId_PageIndexIs0_PageSizeIs5_Then_ResultNotEffectCase()
        {
            // Arrange
            var condition = new PreAdjustCondition()
            {
                PageIndex = 0,
                PagingSize = 5,
                CloseDate = null,
                CcasReplyCode = null,
                CustomerId = null,
                CampaignId = null,
                OrderBy = PreAdjustCondition.OrderByKind.CustomerId,
            };
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.Query(condition);
            var actual = (result != null);

            Console.Write(String.Join(",", result.Select(x => new { x.CampaignId, x.Id })));

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetTest_When_CcasCodeIsNull_OrderByCustomerId_PageIndexIs1_PageSizeIs5_Then_ResultNotEffectCase()
        {
            // Arrange
            var condition = new PreAdjustCondition()
            {
                PageIndex = 1,
                PagingSize = 5,
                CloseDate = null,
                CcasReplyCode = null,
                CustomerId = null,
                CampaignId = null,
                OrderBy = PreAdjustCondition.OrderByKind.CustomerId,
            };
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.Query(condition);
            var actual = (result != null);

            Console.Write(String.Join(",", result.Select(x => new { x.CampaignId, x.Id })));

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetTest_When_CcasCodeIsNull_IdIsA177842053_Then_ResultNotEffectCase()
        {
            // Arrange
            var condition = new PreAdjustCondition()
            {
                PageIndex = null,
                PagingSize = null,
                CloseDate = null,
                CcasReplyCode = null,
                CustomerId = "A177842053",
                CampaignId = null,
            };
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.Query(condition);
            var actual = (result != null);

            Console.Write(String.Join(",", result.Select(x => new { x.CampaignId, x.Id })));

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetTest_When_CcasCodeIsNull_IdIsA177842053_CampaignIdIsZZ20190101X99Y99Z99A_Then_ResultNotEffectCase()
        {
            // Arrange
            var condition = new PreAdjustCondition()
            {
                PageIndex = null,
                PagingSize = null,
                CloseDate = null,
                CcasReplyCode = null,
                CustomerId = "A177842053",
                CampaignId = "ZZ20190101X99Y99Z99A",
            };
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.Query(condition);
            var actual = (result != null);

            Console.Write(String.Join(",", result.Select(x => new { x.CampaignId, x.Id })));

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetTest_When_CcasCodeIsNull_CampaignIdIsZZ20190101X99Y99Z99A_Then_ResultNotEffectCase()
        {
            // Arrange
            var condition = new PreAdjustCondition()
            {
                PageIndex = null,
                PagingSize = null,
                CloseDate = null,
                CcasReplyCode = null,
                CustomerId = null,
                CampaignId = "ZZ20190101X99Y99Z99A",
            };
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.Query(condition);
            var actual = (result != null);

            Console.Write(String.Join(",", result.Select(x => new { x.CampaignId, x.Id })));

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}