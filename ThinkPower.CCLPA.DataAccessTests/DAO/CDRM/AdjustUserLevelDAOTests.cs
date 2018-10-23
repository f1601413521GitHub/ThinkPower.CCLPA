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
    public class AdjustUserLevelDAOTests
    {
        [TestMethod()]
        public void GetTest()
        {
            // Arrange
            string id = "I58860";
            var expected = true;

            // Actual
            var result = new AdjustUserLevelDAO().Get(id);
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}