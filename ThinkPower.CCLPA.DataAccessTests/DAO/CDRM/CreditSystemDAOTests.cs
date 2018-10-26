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
    public class CreditSystemDAOTests
    {
        [TestMethod()]
        public void ValidatePreAdjustEffectConditionTest()
        {
            // Arrange
            var id = "A177842053";
            var dao = new AdjustSystemDAO();
            var expected = true;

            // Actual
            var result = dao.PreAdjustEffectCondition(id);
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}