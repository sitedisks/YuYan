using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YuYan.Domain.Database;

namespace YuYan.Interface.Repository
{
    public interface IYuYunDBRepository: IDisposable
    {
        #region survey
        Task<tbSurvey> GetSurveyBySurveyId(int surveyId);
        Task<IEnumerable<tbSurvey>> GetSurveysByUserId(Guid userId);
        #endregion

        #region question
        Task<IEnumerable<tbSurveyQuestion>> GetSurveyQuestionsBySurveyId(int surveyId);
        #endregion

        #region item
        Task<IEnumerable<tbSurveyQuestionItem>> GetQuestionItemsByQuestionId(int questionId);
        #endregion 
    }
}
