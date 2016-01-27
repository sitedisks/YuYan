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
        Task<IEnumerable<tbSurvey>> GetAllActiveSurveys();
        Task<tbSurvey> GetSurveyBySurveyId(int surveyId);
        Task<IEnumerable<tbSurvey>> GetSurveysByUserId(Guid userId);
        Task<tbSurvey> CreateNewSurvey(dtoSurvey survey);
        Task<tbSurvey> UpdateSurvey(dtoSurvey survey);
        Task DeleteSurvey(int surveyId);
        Task DeactiveSurvey(int surveyId);

        #endregion

        #region question
        Task<IEnumerable<tbSurveyQuestion>> GetSurveyQuestionsBySurveyId(int surveyId);
        Task<tbSurveyQuestion> CreateNewQuestion(dtoSurveyQuestion question);
        Task<tbSurveyQuestion> UpdateQuestion(dtoSurveyQuestion question);
        Task DeleteQuestion(int questionId);
        Task DeactiveQuestion(int questionId);

        #endregion

        #region item
        Task<IEnumerable<tbSurveyQuestionItem>> GetQuestionItemsByQuestionId(int questionId);
        Task<tbSurveyQuestionItem> CreateNewItem(dtoSurveyQuestionItem item);
        Task<tbSurveyQuestionItem> UpdateItem(dtoSurveyQuestionItem item);
        Task DeleteItem(int itemId);
        Task DeactiveItem(int itemId);

        #endregion 
    }
}
