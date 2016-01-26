﻿using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using YuYan.Domain.Database;
using YuYan.Interface.Repository;
using YuYan.Interface.DbContext;
using System.Collections.Generic;
using YuYan.Domain.DTO;

namespace YuYan.Data.Repository
{
    public class YuYanDBRepository : IYuYanDBRepository
    {
        private readonly IYuYanDBContext _db;
        public YuYanDBRepository(IYuYanDBContext db)
        {
            _db = db;
        }

        #region survey
        public async Task<tbSurvey> GetSurveyBySurveyId(int surveyId)
        {
            tbSurvey survey = null;

            try
            {
                survey = await _db.tbSurveys.FirstOrDefaultAsync(x => x.SurveyId == surveyId && (x.IsActive ?? true) && !(x.IsDeleted ?? false));
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return survey;
        }

        public async Task<IEnumerable<tbSurvey>> GetSurveysByUserId(Guid userId)
        {
            IList<tbSurvey> surveyList = new List<tbSurvey>();

            try
            {
                surveyList = await _db.tbSurveys.Where(x => x.UserId == userId && (x.IsActive ?? true) && !(x.IsDeleted ?? false)).ToListAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return surveyList;
        }
        #endregion

        #region question
        public async Task<IEnumerable<tbSurveyQuestion>> GetSurveyQuestionsBySurveyId(int surveyId)
        {
            IList<tbSurveyQuestion> surveyQuestionList = new List<tbSurveyQuestion>();

            try
            {
                surveyQuestionList = await _db.tbSurveyQuestions.Where(x => x.SurveyId == surveyId && (x.IsActive ?? true) && !(x.IsDeleted ?? false)).ToListAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return surveyQuestionList;
        }

        public async Task<tbSurveyQuestion> CreateNewQuestion(dtoSurveyQuestion question) {
            tbSurveyQuestion newQuestion = new tbSurveyQuestion();

            try {
                var questionCount = _db.tbSurveyQuestions.Where(x => x.SurveyId == question.SurveryId 
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false)).Count();
                newQuestion.SurveyId = question.SurveryId;
                newQuestion.Question = question.Question;
                newQuestion.QuestionType = question.QuestionType;
                newQuestion.QuestionOrder = questionCount + 1;
                newQuestion.CreatedDate = DateTime.UtcNow;
                newQuestion.IsActive = true;
                newQuestion.IsDeleted = false;

                _db.tbSurveyQuestions.Add(newQuestion);
                await _db.SaveChangesAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return newQuestion;
        }

        public async Task<tbSurveyQuestion> UpdateQuestion(dtoSurveyQuestion question) {
            tbSurveyQuestion theQuestion = null;

            try {
                theQuestion = await _db.tbSurveyQuestions.FirstOrDefaultAsync(x => x.QuestionId == question.QuestionId 
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false));

                if (theQuestion != null) {
                    theQuestion.Question = question.Question;
                    theQuestion.UpdatedDate = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return theQuestion;
        }

        public async Task DeleteQuestion(int questionId) {
            try {
                tbSurveyQuestion theQuestion = await _db.tbSurveyQuestions.FirstOrDefaultAsync(x => x.QuestionId == questionId && (x.IsActive ?? true) && !(x.IsDeleted ?? false));

                if (theQuestion != null)
                {
                    theQuestion.IsDeleted = true;
                    theQuestion.UpdatedDate = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }
        }

        public async Task DeactiveQuestion(int questionId) {
            try
            {
                tbSurveyQuestion theQuestion = await _db.tbSurveyQuestions.FirstOrDefaultAsync(x => x.QuestionId == questionId && (x.IsActive ?? true) && !(x.IsDeleted ?? false));

                if (theQuestion != null)
                {
                    theQuestion.IsActive = false;
                    theQuestion.UpdatedDate = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }
        }

        #endregion

        #region item
        public async Task<IEnumerable<tbSurveyQuestionItem>> GetQuestionItemsByQuestionId(int questionId)
        {
            IList<tbSurveyQuestionItem> itemList = new List<tbSurveyQuestionItem>();

            try
            {
                itemList = await _db.tbSurveyQuestionItems.Where(x => x.QuestionId ==
                    questionId && (x.IsActive ?? true) && !(x.IsDeleted ?? false)).ToListAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return itemList;
        }

        public async Task<tbSurveyQuestionItem> CreateNewItem(dtoSurveyQuestionItem item)
        {

            tbSurveyQuestionItem newItem = new tbSurveyQuestionItem();
            try
            {
                var itemCount = _db.tbSurveyQuestionItems.Where(x => x.QuestionId == item.QuestionId 
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false)).Count();
                newItem.QuestionId = item.QuestionId;
                newItem.ItemDescription = item.ItemDescription;
                newItem.ItemOrder = itemCount + 1;
                newItem.CreatedDate = DateTime.UtcNow;
                newItem.IsActive = true;
                newItem.IsDeleted = false;

                _db.tbSurveyQuestionItems.Add(newItem);
                await _db.SaveChangesAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return newItem;
        }

        public async Task<tbSurveyQuestionItem> UpdateItem(dtoSurveyQuestionItem item)
        {
            tbSurveyQuestionItem theItem = null;
            try
            {
                theItem = await _db.tbSurveyQuestionItems.FirstOrDefaultAsync(x => x.QuestionItemId == item.QuestionItemId 
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false));
                if (theItem != null)
                {
                    theItem.ItemDescription = item.ItemDescription;
                    theItem.UpdatedDate = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return theItem;
        }

        public async Task DeleteItem(int itemId) {
            try {
                tbSurveyQuestionItem theItem = await _db.tbSurveyQuestionItems.FirstOrDefaultAsync(x => x.QuestionItemId == itemId && (x.IsActive ?? true) && !(x.IsDeleted ?? false));
                if (theItem != null)
                {
                    theItem.IsDeleted = true;
                    theItem.UpdatedDate = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }
        }

        public async Task DeactiveItem(int itemId) {
            try
            {
                tbSurveyQuestionItem theItem = await _db.tbSurveyQuestionItems.FirstOrDefaultAsync(x => x.QuestionItemId == itemId && (x.IsActive ?? true) && !(x.IsDeleted ?? false));
                if (theItem != null)
                {
                    theItem.IsActive = false;
                    theItem.UpdatedDate = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }
        }

        #endregion

        #region dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null)
                    _db.Dispose();
            }
        }
        #endregion
    }
}
