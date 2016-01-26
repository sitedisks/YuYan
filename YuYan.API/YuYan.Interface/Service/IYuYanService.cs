using System.Threading.Tasks;
using YuYan.Domain.DTO;

namespace YuYan.Interface.Service
{
    public interface IYuYanService
    {
        #region survey
        Task<dtoSurvey> GetSurveyBySurveyId(int surveyId);

        Task<dtoSurvey> CreateSurvey(dtoSurvey survey);

        Task<dtoSurvey> UpdateSurvey(dtoSurvey survey);

        Task DeleteSurvey(int surveyId);

        Task DeactiveSurvey(int surveyId);
        #endregion

        #region question
        Task<dtoSurveyQuestion> CreateSurveyQuestion(dtoSurveyQuestion question);

        Task<dtoSurveyQuestion> UpdateSurveyQuestion(dtoSurveyQuestion question);

        Task DeleteSurveyQuestion(int questionId);

        Task DeactiveSurveyQuestion(int questionId);
        #endregion

        #region item
        Task<dtoSurveyQuestionItem> CreateSurveyQuestionItem(dtoSurveyQuestionItem questionItem);

        Task<dtoSurveyQuestionItem> UpdateSurveyQuestionItem(dtoSurveyQuestionItem questionItem);

        Task DeleteSurveyQuestionItem(int itemId);

        Task DeactiveSurveyQuestionItem(int itemId);
        #endregion
    }
}
