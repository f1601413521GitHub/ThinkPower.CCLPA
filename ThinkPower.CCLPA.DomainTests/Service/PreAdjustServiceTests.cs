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
                //Condition = new PreAdjustCondition()
                //{
                //    PageIndex = null,
                //    PagingSize = null,
                //    CloseDate = DateTime.Now,
                //    CcasReplyCode = null,
                //    CampaignId = null,
                //    Id = null,
                //},
                PreAdjustList = new List<PreAdjustShortData>() {
                    new PreAdjustShortData(){CampaignId = "AA20991022X99Y99Z99A",CustomerId="A177842053" },
                    new PreAdjustShortData(){CampaignId = "AA20991022X99Y99Z99A",CustomerId="B142357855" },
                },
                Remark = $"UnitTest Delete No.{DateTime.Now.Millisecond}",
            };
            var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };

            PreAdjustResult expected = new PreAdjustResult()
            {
                PreAdjustList = new List<PreAdjustShortData>() {
                     new PreAdjustShortData(){CampaignId="AA20991022X99Y99Z99A", CustomerId="A177842053" ,Status="刪除" },
                     new PreAdjustShortData(){CampaignId="AA20991022X99Y99Z99A", CustomerId="B142357855" ,Status="刪除" },
                }
            };

            // Actual
            PreAdjustResult actual = service.DeleteNotEffect(preAdjustInfo);



            var expectedAnonymous = expected.PreAdjustList.
                Select(x => new { x.CampaignId, x.CustomerId, x.Status });

            var actualAnonymous = actual.PreAdjustList.
                Select(x => new { x.CampaignId, x.CustomerId, x.Status });



            // Assert
            Assert.IsTrue(expectedAnonymous.SequenceEqual(actualAnonymous));
        }

        [TestMethod()]
        public void DeleteEffectTest()
        {
            // Arrange
            PreAdjustInfo preAdjustInfo = new PreAdjustInfo()
            {
                PreAdjustList = new List<PreAdjustShortData>() {
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="A177842053" },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="B142357855" },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="D187388854" },
                },
                Remark = $"UnitTest Delete No.{DateTime.Now.Millisecond}",

            };
            var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };

            PreAdjustResult expected = new PreAdjustResult()
            {
                PreAdjustList = new List<PreAdjustShortData>() {
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="A177842053",Status="刪除" },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="B142357855",Status="刪除" },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="D187388854",Status="刪除" },
                }
            };

            // Actual
            PreAdjustResult actual = service.DeleteEffect(preAdjustInfo);


            var expectedAnonymous = expected.PreAdjustList.
                Select(x => new { x.CampaignId, x.CustomerId, x.Status });

            var actualAnonymous = actual.PreAdjustList.
                Select(x => new { x.CampaignId, x.CustomerId, x.Status });


            // Assert
            Assert.IsTrue(expectedAnonymous.SequenceEqual(actualAnonymous));
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

        //    var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };
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
            var campaignId = "ZZ20190101X99Y99Z99A";
            var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };

            var expected = true;

            // Actual
            var actual = true;
            try
            {
                service.Import(campaignId);
            }
            catch (Exception e)
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
            var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };
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
            var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };
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
            var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };
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
            var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };
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
        public void Validate_When_CampaignIdIsZZ20190101X99Y99Z99A_Then_ResultCampaignDetailCountIs5()
        {
            // Arrange
            var campaignId = "ZZ20190101X99Y99Z99A";
            var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };
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
                CustomerId = null,
            };
            var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };
            IEnumerable<PreAdjustEntity> expected = new List<PreAdjustEntity>() {
                new PreAdjustEntity(){ CampaignId = "AA20160401X99Y99Z99A",CustomerId="A177842053"},
                new PreAdjustEntity(){ CampaignId = "AA20160401X99Y99Z99A",CustomerId="B142357855"},
                new PreAdjustEntity(){ CampaignId = "AA20160401X99Y99Z99A",CustomerId="D187388854"},
                new PreAdjustEntity(){ CampaignId = "AA20160401X99Y99Z99A",CustomerId="G183928828"},
                new PreAdjustEntity(){ CampaignId = "AA20160401X99Y99Z99A",CustomerId="Q100016360"},
            };

            // Actual
            IEnumerable<PreAdjustEntity> actual = service.Query(condition);

            var expectedAnonymous = expected.Select(x => new { x.CampaignId, x.CustomerId }).ToList();

            var actualAnonymous = actual.Select(x => new { x.CampaignId, x.CustomerId }).ToList();

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
                CloseDate = new DateTime(2019, 01, 01),
                CcasReplyCode = "00",
                CampaignId = "ZZ20190101X99Y99Z99A",
                CustomerId = null,
            };
            var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };
            IEnumerable<PreAdjustEntity> expected = new List<PreAdjustEntity>() {
                new PreAdjustEntity(){ CampaignId = "ZZ20190101X99Y99Z99A",CustomerId="A177842053"},
                new PreAdjustEntity(){ CampaignId = "ZZ20190101X99Y99Z99A",CustomerId="B142357855"},
                new PreAdjustEntity(){ CampaignId = "ZZ20190101X99Y99Z99A",CustomerId="D187388854"},
                new PreAdjustEntity(){ CampaignId = "ZZ20190101X99Y99Z99A",CustomerId="G183928828"},
                new PreAdjustEntity(){ CampaignId = "ZZ20190101X99Y99Z99A",CustomerId="Q100016360"},
            };


            // Actual
            IEnumerable<PreAdjustEntity> actual = service.Query(condition);

            var expectedAnonymous = expected.Select(x => new { x.CampaignId, x.CustomerId });
            var actualAnonymous = actual.Select(x => new { x.CampaignId, x.CustomerId });

            // Assert
            Assert.IsTrue(expectedAnonymous.SequenceEqual(actualAnonymous));
        }

        [TestMethod()]
        public void AgreeTest()
        {
            // Arrange
            var preAdjustInfo = new PreAdjustInfo()
            {
                PreAdjustList = new List<PreAdjustShortData>() {
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="A177842053" },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="B142357855" },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="D187388854" },
                },
                Remark = "UnitTestAgree",
            };
            var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };

            var expected = new PreAdjustResult()
            {
                PreAdjustList = new List<PreAdjustShortData>() {
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="A177842053" ,Status="生效中" },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="B142357855" ,Status="生效中" },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="D187388854" ,Status="生效中" },
                }
            };

            // Actual 
            var actual = service.Agree(preAdjustInfo);


            var expectedAnonymous = expected.PreAdjustList.
                Select(x => new { x.CampaignId, x.CustomerId, x.Status });

            var actualAnonymous = actual.PreAdjustList.
                Select(x => new { x.CampaignId, x.CustomerId, x.Status });


            // Assert
            Assert.IsTrue(expectedAnonymous.SequenceEqual(actualAnonymous));
        }

        [TestMethod()]
        public void ForceAgree_When_Validate_CustomTestCase_Then()
        {
            // Arrange
            var preAdjustInfo = new PreAdjustInfo()
            {
                PreAdjustList = new List<PreAdjustShortData>() {
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="A177842053" },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="B142357855" },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="D187388854" },
                },
                Remark = "UnitTestAgree",
            };
            var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };
            var expected = new PreAdjustResult()
            {
                PreAdjustList = new List<PreAdjustShortData>() {
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="A177842053",Status="生效中" },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="B142357855",Status="生效中" },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="D187388854",Status="生效中" },
                }
            };

            // Actual 
            var actual = service.ForceAgree(preAdjustInfo, true);


            var expectedAnonymous = expected.PreAdjustList.
                Select(x => new { x.CampaignId, x.CustomerId, x.Status });

            var actualAnonymous = actual.PreAdjustList.
                Select(x => new { x.CampaignId, x.CustomerId, x.Status });

            // Actaul
            Assert.IsTrue(expectedAnonymous.SequenceEqual(actualAnonymous));
        }

        [TestMethod()]
        public void ForceAgree_When_NotValidate_CustomTestCase_Then()
        {
            // Arrange
            var preAdjustInfo = new PreAdjustInfo()
            {
                PreAdjustList = new List<PreAdjustShortData>() {
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="A177842053",Status="刪除"   },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="B142357855",Status="生效中" },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="D187388854",Status="生效中" },
                },
                Remark = "UnitTestAgree",
            };
            var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };
            var expected = new PreAdjustResult()
            {
                PreAdjustList = new List<PreAdjustShortData>() {
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="B142357855",Status="生效中" },
                     new PreAdjustShortData(){CampaignId="ZZ20190101X99Y99Z99A", CustomerId="D187388854",Status="生效中" },
                }
            };

            // Actual 
            var actual = service.ForceAgree(preAdjustInfo, false);


            var expectedAnonymous = expected.PreAdjustList.
                Select(x => new { x.CampaignId, x.CustomerId, x.Status });

            var actualAnonymous = actual.PreAdjustList.
                Select(x => new { x.CampaignId, x.CustomerId, x.Status });

            // Actaul
            Assert.IsTrue(expectedAnonymous.SequenceEqual(actualAnonymous));
        }

        [TestMethod()]
        public void Query_When_CloseDateIs20181030_CustomerIdIsA177842053_Then_Result_NotEffect_And_EffectCase()
        {
            // Arrange
            var condition = new PreAdjustCondition()
            {
                PageIndex = null,
                PagingSize = null,
                CloseDate = new DateTime(2018, 10, 30),
                CcasReplyCode = null,
                CampaignId = null,
                CustomerId = "A177842053",
            };
            var condition2 = new PreAdjustCondition()
            {
                PageIndex = null,
                PagingSize = null,
                CloseDate = new DateTime(2018, 10, 30),
                CcasReplyCode = "00",
                CampaignId = null,
                CustomerId = "A177842053",
            };

            var service = new PreAdjustService() { UserInfo = new UserInfo() { Id = "58860", Name = "User58860" } };
            IEnumerable<PreAdjustEntity> expected = new List<PreAdjustEntity>() {
                new PreAdjustEntity(){ CampaignId = "AA20991022X99Y99Z99A",CustomerId="A177842053"},
                new PreAdjustEntity(){ CampaignId = "AA20991024X99Y99Z99A",CustomerId="A177842053"},
                new PreAdjustEntity(){ CampaignId = "ZZ20190101X99Y99Z99A",CustomerId="A177842053"},
                new PreAdjustEntity(){ CampaignId = "AA20951022X99Y99Z99A",CustomerId="A177842053"},
                new PreAdjustEntity(){ CampaignId = "AA20961022X99Y99Z99A",CustomerId="A177842053"},
                new PreAdjustEntity(){ CampaignId = "AA20971022X99Y99Z99A",CustomerId="A177842053"},
                new PreAdjustEntity(){ CampaignId = "AA20981022X99Y99Z99A",CustomerId="A177842053"},
            };

            // Actual
            IEnumerable<PreAdjustEntity> result = service.Query(condition);
            IEnumerable<PreAdjustEntity> result2 = service.Query(condition2);

            List<PreAdjustEntity> actual = new List<PreAdjustEntity>();
            actual.AddRange(result);
            actual.AddRange(result2);

            var expectedAnonymous = expected.Select(x => new { x.CampaignId, x.CustomerId });
            var actualAnonymous = actual.Select(x => new { x.CampaignId, x.CustomerId });

            // Assert
            Assert.IsTrue(expectedAnonymous.SequenceEqual(actualAnonymous));
        }
    }
}