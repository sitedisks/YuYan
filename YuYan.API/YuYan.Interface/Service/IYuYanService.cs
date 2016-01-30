﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YuYan.Domain.DTO;

namespace YuYan.Interface.Service
{
    public interface IYuYanService
    {
        #region user

        Task<dtoUserProfile> RegisterNewUser(dtoUser user);

        Task<dtoUserProfile> LoginUser(dtoUser user);

        Task<bool> LogoutUser(Guid sessionId);

        //Task<dtoUserProfile> UpdateUserProfile(dtoUserProfile userprofile);
        #endregion

        #region survey
        Task<dtoSurvey> GetSurveyBySurveyId(int surveyId);

        Task<dtoSurvey> CreateSurvey(dtoSurvey survey);

        Task<dtoSurvey> UpdateSurvey(dtoSurvey survey);

        Task DeleteSurvey(int surveyId);

        Task DeactiveSurvey(int surveyId);
        #endregion

        #region question
        Task<IList<dtoSurveyQuestion>> GetSurveyQuestionsBySurveyId(int surveyId);

        Task<dtoSurveyQuestion> GetSurveyQuestionByQuestionId(int questionId);

        Task<dtoSurveyQuestion> CreateSurveyQuestion(dtoSurveyQuestion question);

        Task<dtoSurveyQuestion> UpdateSurveyQuestion(dtoSurveyQuestion question);

        Task DeleteSurveyQuestion(int questionId);

        Task DeactiveSurveyQuestion(int questionId);
        #endregion

        #region item
        Task<IList<dtoSurveyQuestionItem>> GetQuestionItemsByQuestionId(int questionId);

        Task<dtoSurveyQuestionItem> GetQuestionItemByItemId(int itemId);

        Task<dtoSurveyQuestionItem> CreateSurveyQuestionItem(dtoSurveyQuestionItem questionItem);

        Task<dtoSurveyQuestionItem> UpdateSurveyQuestionItem(dtoSurveyQuestionItem questionItem);

        Task DeleteSurveyQuestionItem(int itemId);

        Task DeactiveSurveyQuestionItem(int itemId);
        #endregion
    }
}
