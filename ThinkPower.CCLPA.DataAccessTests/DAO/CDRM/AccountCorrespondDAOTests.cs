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
    public class AccountCorrespondDAOTests
    {
        [TestMethod()]
        public void GetTest()
        {
            // Arrange
            string id = "58860";
            var expected = true;

            // Actual
            var result = new AccountCorrespondDAO().Get(id);
            var actual = (result != null);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}