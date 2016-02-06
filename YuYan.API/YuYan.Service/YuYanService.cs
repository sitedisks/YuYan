﻿using System;
using System.Threading.Tasks;
using YuYan.Domain.DTO;
using YuYan.Domain.Extensions;
using YuYan.Domain.Database;
using YuYan.Interface.Service;
using YuYan.Interface.Repository;
using System.Collections.Generic;

namespace YuYan.Service
{
    public class YuYanService : IYuYanService
    {
        private readonly IYuYanDBRepository _yuyanRepos;

        public YuYanService(IYuYanDBRepository yuyanRepos)
        {
            _yuyanRepos = yuyanRepos;
        }

        #region user
        public async Task<bool> CheckUserAvailability(string email) {
            bool isAvailable = true;

            try {
                var user = await _yuyanRepos.GetUserByEmail(email);
                if (user != null)
                    isAvailable = false;
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error get user By email.", ex);
            }

            return isAvailable;
        }

        public async Task<dtoUserProfile> RegisterNewUser(dtoUser user)
        {
            dtoUserProfile userProfile = new dtoUserProfile();

            try
            {
                tbUser userObj = await _yuyanRepos.CreateNewUser(user);
                userProfile = userObj.ConvertToDtoUserProfile();
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error create user.", ex);
            }

            return userProfile;
        }

        public async Task<dtoUserProfile> LoginUser(dtoUser user)
        {
            dtoUserProfile userProfile = null;

            try
            {
                tbUser userObj = await _yuyanRepos.LoginUser(user);
                if (userObj == null)
                    return userProfile; //user not exist

                if (userObj.UserId == Guid.Empty)
                    return new dtoUserProfile();  // password not match

                userObj.IPAddress = user.IPAddress; //set the current Login IP for Session
                await _yuyanRepos.CreateUpdateUserSession(userObj); // create the session
                userProfile = userObj.ConvertToDtoUserProfile();

            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Login user.", ex);
            }

            return userProfile;
        }

        public async Task<bool> LogoutUser(Guid sessionId)
        {
            bool IsLogout = false;

            try
            {
                IsLogout = await _yuyanRepos.LogoutUser(sessionId);
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Logout user.", ex);
            }

            return IsLogout;
        }

        public async Task<dtoUserProfile> UpdateUserProfile(dtoUserProfile userProfile) {
            dtoUserProfile profile = null;

            try {
                tbUser uprofile = await _yuyanRepos.UpdateUser(userProfile);
                if (uprofile != null)
                    profile = uprofile.ConvertToDtoUserProfile();
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error update user profile.", ex);
            }

            return profile;
        }

        public bool ValidateSession(Guid sessionId) {
            bool IsExpired = true;

            try {
                tbSession session = _yuyanRepos.GetSessionBySessionId(sessionId);
                if (session != null)
                {
                    if (session.Expiry > DateTime.UtcNow)
                        IsExpired = false;
                }
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("ERROR: Unable to validate session expiry!", ex);
            }

            return IsExpired;
        }

        public dtoUserProfile GetUserBySessionId(Guid sessionId) {
            dtoUserProfile userProfile = null;

            try {
                var u = _yuyanRepos.GetUserBySessionId(sessionId);
                if (u != null)
                    userProfile = u.ConvertToDtoUserProfile();
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retriving Survey", ex);
            }

            return userProfile;
        }
        #endregion

        #region survey
        public async Task<dtoSurvey> GetSurveyBySurveyId(int surveyId)
        {
            dtoSurvey survey = new dtoSurvey();

            try
            {
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

        public async Task<dtoSurvey> CreateSurvey(dtoSurvey survey)
        {
            dtoSurvey s = null;

            try
            {
                var surveyObj = await _yuyanRepos.CreateNewSurvey(survey);
                s = surveyObj.ConvertToDtoSurvey();
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error create survey", ex);
            }

            return s;
        }

        public async Task<dtoSurvey> UpdateSurvey(dtoSurvey survey)
        {
            dtoSurvey s = null;

            try
            {
                var surveyObj = await _yuyanRepos.UpdateSurvey(survey);
                s = surveyObj.ConvertToDtoSurvey();
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error update survey.", ex);
            }

            return s;
        }

        public async Task DeleteSurvey(int surveyId)
        {
            try
            {
                await _yuyanRepos.DeleteSurvey(surveyId);
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error delete survey", ex);
            }
        }

        public async Task DeactiveSurvey(int surveyId)
        {
            try
            {
                await _yuyanRepos.DeactiveSurvey(surveyId);
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deactive survey", ex);
            }
        }
        #endregion

        #region question
        public async Task<IList<dtoSurveyQuestion>> GetSurveyQuestionsBySurveyId(int surveyId)
        {
            IList<dtoSurveyQuestion> questionList = new List<dtoSurveyQuestion>();

            try
            {
                var questions = await _yuyanRepos.GetSurveyQuestionsBySurveyId(surveyId);
                foreach (var question in questions)
                {
                    questionList.Add(question.ConvertToDtoSurveyQuestion());
                }
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error get question list", ex);
            }

            return questionList;
        }

        public async Task<dtoSurveyQuestion> GetSurveyQuestionByQuestionId(int questionId)
        {
            dtoSurveyQuestion question = new dtoSurveyQuestion();

            try
            {
                var questionObj = await _yuyanRepos.GetSurveyQuestionByQuestionId(questionId);
                question = questionObj.ConvertToDtoSurveyQuestion();
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error get question", ex);
            }

            return question;
        }

        public async Task<dtoSurveyQuestion> CreateSurveyQuestion(dtoSurveyQuestion question)
        {
            dtoSurveyQuestion q = null;

            try
            {
                var questionObj = await _yuyanRepos.CreateNewQuestion(question);
                q = questionObj.ConvertToDtoSurveyQuestion();
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error create survey question", ex);
            }

            return q;
        }

        public async Task<dtoSurveyQuestion> UpdateSurveyQuestion(dtoSurveyQuestion question)
        {
            dtoSurveyQuestion q = null;

            try
            {
                var questionObj = await _yuyanRepos.UpdateQuestion(question);
                q = questionObj.ConvertToDtoSurveyQuestion();
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error update survey question", ex);
            }

            return q;
        }

        public async Task DeleteSurveyQuestion(int questionId)
        {
            try
            {
                await _yuyanRepos.DeleteQuestion(questionId);
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error delete survey question", ex);
            }
        }

        public async Task DeactiveSurveyQuestion(int questionId)
        {
            try
            {
                await _yuyanRepos.DeactiveQuestion(questionId);
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error delete survey question", ex);
            }
        }
        #endregion

        #region item
        public async Task<IList<dtoSurveyQuestionItem>> GetQuestionItemsByQuestionId(int questionId)
        {
            IList<dtoSurveyQuestionItem> itemList = new List<dtoSurveyQuestionItem>();

            try
            {
                var items = await _yuyanRepos.GetQuestionItemsByQuestionId(questionId);
                foreach (var item in items)
                {
                    itemList.Add(item.ConvertToDtoSurveyQuestionItem());
                }
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error get item list", ex);
            }

            return itemList;
        }

        public async Task<dtoSurveyQuestionItem> GetQuestionItemByItemId(int itemId)
        {
            dtoSurveyQuestionItem item = new dtoSurveyQuestionItem();

            try
            {
                var itemObj = await _yuyanRepos.GetQuestionItemByItemId(itemId);
                item = itemObj.ConvertToDtoSurveyQuestionItem();
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error get item list", ex);
            }

            return item;
        }

        public async Task<dtoSurveyQuestionItem> CreateSurveyQuestionItem(dtoSurveyQuestionItem questionItem)
        {
            dtoSurveyQuestionItem item = null;

            try
            {
                var itemObj = await _yuyanRepos.CreateNewItem(questionItem);
                item = itemObj.ConvertToDtoSurveyQuestionItem();
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error create survey item", ex);
            }

            return item;

        }

        public async Task<dtoSurveyQuestionItem> UpdateSurveyQuestionItem(dtoSurveyQuestionItem questionItem)
        {
            dtoSurveyQuestionItem item = null;

            try
            {
                var itemObj = await _yuyanRepos.UpdateItem(questionItem);
                item = itemObj.ConvertToDtoSurveyQuestionItem();
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error update survey item", ex);
            }

            return item;
        }

        public async Task DeleteSurveyQuestionItem(int itemId)
        {
            try
            {
                await _yuyanRepos.DeleteItem(itemId);
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error delete survey item", ex);
            }
        }

        public async Task DeactiveSurveyQuestionItem(int itemId)
        {
            try
            {
                await _yuyanRepos.DeactiveItem(itemId);
            }
            catch (ApplicationException aex)
            {
                throw aex;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deactive survey item", ex);
            }
        }

        #endregion
    }
}
