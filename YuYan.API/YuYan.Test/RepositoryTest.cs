using YuYan.Data.DbContext;
using YuYan.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YuYan.Domain.Database;
using System.Threading.Tasks;
using YuYan.Domain.DTO;

namespace YuYan.Test
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void TestSelf()
        {
            int x = 1; int y = 1;
            int z = 2;

            Assert.AreEqual(z, x + y);
        }

        #region survey
        [TestMethod]
        public async Task TestGetSurveyBySurveyId()
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
        public async Task TestGetSurveyQuestionBySurveyId()
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
        public async Task TestGetQuestionItemsByQuestionId()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {

                var obj = await repos.GetQuestionItemsByQuestionId(1);
                Assert.IsNotNull(obj);
            }
        }

        [TestMethod]
        public async Task TestCreateItem()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                dtoSurveyQuestionItem newItem = new dtoSurveyQuestionItem();
                newItem.QuestionId = 1;
                newItem.ItemDescription = "The first choice";

                var obj = await repos.CreateNewItem(newItem);
                Assert.IsNotNull(obj);
            }
        }

        [TestMethod]
        public async Task TestUpdateItem() {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                dtoSurveyQuestionItem newItem = new dtoSurveyQuestionItem();
                newItem.QuestionItemId = 2;
                newItem.ItemDescription = "The second choice";

                var obj = await repos.UpdateItem(newItem);
                Assert.IsNotNull(obj);
            }
        }

        [TestMethod]
        public async Task TestDeleteItem()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                dtoSurveyQuestionItem newItem = new dtoSurveyQuestionItem();
                newItem.QuestionItemId = 3; // please reorder 
                newItem.IsDeleted = true;
                newItem.ItemDescription = "The second choice";

                var obj = await repos.UpdateItem(newItem);
                Assert.IsNotNull(obj);
            }
        }

        #endregion

    }
}
