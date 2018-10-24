using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.DataAccess.DO.CDRM;

namespace ThinkPower.CCLPA.DataAccess.DAO.CDRM.Tests
{
    [TestClass()]
    public class PreAdjustDAOTests
    {
        [TestMethod()]
        public void GetAllWaitData_When_HasOrderCondition()
        {
            // Arrange
            var dao = new PreAdjustDAO();
            var condition = new PagingCondition()
            {
                PageIndex = 1,
                PagingSize = 3
            };
            var expected = true;

            // Actual
            var result = dao.GetAllWaitData(condition);
            var actual = (result != null);


            Console.WriteLine(String.Join(",", result.Select(x => x.Id)));

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetAllEffectData_When_HasOrderCondition()
        {
            // Arrange
            var dao = new PreAdjustDAO();
            var condition = new PagingCondition()
            {
                PageIndex = 0,
                PagingSize = 2
            };
            var expected = true;

            // Actual
            var result = dao.GetAllEffectData(condition);
            var actual = (result != null);


            Console.WriteLine(String.Join(",", result.Select(x => x.Id)));

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetAllWaitDataTest()
        {
            // Arrange
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.GetAllWaitData();
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetAllEffectDataTest()
        {
            // Arrange
            var expected = true;

            // Actual
            var result = new PreAdjustDAO().GetAllEffectData();
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetWaitData_When_HasOrderCondition()
        {
            // Arrange
            var id = "A177842053";
            var condition = new PagingCondition()
            {
                PageIndex = 0,
                PagingSize = 1,
            };
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.GetWaitData(id, condition);
            var actual = (result != null);

            Console.WriteLine(String.Join(",", result.Select(x => new { x.CampaignId,x.Id })));

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetEffectData_When_HasOrderCondition()
        {
            // Arrange
            var id = "A177842053";
            var condition = new PagingCondition()
            {
                PageIndex = 0,
                PagingSize = 2,
            };
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.GetEffectData(id, condition);
            var actual = (result != null);

            Console.WriteLine(String.Join(",", result.Select(x => new { x.CampaignId, x.Id })));

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetWaitDataTest()
        {
            // Arrange
            var id = "A177842053";
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.GetWaitData(id);
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetEffectDataTest()
        {
            // Arrange
            var id = "A177842053";
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.GetEffectData(id);
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetWaitDataTest_When_Input_CampaignId_And_Id()
        {
            // Arrange
            var campaignId = "AA20991022X99Y99Z99A";
            var id = "A177842053";
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.GetWaitData(campaignId, id);
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetEffectDataTest_When_Input_CampaignId_And_Id()
        {
            // Arrange
            var campaignId = "AA20981022X99Y99Z99A";
            var id = "D187388854";
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            var result = dao.GetEffectData(campaignId, id);
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

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
                    AgreeUserId             = null,
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
                    AgreeUserId             = null,
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
                CampaignId = "AA20991024X99Y99Z99A",
                Id = "A177842053",
                ProjectName = "等待區測試資料Insert",
                ProjectAmount = 30000,
                CloseDate = "2099/10/22",
                ImportDate = "2018/10/23",
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
                AgreeUserId = null,
                MobileTel = "0933113885",
                RejectReasonCode = null,
                CcasReplyCode = null,
                CcasReplyStatus = null,
                CcasReplyDateTime = null,
            };
            var dao = new PreAdjustDAO();
            var expected = true;

            // Actual
            dao.Insert(preAdjust);
            var actual = true;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}