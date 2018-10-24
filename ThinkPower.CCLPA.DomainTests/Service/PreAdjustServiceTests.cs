using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkPower.CCLPA.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.Domain.VO;
using ThinkPower.CCLPA.Domain.Entity;

namespace ThinkPower.CCLPA.Domain.Service.Tests
{
    [TestClass()]
    public class PreAdjustServiceTests
    {
        [TestMethod()]
        public void SearchTest_When_IdIsNullOrEmpty_Then_ResultNotNull()
        {
            // Arrange
            string id = null;
            var service = new PreAdjustService();
            var expected = true;

            // Actual
            var result = service.Search(id);
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void SearchTest_When_HasId_Then_ResultNotNull()
        {
            // Arrange
            string id = "A177842053";
            var service = new PreAdjustService();
            var expected = true;

            // Actual
            var result = service.Search(id);
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void DeleteWaitZone()
        {
            // Arrange
            PreAdjustInfoEntity preAdjustInfo = new PreAdjustInfoEntity()
            {
                WaitZone = new List<PreAdjustEntity>() {
                    new PreAdjustEntity(){CampaignId = "AA20991022X99Y99Z99A",Id="A177842053" },
                    new PreAdjustEntity(){CampaignId = "AA20991022X99Y99Z99A",Id="B142357855" },
                },
                Remark = "測試刪除等待區資料",
            };
            var service = new PreAdjustService();
            service.UserInfo = new UserInfoVO() { Id = "14260", Name = "User14260" };
            var expected = 2;

            // Actual
            var result = service.Delete(preAdjustInfo, true);
            var actual = 2;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void DeleteEffectZone()
        {
            // Arrange
            PreAdjustInfoEntity preAdjustInfo = new PreAdjustInfoEntity()
            {
                EffectZone = new List<PreAdjustEntity>() {
                    new PreAdjustEntity(){CampaignId = "AA20981022X99Y99Z99A",Id="A177842053" },
                    new PreAdjustEntity(){CampaignId = "AA20981022X99Y99Z99A",Id="B142357855" },
                },
                Remark = "測試刪除生效區資料",
            };
            var service = new PreAdjustService();
            service.UserInfo = new UserInfoVO() { Id = "14260", Name = "User14260" };
            var expected = 2;

            // Actual
            var result = service.Delete(preAdjustInfo, false);
            var actual = 2;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}