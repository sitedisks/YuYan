using YuYan.Data.DbContext;
using YuYan.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YuYan.Domain.Database;
using System.Threading.Tasks;
using YuYan.Domain.DTO;
using System;

namespace YuYan.Test
{
    [TestClass]
    public class RepositoryTest
    {
        /*
        [TestMethod]
        public async void TestInitialize()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                var surveys = await repos.GetAllActiveSurveys();
                foreach (var survey in surveys) {
                    survey.IsDeleted = false;
                    var questions = await repos.GetSurveyQuestionsBySurveyId(survey.SurveyId);
                    foreach (var question in questions) {
                        question.IsDeleted = false;
                        var items = await repos.GetQuestionItemsByQuestionId(question.QuestionId);
                        foreach (var item in items) {
                            item.IsDeleted = false;
                        }
                    }
                }
                await db.SaveChangesAsync();
            }
        }
         */

        [TestMethod]
        public void TestSelf()
        {
            int x = 1; int y = 1;
            int z = 2;

            Assert.AreEqual(z, x + y);
        }

        #region user
        [TestMethod]
        public async Task TestRepo_CreateUser()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                dtoUser testuser = new dtoUser() { Email = "test001@test.com", Password = "qwerty" };

                tbUser userobj = await repos.CreateNewUser(testuser);
                Assert.IsNotNull(userobj);
                Assert.AreEqual("test001@test.com", userobj.Email, true);
            }
        }

        [TestMethod]
        public async Task TestRepo_SuccessLogin_User()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                dtoUser testuser = new dtoUser() { Email = "test001@test.com", Password = "qwerty" };

                tbUser userobj = await repos.LoginUser(testuser);
                Assert.IsNotNull(userobj);
                Assert.AreEqual("test001@test.com", userobj.Email, true);
            }
        }

        [TestMethod]
        public async Task TestRepo_FailedLogin_UnknownUser()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                dtoUser testuser = new dtoUser() { Email = "test001@unknown.com", Password = "qwerty" };

                tbUser userobj = await repos.LoginUser(testuser);
                Assert.IsNull(userobj); // is null means the user not existed in the database
            }
        }

        [TestMethod]
        public async Task TestRepo_FailedLogin_WrongPasswordUser()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                dtoUser testuser = new dtoUser() { Email = "test001@test.com", Password = "12341234" };

                tbUser userobj = await repos.LoginUser(testuser);
                Assert.IsNotNull(userobj); // is new tbUser means the password not match
                Assert.AreEqual(Guid.Empty, userobj.UserId);
            }
        }


        #endregion

        #region survey
        [TestMethod]
        public async Task TestRepo_GetSurveyBySurveyId()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {

                tbSurvey obj = await repos.GetSurveyBySurveyId(1);
                Assert.IsNotNull(obj);
                Assert.AreEqual("Test Survey", obj.Title, true);
            }
        }
        #endregion

        #region question
        [TestMethod]
        public async Task TestRepo_GetSurveyQuestionBySurveyId()
        {

            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {

                var obj = await repos.GetSurveyQuestionsBySurveyId(1);
                Assert.IsNotNull(obj);
            }
        }
        #endregion

        #region item
        [TestMethod]
        public async Task TestRepo_GetQuestionItemsByQuestionId()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {

                var obj = await repos.GetQuestionItemsByQuestionId(1);
                Assert.IsNotNull(obj);
            }
        }

        [TestMethod]
        public async Task TestRepo_CreateItem()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                dtoSurveyQuestionItem newItem = new dtoSurveyQuestionItem();
                newItem.QuestionId = 1;
                newItem.ItemDescription = "The third choice";

                var obj = await repos.CreateNewItem(newItem);
                Assert.IsNotNull(obj);
            }
        }

        [TestMethod]
        public async Task TestRepo_UpdateItem()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                dtoSurveyQuestionItem newItem = new dtoSurveyQuestionItem();
                newItem.QuestionItemId = 7;
                newItem.ItemDescription = "The ddddd chdfdsfsdsfoice";

                var obj = await repos.UpdateItem(newItem);
                Assert.IsNotNull(obj);
            }
        }

        [TestMethod]
        public async Task TestRepo_DeleteItem()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                dtoSurveyQuestionItem newItem = new dtoSurveyQuestionItem();

                await repos.DeleteItem(4);

            }
        }

        #endregion

    }
}
