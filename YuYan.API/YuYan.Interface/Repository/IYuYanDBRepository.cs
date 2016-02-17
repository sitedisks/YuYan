﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YuYan.Domain.Database;
using YuYan.Domain.DTO;

namespace YuYan.Interface.Repository
{
    public interface IYuYanDBRepository : IDisposable
    {
        #region user
        Task<tbUser> GetUserByEmail(string email);
        Task<tbUser> CreateNewUser(dtoUser user);
        Task<tbUser> UpdateUser(dtoUserProfile profile);
        Task<tbUser> LoginUser(dtoUser user);
        Task<bool> LogoutUser(Guid sessionId);
        Task<tbSession> CreateUpdateUserSession(tbUser user);
        tbSession GetSessionBySessionId(Guid sessionId);
        tbSession ExtendSession(tbSession session);
        bool DeleteSession(tbSession session);
        tbUser GetUserBySessionId(Guid sessionId);
        #endregion

        #region client
        Task<tbSurveyClient> SaveSurveyClient(dtoSurveyClient surveyClient);

        #endregion

        #region survey
        Task<IList<tbSurvey>> GetAllActiveSurveys();
        Task<IList<tbSurvey>> GetSurveysByUserId(Guid userId);
        Task<tbSurvey> GetSurveyBySurveyId(int surveyId);
        Task<tbSurvey> GetSurveyByUrlToken(string url);
        Task<tbSurvey> CreateNewSurvey(dtoSurvey survey);
        Task<tbSurvey> UpdateSurvey(dtoSurvey survey);
        Task DeleteSurvey(int surveyId);
        Task DeactiveSurvey(int surveyId);

        #endregion

        #region question
        Task<IList<tbSurveyQuestion>> GetSurveyQuestionsBySurveyId(int surveyId);
        Task<tbSurveyQuestion> GetSurveyQuestionByQuestionId(int questionId);
        Task<tbSurveyQuestion> CreateNewQuestion(dtoSurveyQuestion question);
        Task<tbSurveyQuestion> UpdateQuestion(dtoSurveyQuestion question);
        Task DeleteQuestion(int questionId);
        Task DeactiveQuestion(int questionId);

        #endregion

        #region item
        Task<IList<tbSurveyQuestionItem>> GetQuestionItemsByQuestionId(int questionId);
        Task<tbSurveyQuestionItem> GetQuestionItemByItemId(int itemId);
        Task<tbSurveyQuestionItem> CreateNewItem(dtoSurveyQuestionItem item);
        Task<tbSurveyQuestionItem> UpdateItem(dtoSurveyQuestionItem item);
        Task DeleteItem(int itemId);
        Task DeactiveItem(int itemId);
        //Task ActiveItem(int itemId);
        #endregion
    }
}
