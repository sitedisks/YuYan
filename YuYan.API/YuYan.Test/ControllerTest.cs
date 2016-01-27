using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using YuYan.Data.DbContext;
using YuYan.Data.Repository;
using YuYan.Service;
using YuYan.API.Controllers;
using YuYan.Domain.DTO;
using YuYan.Domain.Enum;

namespace YuYan.Test
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public async Task TestController_GetSurveyBySurveyId()
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

        [TestMethod]
        public async Task TestController_CreateSurvey() {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                YuYanService svc = new YuYanService(repos);
                var controller = new SurveyController(svc);

                dtoSurvey testObj = new dtoSurvey();
                testObj.Title = "Make me happy";
                testObj.ShortDesc = "No short description";

                var result = await controller.CreateSurvey(testObj);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public async Task TestController_CreateQuestion() { 
            using(YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db)) {
                YuYanService svc = new YuYanService(repos);
                var controller = new SurveyController(svc);

                dtoSurveyQuestion questionObj = new dtoSurveyQuestion();
                questionObj.Question = "Add from Test";
                questionObj.QuestionType = QuestionType.checkbox;
                //questionObj.SurveyId = 6;

                var result = await controller.CreateQuestion(6, questionObj);
                Assert.IsNotNull(result);
            }
        }
    }
}
