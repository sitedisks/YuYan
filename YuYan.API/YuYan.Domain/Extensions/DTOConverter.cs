using System.Collections.Generic;
using YuYan.Domain.Database;
using YuYan.Domain.DTO;
using YuYan.Domain.Enum;

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
            data.Slug = source.Slug;
            data.URLToken = source.URLToken;
            data.ShortDescription = source.ShortDesc;
            data.LongDescription = source.LongDesc;
            data.ShowReport = source.ShowReport;
            data.UserId = source.UserId;
            data.IsActive = source.IsActive;
            data.IsDeleted = source.IsDeleted;

            return data;
        }
        #endregion

        #region table to dto
        public static dtoUser ConvertToDtoUser(this tbUser source, dtoUser data = null)
        {
            if (data == null)
                data = new dtoUser();

            if (source == null)
                return null;

            data.UserId = source.UserId;
            data.Username = source.Username;
            data.Email = source.Email;
            data.IPAddress = source.IPAddress;
            data.IsActive = source.IsActive;
            data.IsDeleted = source.IsDeleted;

            return data;
        }

        public static dtoUserProfile ConvertToDtoUserProfile(this tbUser source, dtoUserProfile data = null)
        {
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
            data.Score = source.Score;
            data.GotoQuestionId = source.GotoQuestionId;
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
            data.Slug = source.Slug;
            data.URLToken = source.URLToken;
            data.ShortDesc = source.ShortDescription;
            data.LongDesc = source.LongDescription;
            data.UserId = source.UserId;
            data.ShowReport = source.ShowReport;
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

        public static dtoSession ConvertToDtoSession(this tbSession source, dtoSession data = null)
        {
            if (data == null)
                data = new dtoSession();

            if (source == null)
                return null;

            data.SessionId = source.SessionId;
            data.UserId = source.UserId;
            data.Expiry = source.Expiry;
            data.IPAddress = source.IPAddress;
            data.IsActive = source.IsActive;
            data.IsDeleted = source.IsDeleted;

            return data;
        }

        public static dtoSurveyClient ConvertToDtoSurveyClient(this tbSurveyClient source, dtoSurveyClient data = null)
        {

            if (data == null)
                data = new dtoSurveyClient();

            if (source == null)
                return null;

            data.ClientId = source.ClientId;
            data.Email = source.Email;
            data.IPAddress = source.IPAddress;
            data.SurveyId = source.SurveyId;
            data.TotalScore = source.TotalScore;
            data.City = source.City;
            data.State = source.State;
            data.Country = source.Country;

            IList<dtoSurveyClientAnswer> answerList = new List<dtoSurveyClientAnswer>();
            if (source.tbClientAnswers != null)
            {
                foreach (tbSurveyClientAnswer answer in source.tbClientAnswers)
                {
                    if (answer.IsChecked)
                        answerList.Add(answer.ConvertToDtoAnswwer());
                }
            }

            data.dtoClientAnswers = answerList;

            return data;
        }

        private static dtoSurveyClientAnswer ConvertToDtoAnswwer(this tbSurveyClientAnswer source, dtoSurveyClientAnswer data = null)
        {
            if (data == null)
                data = new dtoSurveyClientAnswer();

            if (source == null)
                return null;

            data.AnswerId = source.AnswerId;
            data.ClientId = source.ClientId;
            data.QuestionId = source.QuestionId;
            data.QuestionItemId = source.QuestionItemId;
            data.IsChecked = source.IsChecked;

            return data;
        }

        public static dtoSurveyShare ConvertToDtoSurveyShare(this tbSurveyShare source, dtoSurveyShare data = null)
        {
            if (data == null)
                data = new dtoSurveyShare();

            if (source == null)
                return null;

            data.SurveyShareId = source.SurveyShareId;
            data.SurveyId = source.SurveyId;
            data.IPAddress = source.IPAddress;
            data.VisitedDate = source.VisitedDate;

            return data;
        }

        public static dtoSurveyResult ConvertToDtoSurveyResult(this tbSurveyResult source, dtoSurveyResult data = null)
        {
            if (data == null)
                data = new dtoSurveyResult();

            if (source == null)
                return null;

            data.SurveyResultId = source.SurveyResultId;
            data.MinScore = source.MinScore;
            data.MaxScore = source.MaxScore;
            data.SurveyId = source.SurveyId;
            data.Title = source.Title;
            data.Description = source.Description;

            return data;
        }

        public static dtoLocationGeo ConvertToLocationGeo(this ip2location_db3 source, dtoLocationGeo data = null)
        {
            if (data == null)
                data = new dtoLocationGeo();
            if (source == null)
                return null;

            data.State = source.region_name;
            data.City = source.city_name;
            data.CountryCode = source.country_code;
            data.Country = source.country_name;

            return data;
        }

        public static dtoImage ConvertToDtoImage(this tbImage source, dtoImage data = null)
        {
            if (data == null)
                data = new dtoImage();
            if (source == null)
                return null;

            data.ImageId = source.ImageId;
            data.ImageType = (ImageType)source.ImageType;
            data.UserId = source.UserId;
            data.FileName = source.FileName;
            data.RefId = source.RefId;

            return data;
        }

        #endregion

    }
}
