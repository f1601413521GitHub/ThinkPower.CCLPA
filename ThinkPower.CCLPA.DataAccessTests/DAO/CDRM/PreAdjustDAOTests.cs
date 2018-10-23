using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkPower.CCLPA.DataAccess.DAO.CDRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void GetWaitDataTest1()
        {
            // Arrange
            var campaignId = "AA20991022X99Y99Z99A";
            var id = "A177842053";
            var expected = true;

            // Actual
            var result = new PreAdjustDAO().GetWaitData(campaignId,id);
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}