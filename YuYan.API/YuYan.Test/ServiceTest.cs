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
        [TestMethod]
        public async Task TestGetSurveyBySurveyId()
        {
            using (YuYanDBContext db = new YuYanDBContext())
            using (YuYanDBRepository repos = new YuYanDBRepository(db))
            {
                YuYanService srv = new YuYanService(repos);
                dtoSurvey obj = await srv.GetSurveyBySurveyId(1);
                Assert.IsNotNull(obj);
            }
        }
    }
}
