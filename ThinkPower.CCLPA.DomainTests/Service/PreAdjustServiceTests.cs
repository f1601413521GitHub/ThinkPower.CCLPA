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
    }
}