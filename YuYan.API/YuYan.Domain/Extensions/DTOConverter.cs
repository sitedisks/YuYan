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

            data.SurveyId = source.SurveryId;
            data.Title = source.Title;
            data.ShortDescription = source.ShortDesc;
            data.LongDescription = source.LongDesc;
            data.UserId = source.UserId;

            return data;
        }
        #endregion

        #region table to dto
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

            return data;
        }

        public static dtoSurveyQuestion ConvertToDtoSurveyQuestion(this tbSurveyQuestion source, dtoSurveyQuestion data = null)
        {

            if (data == null)
                data = new dtoSurveyQuestion();

            if (source == null)
                return null;

            data.QuestionId = source.QuestionId;
            data.SurveryId = source.SurveyId;
            data.Question = source.Question;
            data.QuestionOrder = source.QuestionOrder;
            data.QuestionType = source.QuestionType;

            IList<dtoSurveyQuestionItem> itemList = new List<dtoSurveyQuestionItem>();
            if (source.tbSurveyQuestionItems.Count > 0)
            {
                foreach (tbSurveyQuestionItem item in source.tbSurveyQuestionItems) {
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

            data.SurveryId = source.SurveyId;
            data.Title = source.Title;
            data.ShortDesc = source.ShortDescription;
            data.LongDesc = source.LongDescription;
            data.UserId = source.UserId;

            IList<dtoSurveyQuestion> questionList = new List<dtoSurveyQuestion>();
            if (source.tbSurveyQuestions.Count > 0)
            {
                foreach (tbSurveyQuestion question in source.tbSurveyQuestions)
                {
                    questionList.Add(question.ConvertToDtoSurveyQuestion());
                }
            }
            data.dtoQuestions = questionList;

            return data;
        }
        #endregion

    }
}
