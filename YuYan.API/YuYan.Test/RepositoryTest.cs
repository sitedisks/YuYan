using YuYan.Data.DbContext;
using YuYan.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YuYan.Domain.Database;
using System.Threading.Tasks;

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

    }
}
