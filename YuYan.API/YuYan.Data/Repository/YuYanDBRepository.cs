using System;
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
                var itemCount = _db.tbSurveyQuestionItems.Where(x => x.QuestionId ==
                    item.QuestionId && (x.IsActive ?? true) && !(x.IsDeleted ?? false)).Count();
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
                theItem = _db.tbSurveyQuestionItems.FirstOrDefault(x => x.QuestionItemId == item.QuestionItemId);
                if (theItem != null)
                {
                    //theItem.QuestionId = item.QuestionId;
                    theItem.ItemDescription = item.ItemDescription;
                    //theItem.ItemOrder = item.ItemOrder;
                    theItem.UpdatedDate = DateTime.UtcNow;
                    theItem.IsActive = item.IsActive;
                    theItem.IsDeleted = item.IsDeleted;
                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return theItem;
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
