using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using YuYan.Data.DbContext;
using YuYan.Data.Repository;
using YuYan.Service;
using YuYan.API.Controllers;

namespace YuYan.Test
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public async Task TestGetSurveyBySurveyId()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                YuYanService svc = new YuYanService(repos);
                var controller = new SurveyController(svc);

                var result = await controller.GetSurveyBySurveyId(1);
                Assert.IsNotNull(result);
            }
        }
    }
}
