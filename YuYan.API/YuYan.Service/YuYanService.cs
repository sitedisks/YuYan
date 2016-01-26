﻿using System;
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

        #region survey
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

        #endregion

        #region question
        public async Task<dtoSurveyQuestion> CreateSurveyQuestion(dtoSurveyQuestion question) {
            dtoSurveyQuestion q = null;

            try {
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

        public async Task<dtoSurveyQuestion> UpdateSurveyQuestion(dtoSurveyQuestion question) {
            dtoSurveyQuestion q = null;

            try {
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

        public async Task DeleteSurveyQuestion(int questionId) {
            try {
                await _yuyanRepos.DeactiveQuestion(questionId);
                // get items by questionId
                var itemList = await _yuyanRepos.GetQuestionItemsByQuestionId(questionId);
                if (itemList != null) {
                    foreach (var item in itemList) {
                        await _yuyanRepos.DeactiveItem(item.QuestionItemId);
                    }
                }

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
        public async Task<dtoSurveyQuestionItem> CreateSurveyQuestionItem(dtoSurveyQuestionItem questionItem)
        {
            dtoSurveyQuestionItem item = null;

            try {
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

            try {
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

        public async Task DeleteSurveyQuestionItem(int itemId) {
            try {
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

        public async Task DeactiveSurveyQuestionItem(int itemId) {
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
