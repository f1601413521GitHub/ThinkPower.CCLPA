using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThinkPower.CCLPA.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkPower.CCLPA.Domain.VO;
using ThinkPower.CCLPA.Domain.Entity;
using ThinkPower.CCLPA.DataAccess;
using Newtonsoft.Json;
using ThinkPower.CCLPA.Domain.Condition;

namespace ThinkPower.CCLPA.Domain.Service.Tests
{
    [TestClass()]
    public class PreAdjustServiceTests
    {
        [TestMethod()]
        public void DeleteNotEffectTest()
        {
            // Arrange
            PreAdjustInfo preAdjustInfo = new PreAdjustInfo()
            {
                Condition = new PreAdjustCondition()
                {
                    PageIndex = null,
                    PagingSize = null,
                    CloseDate = DateTime.Now,
                    CcasReplyCode = null,
                    CampaignId = null,
                    Id = null,
                },
                PreAdjustList = new List<PreAdjustEntity>() {
                    new PreAdjustEntity(){CampaignId = "AA20991022X99Y99Z99A",Id="A177842053" },
                    new PreAdjustEntity(){CampaignId = "AA20991022X99Y99Z99A",Id="B142357855" },
                },
                Remark = $"UnitTest Delete No.{DateTime.Now.Millisecond}",
            };
            var service = new PreAdjustService();
            service.UserInfo = new UserInfo() { Id = "14260", Name = "User14260" };
            var expected = 2;

            // Actual
            var actual = service.DeleteNotEffect(preAdjustInfo);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void DeleteEffectTest()
        {
            // Arrange
            PreAdjustInfo preAdjustInfo = new PreAdjustInfo()
            {
                Condition = new PreAdjustCondition()
                {
                    PageIndex = null,
                    PagingSize = null,
                    CloseDate = DateTime.Now,
                    CcasReplyCode = "00",
                    CampaignId = null,
                    Id = null,
                },
                PreAdjustList = new List<PreAdjustEntity>() {
                    new PreAdjustEntity(){CampaignId = "AA20981022X99Y99Z99A",Id="A177842053" },
                    new PreAdjustEntity(){CampaignId = "AA20981022X99Y99Z99A",Id="B142357855" },
                },
                Remark = $"UnitTest Delete No.{DateTime.Now.Millisecond}",
            };
            var service = new PreAdjustService();
            service.UserInfo = new UserInfo() { Id = "14260", Name = "User14260" };
            var expected = 2;

            // Actual
            var actual = service.DeleteEffect(preAdjustInfo);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        #region ConvertPagingConditionTest
        //[TestMethod()]
        //public void ConvertPagingConditionTest()
        //{
        //    // Arrange
        //    var entity = new PagingConditionEntity()
        //    {
        //        PageIndex = 1,
        //        PagingSize = 12,
        //        OrderBy = PagingConditionEntity.OrderByKind.OrderDate,
        //    };

        //    var service = new PreAdjustService();
        //    service.UserInfo = new UserInfoVO() { Id = "14260", Name = "User14260" };

        //    var expected = new PagingCondition() {

        //        PageIndex = 1,
        //        PagingSize = 12,
        //        OrderBy = PagingCondition.OrderByKind.OrderDate,
        //    };

        //    // Actual
        //    var actual = service.ConvertPagingCondition(entity);


        //    var e = new
        //    {
        //        expected.PageIndex,
        //        expected.PagingSize,
        //        expected.OrderBy,
        //    };

        //    var a = new
        //    {
        //        actual.PageIndex,
        //        actual.PagingSize,
        //        actual.OrderBy,
        //    };

        //    // Assert
        //    Assert.AreEqual(e, a);
        //} 
        #endregion

        [TestMethod()]
        public void ImportTest()
        {
            // Arrange
            var campaignId = "AA20160401X99Y99Z99A";
            var service = new PreAdjustService();
            service.UserInfo = new UserInfo() { Id = "14260", Name = "User14260" };
            var expected = true;

            // Actual
            var actual = true;
            try
            {
                service.Import(campaignId);
            }
            catch (Exception)
            {
                actual = false;
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Validate_When_CampaignIdNotFound_Then_ResultErrorMsgAndCampaignDetailCount()
        {
            // Arrange
            var campaignId = "TEST";
            var service = new PreAdjustService();
            PreAdjustValidateResult expected = new PreAdjustValidateResult()
            {
                ErrorMessage = "ILRC行銷活動編碼，輸入錯誤。",
                CampaignDetailCount = 0,
            };

            // Actual
            PreAdjustValidateResult actual = service.Validate(campaignId);

            var expectedAnonymous = new
            {
                expected.ErrorMessage,
                expected.CampaignDetailCount,
            };

            var actualAnonymous = new
            {
                actual.ErrorMessage,
                actual.CampaignDetailCount,
            };

            // Assert
            Assert.AreEqual(expectedAnonymous, actualAnonymous);
        }

        [TestMethod()]
        public void Validate_When_ConvertExpectedCloseDateFail_Then_ResultExceptionMsg()
        {
            // Arrange
            var campaignId = "AA20181019X99Y99Z99A";
            var service = new PreAdjustService();
            string expected = "Convert ExpectedCloseDate Fail";

            // Actual
            string actual = null;
            try
            {
                PreAdjustValidateResult result = service.Validate(campaignId);
            }
            catch (InvalidOperationException e)
            {
                actual = e.Message;
            }

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Validate_When_CampaignClose_Then_ResultErrorMsgAndCampaignDetailCount()
        {
            // Arrange
            var campaignId = "BB20181019X99Y99Z99A";
            var service = new PreAdjustService();
            PreAdjustValidateResult expected = new PreAdjustValidateResult()
            {
                ErrorMessage = "此行銷活動已結案，無法進入匯入作業。",
                CampaignDetailCount = 0,
            };

            // Actual
            PreAdjustValidateResult actual = service.Validate(campaignId);

            var expectedAnonymous = new
            {
                expected.ErrorMessage,
                expected.CampaignDetailCount,
            };

            var actualAnonymous = new
            {
                actual.ErrorMessage,
                actual.CampaignDetailCount,
            };

            // Assert
            Assert.AreEqual(expectedAnonymous, actualAnonymous);
        }

        [TestMethod()]
        public void Validate_When_CampaignImportLogExsit_Then_ResultErrorMsgAndCampaignDetailCount()
        {
            // Arrange
            var campaignId = "CC20181019X99Y99Z99A";
            var service = new PreAdjustService();
            PreAdjustValidateResult expected = new PreAdjustValidateResult()
            {
                ErrorMessage = "此行銷活動已於2018/10/19匯入過，無法再進行匯入。",
                CampaignDetailCount = 0,
            };

            // Actual
            PreAdjustValidateResult actual = service.Validate(campaignId);

            var expectedAnonymous = new
            {
                expected.ErrorMessage,
                expected.CampaignDetailCount,
            };

            var actualAnonymous = new
            {
                actual.ErrorMessage,
                actual.CampaignDetailCount,
            };

            // Assert
            Assert.AreEqual(expectedAnonymous, actualAnonymous);
        }

        [TestMethod()]
        public void Validate_When_CampaignIdIsAA20160401X99Y99Z99A_Then_ResultCampaignDetailCountIs5()
        {
            // Arrange
            var campaignId = "AA20160401X99Y99Z99A";
            var service = new PreAdjustService();
            PreAdjustValidateResult expected = new PreAdjustValidateResult()
            {
                ErrorMessage = null,
                CampaignDetailCount = 5,
            };

            // Actual
            PreAdjustValidateResult actual = service.Validate(campaignId);

            var expectedAnonymous = new
            {
                expected.ErrorMessage,
                expected.CampaignDetailCount,
            };

            var actualAnonymous = new
            {
                actual.ErrorMessage,
                actual.CampaignDetailCount,
            };

            // Assert
            Assert.AreEqual(expectedAnonymous, actualAnonymous);
        }

        [TestMethod()]
        public void Query_When_CloseDateIs20160525_CampaignIdIsAA20160401X99Y99Z99A_Then_ResultAnonymousObjectList_CampaignId_Id()
        {
            // Arrange
            var condition = new PreAdjustCondition()
            {
                PageIndex = null,
                PagingSize = null,
                CloseDate = new DateTime(2016, 5, 25),
                CcasReplyCode = null,
                CampaignId = "AA20160401X99Y99Z99A",
                Id = null,
            };
            var service = new PreAdjustService();
            IEnumerable<PreAdjustEntity> expected = new List<PreAdjustEntity>() {
                new PreAdjustEntity(){ CampaignId = "AA20160401X99Y99Z99A",Id="A177842053"},
                new PreAdjustEntity(){ CampaignId = "AA20160401X99Y99Z99A",Id="B142357855"},
                new PreAdjustEntity(){ CampaignId = "AA20160401X99Y99Z99A",Id="D187388854"},
                new PreAdjustEntity(){ CampaignId = "AA20160401X99Y99Z99A",Id="G183928828"},
                new PreAdjustEntity(){ CampaignId = "AA20160401X99Y99Z99A",Id="Q100016360"},
            };

            // Actual
            IEnumerable<PreAdjustEntity> actual = service.Query(condition);

            var expectedAnonymous = expected.Select(x => new { x.CampaignId, x.Id }).ToList();

            var actualAnonymous = actual.Select(x => new { x.CampaignId, x.Id }).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedAnonymous, actualAnonymous);
        }


        [TestMethod()]
        public void Query_When_CloseDateIs20991022_CampaignIdIsAA20981022X99Y99Z99A_CcasReplyCodeIs00_Then_ResultAnonymousObjectList_CampaignId_Id()
        {
            // Arrange
            var condition = new PreAdjustCondition()
            {
                PageIndex = null,
                PagingSize = null,
                CloseDate = new DateTime(2099, 10, 22),
                CcasReplyCode = "00",
                CampaignId = "AA20981022X99Y99Z99A",
                Id = null,
            };
            var service = new PreAdjustService();
            IEnumerable<PreAdjustEntity> expected = new List<PreAdjustEntity>() {
                new PreAdjustEntity(){ CampaignId = "AA20981022X99Y99Z99A",Id="A177842053"},
                new PreAdjustEntity(){ CampaignId = "AA20981022X99Y99Z99A",Id="B142357855"},
                new PreAdjustEntity(){ CampaignId = "AA20981022X99Y99Z99A",Id="D187388854"},
            };


            // Actual
            IEnumerable<PreAdjustEntity> actual = service.Query(condition);

            var expectedAnonymous = expected.Select(x => new { x.CampaignId, x.Id }).ToList();

            var actualAnonymous = actual.Select(x => new { x.CampaignId, x.Id }).ToList();

            // Assert
            CollectionAssert.AreEqual(expectedAnonymous, actualAnonymous);
        }
    }
}