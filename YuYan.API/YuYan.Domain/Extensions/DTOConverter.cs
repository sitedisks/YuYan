using System.Collections.Generic;
using YuYan.Domain.Database;
using YuYan.Domain.DTO;

namespace YuYan.Domain.Extensions
{
    public static class DTOConverter
    {
        #region dto to table
        public static tbSurvey ConverToTbSurvey(this dtoSurvey source, tbSurvey data = null)
        {
            if (data == null)
                data = new tbSurvey();

            if (source == null)
                return null;

            data.SurveyId = source.SurveyId;
            data.Title = source.Title;
            data.ShortDescription = source.ShortDesc;
            data.LongDescription = source.LongDesc;
            data.UserId = source.UserId;
            data.IsActive = source.IsActive;
            data.IsDeleted = source.IsDeleted;

            return data;
        }
        #endregion

        #region table to dto
        public static dtoUserProfile ConvertToDtoUserProfile(this tbUser source, dtoUserProfile data = null) {
            if (data == null)
                data = new dtoUserProfile();

            if (source == null)
                return null;

            data.UserId = source.UserId;
            data.Email = source.Email;
            data.Username = source.Username;
            data.UserRole = source.UserRole;
            data.StreetNo = source.StreetNo;
            data.Street = source.Street;
            data.City = source.City;
            data.State = source.State;
            data.Country = source.Country;
            data.IsDeleted = source.IsDeleted;
            data.IsActive = source.IsActive;

            return data;
        }

        public static dtoSurveyQuestionItem ConvertToDtoSurveyQuestionItem(this tbSurveyQuestionItem source, dtoSurveyQuestionItem data = null)
        {
            if (data == null)
                data = new dtoSurveyQuestionItem();

            if (source == null)
                return null;

            data.QuestionItemId = source.QuestionItemId;
            data.QuestionId = source.QuestionId;
            data.ItemDescription = source.ItemDescription;
            data.ItemOrder = source.ItemOrder;
            data.IsActive = source.IsActive;
            data.IsDeleted = source.IsDeleted;

            return data;
        }

        public static dtoSurveyQuestion ConvertToDtoSurveyQuestion(this tbSurveyQuestion source, dtoSurveyQuestion data = null)
        {

            if (data == null)
                data = new dtoSurveyQuestion();

            if (source == null)
                return null;

            data.QuestionId = source.QuestionId;
            data.SurveyId = source.SurveyId;
            data.Question = source.Question;
            data.QuestionOrder = source.QuestionOrder;
            data.QuestionType = source.QuestionType;
            data.IsActive = source.IsActive;
            data.IsDeleted = source.IsDeleted;

            IList<dtoSurveyQuestionItem> itemList = new List<dtoSurveyQuestionItem>();
            if (source.tbSurveyQuestionItems != null)
            {
                foreach (tbSurveyQuestionItem item in source.tbSurveyQuestionItems)
                {
                    if ((item.IsActive ?? true) && !(item.IsDeleted ?? false))
                        itemList.Add(item.ConvertToDtoSurveyQuestionItem());
                }
            }
            data.dtoItems = itemList;

            return data;
        }

        public static dtoSurvey ConvertToDtoSurvey(this tbSurvey source, dtoSurvey data = null)
        {
            if (data == null)
                data = new dtoSurvey();

            if (source == null)
                return null;

            data.SurveyId = source.SurveyId;
            data.Title = source.Title;
            data.ShortDesc = source.ShortDescription;
            data.LongDesc = source.LongDescription;
            data.UserId = source.UserId;
            data.IsActive = source.IsActive;
            data.IsDeleted = source.IsDeleted;

            IList<dtoSurveyQuestion> questionList = new List<dtoSurveyQuestion>();
            if (source.tbSurveyQuestions != null)
            {
                foreach (tbSurveyQuestion question in source.tbSurveyQuestions)
                {
                    if ((question.IsActive ?? true) && !(question.IsDeleted ?? false))
                        questionList.Add(question.ConvertToDtoSurveyQuestion());
                }
            }
            data.dtoQuestions = questionList;

            return data;
        }
        #endregion

    }
}
