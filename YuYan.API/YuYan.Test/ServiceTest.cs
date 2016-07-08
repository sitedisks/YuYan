using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YuYan.Data.DbContext;
using YuYan.Data.Repository;
using YuYan.Service;
using YuYan.Domain.DTO;

namespace YuYan.Test
{
    [TestClass]
    public class ServiceTest
    {
        #region user

        [TestMethod]
        public async Task TestService_RegisterNewUser() {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                YuYanService svc = new YuYanService(repos);
                dtoUser newUser = new dtoUser() { Email = "fromService@test.com", Password = "qwerty" };
                dtoUserProfile userObj = await svc.RegisterNewUser(newUser);
                Assert.IsNotNull(userObj);
                Assert.AreEqual("fromService@test.com", userObj.Email);
            }
        }

        [TestMethod]
        public async Task TestService_LoginUser_Success() {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                YuYanService svc = new YuYanService(repos);
                dtoUser newUser = new dtoUser() { Email = "fromService@test.com", Password = "qwerty" };
                dtoUserProfile userObj = await svc.LoginUser(newUser);
                Assert.IsNotNull(userObj);
                Assert.AreEqual("fromService@test.com", userObj.Email);
            }
        }

        [TestMethod]
        public async Task TestService_LoginUser_Failed_NoUser()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                YuYanService svc = new YuYanService(repos);
                dtoUser newUser = new dtoUser() { Email = "xxxxxx@test.com", Password = "qwerty" };
                dtoUserProfile userObj = await svc.LoginUser(newUser);
                Assert.IsNull(userObj);
            }
        }

        [TestMethod]
        public async Task TestService_LoginUser_Failed_WrongPassword()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                YuYanService svc = new YuYanService(repos);
                dtoUser newUser = new dtoUser() { Email = "fromService@test.com", Password = "123123123" };
                dtoUserProfile userObj = await svc.LoginUser(newUser);
                Assert.IsNotNull(userObj);
                Assert.AreEqual(Guid.Empty, userObj.UserId);
            }
        }

        [TestMethod]
        public async Task TestService_LogoutUser_Success() {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                YuYanService svc = new YuYanService(repos);

                bool isLogout = await svc.LogoutUser(new Guid("1A737355-2BF8-4CF6-AB28-A2694CD2A0D8"));

                Assert.AreEqual(true, isLogout);
            }
        }

        #endregion

        #region survey
        [TestMethod]
        public async Task TestService_GetSurveyBySurveyId()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                YuYanService svc = new YuYanService(repos);
                dtoSurvey obj = await svc.GetSurveyBySurveyId(1);
                Assert.IsNotNull(obj);
            }
        }

        [TestMethod]
        public async Task TestService_DeleteSurveyQuestion() {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                YuYanService svc = new YuYanService(repos);
                await svc.DeleteSurveyQuestion(1);
                
            }
        }

        #endregion

        #region image
        [TestMethod]
        public async Task TestService_GetImage()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                YuYanService svc = new YuYanService(repos);
                var image = await svc.GetImage(new Guid("0E71160C-28F3-49BD-ABB8-63594E615FB8"));
                Assert.IsNotNull(image);
            } 
        }
        #endregion
    }
}
