using System;
using System.Threading.Tasks;
using YuYan.Domain.DTO;
using YuYan.Domain.Extensions;
using YuYan.Domain.Database;
using YuYan.Interface.Service;
using YuYan.Interface.Repository;

namespace YuYan.Service
{
    public class YuYanService: IYuYanService
    {
        private readonly IYuYanDBRepository _yuyanRepos;

        public YuYanService(IYuYanDBRepository yuyanRepos) {
            _yuyanRepos = yuyanRepos;
        }

        public async Task<dtoSurvey> GetSurveyBySurveyId(int surveyId) {
            dtoSurvey survey = new dtoSurvey();

            try {
                tbSurvey tbSurvey = await _yuyanRepos.GetSurveyBySurveyId(surveyId);
                survey = tbSurvey.ConvertToDtoSurvey();
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retriving Survey", ex);
            }

            return survey;
        }
    }
}
