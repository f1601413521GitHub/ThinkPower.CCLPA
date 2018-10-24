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
        public void GetAllWaitDataTest()
        {
            // Arrange
            var expected = true;

            // Actual
            var result = new PreAdjustDAO().GetAllWaitData();
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetWaitDataTest()
        {
            // Arrange
            var id = "A177842053";
            var expected = true;

            // Actual
            var result = new PreAdjustDAO().GetWaitData(id);
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetEffectDataTest()
        {
            // Arrange
            var id = "A177842053";
            var expected = true;

            // Actual
            var result = new PreAdjustDAO().GetEffectData(id);
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
            var expected = true;

            // Actual
            var result = new PreAdjustDAO().GetWaitData(campaignId, id);
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
            var expected = true;

            // Actual
            var result = new PreAdjustDAO().GetEffectData(campaignId, id);
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
                    ProjectName             = "等待區測試資料456",
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
                    MobileTel               = "933113885",
                    RejectReasonCode        = null,
                    CcasReplyCode           = null,
                    CcasReplyStatus         = null,
                    CcasReplyDateTime       = null,
                },
                new PreAdjustDO(){
                    CampaignId              = "AA20991022X99Y99Z99A",
                    Id                      = "B142357855",
                    ProjectName             = "等待區測試資料123",
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
                    PayDeadline             = "5",
                    AgreeUserId             = null,
                    MobileTel               = "916987456",
                    RejectReasonCode        = null,
                    CcasReplyCode           = null,
                    CcasReplyStatus         = null,
                    CcasReplyDateTime       = null,
                }
            };
            var expected = true;

            // Actual
            foreach (var item in preAdjustList)
            {
                new PreAdjustDAO().Update(item);
            }
            var actual = true;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}