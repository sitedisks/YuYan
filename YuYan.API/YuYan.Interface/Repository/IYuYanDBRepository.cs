using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YuYan.Domain.Database;
using YuYan.Domain.DTO;

namespace YuYan.Interface.Repository
{
    public interface IYuYanDBRepository: IDisposable
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
        Task<tbSurveyQuestionItem> CreateNewItem(dtoSurveyQuestionItem item);
        Task<tbSurveyQuestionItem> UpdateItem(dtoSurveyQuestionItem item);

        #endregion 
    }
}
