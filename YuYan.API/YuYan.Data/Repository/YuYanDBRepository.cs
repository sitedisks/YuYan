using CryptSharp;
using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using YuYan.Domain.Database;
using YuYan.Interface.Repository;
using YuYan.Interface.DbContext;
using YuYan.Domain.DTO;
using YuYan.Domain.Enum;

namespace YuYan.Data.Repository
{
    public class YuYanDBRepository : IYuYanDBRepository
    {
        private readonly IYuYanDBContext _db;
        public YuYanDBRepository(IYuYanDBContext db)
        {
            _db = db;
        }

        #region user
        public async Task<tbUser> CreateNewUser(dtoUser user)
        {
            tbUser newUser = new tbUser();

            try
            {
                newUser.UserId = Guid.NewGuid();
                newUser.Email = user.Email;
                newUser.IPAddress = user.IPAddress;
                newUser.UserRole = UserRole.User;
                newUser.Password = Crypter.Blowfish.Crypt(user.Password);
                newUser.CreatedDate = DateTime.UtcNow;
                newUser.IsDeleted = false;
                newUser.IsActive = true;

                _db.tbUsers.Add(newUser);
                await _db.SaveChangesAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return newUser;
        }

        public async Task<tbUser> UpdateUser(dtoUserProfile profile)
        {
            tbUser user = null;

            try
            {
                user = await _db.tbUsers.FirstOrDefaultAsync(x => x.UserId == profile.UserId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false));

                if (user != null)
                {
                    // the user exist
                    user.Username = profile.Username;
                    user.IPAddress = profile.IPAddress;
                    user.StreetNo = profile.StreetNo;
                    user.Street = profile.Street;
                    user.City = profile.City;
                    user.State = profile.State;
                    user.Country = profile.Country;
                    user.UpdatedDate = DateTime.UtcNow;

                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return user;
        }

        public async Task<tbUser> LoginUser(dtoUser user)
        {
            tbUser userObj = null;

            try
            {
                if (!string.IsNullOrEmpty(user.Email))
                    userObj = await GetUserByEmail(user.Email);
                else if (!string.IsNullOrEmpty(user.Username))
                    userObj = await GetUserByUserName(user.Username);

                if (userObj == null)
                    return userObj;

                if (!Crypter.CheckPassword(user.Password, userObj.Password))
                {
                    return new tbUser(); // password not match
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return userObj;
        }

        public async Task<bool> LogoutUser(Guid sessionId)
        {
            bool IsLogout = false;
            try
            {
                tbSession session = await _db.tbSessions.FirstOrDefaultAsync(x => x.SessionId == sessionId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false));

                if (session != null)
                {
                    session.IsDeleted = true;
                    session.IsActive = false;
                    session.UpdatedDate = DateTime.UtcNow;
                    await _db.SaveChangesAsync();

                    IsLogout = true;
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return IsLogout;
        }

        public async Task CreateUpdateUserSession(tbUser user)
        {
            try
            {
                tbSession session = await _db.tbSessions.FirstOrDefaultAsync(x => x.UserId == user.UserId && !(x.IsDeleted ?? false));
                if (session != null)
                {
                    session.IsDeleted = true;
                    session.IsActive = false;
                    session.UpdatedDate = DateTime.UtcNow;
                }

                // new session
                session = new tbSession();
                session.SessionId = Guid.NewGuid();
                session.UserId = user.UserId;
                session.CreatedDate = DateTime.UtcNow;
                session.IPAddress = user.IPAddress;
                session.Expiry = DateTime.UtcNow.AddHours(2); // extend next 2 hours
                session.IsDeleted = false;
                session.IsActive = true;
                _db.tbSessions.Add(session);
                await _db.SaveChangesAsync();

            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }
        }

        private async Task<tbUser> GetUserByEmail(string email)
        {
            tbUser user = null;

            try
            {
                user = await _db.tbUsers.FirstOrDefaultAsync(u => u.Email == email && !(u.IsDeleted ?? false));
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return user;
        }

        private async Task<tbUser> GetUserByUserName(string username)
        {
            tbUser user = null;

            try
            {
                user = await _db.tbUsers.FirstOrDefaultAsync(u => u.Username == username && !(u.IsDeleted ?? false));
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return user;
        }
        #endregion

        #region survey
        public async Task<IEnumerable<tbSurvey>> GetAllActiveSurveys()
        {
            IList<tbSurvey> surveyList = new List<tbSurvey>();
            try
            {
                surveyList = await _db.tbSurveys.Where(x => x.IsActive == true).ToListAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }
            return surveyList;
        }

        public async Task<IEnumerable<tbSurvey>> GetSurveysByUserId(Guid userId)
        {
            IList<tbSurvey> surveyList = new List<tbSurvey>();

            try
            {
                surveyList = await _db.tbSurveys.Where(x => x.UserId == userId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false)).ToListAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return surveyList;
        }

        public async Task<tbSurvey> GetSurveyBySurveyId(int surveyId)
        {
            tbSurvey survey = null;

            try
            {
                survey = await _db.tbSurveys.FirstOrDefaultAsync(x => x.SurveyId == surveyId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false));
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return survey;
        }

        public async Task<tbSurvey> CreateNewSurvey(dtoSurvey survey)
        {
            tbSurvey newSurvey = new tbSurvey();

            try
            {
                newSurvey.Title = survey.Title;
                newSurvey.ShortDescription = survey.ShortDesc;
                newSurvey.LongDescription = survey.LongDesc;
                newSurvey.CreatedDate = DateTime.UtcNow;
                newSurvey.IsActive = true;
                newSurvey.IsDeleted = false;

                _db.tbSurveys.Add(newSurvey);
                await _db.SaveChangesAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }
            return newSurvey;
        }

        public async Task<tbSurvey> UpdateSurvey(dtoSurvey survey)
        {
            tbSurvey theSurvey = null;

            try
            {
                theSurvey = await _db.tbSurveys.FirstOrDefaultAsync(x => x.SurveyId == survey.SurveyId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false));

                if (theSurvey != null)
                {
                    theSurvey.Title = survey.Title;
                    theSurvey.ShortDescription = survey.ShortDesc;
                    theSurvey.LongDescription = survey.LongDesc;
                    theSurvey.UpdatedDate = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }
            return theSurvey;
        }

        public async Task DeleteSurvey(int surveyId)
        {
            try
            {
                tbSurvey theSurvey = await _db.tbSurveys.FirstOrDefaultAsync(x => x.SurveyId == surveyId && (x.IsActive ?? true) && !(x.IsDeleted ?? false));

                if (theSurvey != null)
                {
                    theSurvey.IsDeleted = true;
                    theSurvey.UpdatedDate = DateTime.UtcNow;

                    // Set all question (under this survey) to delete
                    var questionList = await GetSurveyQuestionsBySurveyId(surveyId);
                    foreach (var question in questionList)
                    {
                        await DeleteQuestion(question.QuestionId);
                    }

                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }
        }

        public async Task DeactiveSurvey(int surveyId)
        {
            try
            {
                tbSurvey theSurvey = await _db.tbSurveys.FirstOrDefaultAsync(x => x.SurveyId == surveyId && (x.IsActive ?? true) && !(x.IsDeleted ?? false));

                if (theSurvey != null)
                {
                    theSurvey.IsActive = false;
                    theSurvey.UpdatedDate = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }
        }
        #endregion

        #region question
        public async Task<IEnumerable<tbSurveyQuestion>> GetSurveyQuestionsBySurveyId(int surveyId)
        {
            IList<tbSurveyQuestion> surveyQuestionList = new List<tbSurveyQuestion>();

            try
            {
                surveyQuestionList = await _db.tbSurveyQuestions.Where(x => x.SurveyId == surveyId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false)).ToListAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return surveyQuestionList;
        }

        public async Task<tbSurveyQuestion> GetSurveyQuestionByQuestionId(int questionId)
        {
            tbSurveyQuestion question = new tbSurveyQuestion();

            try
            {
                question = await _db.tbSurveyQuestions.FirstOrDefaultAsync(x => x.QuestionId == questionId && (x.IsActive ?? true) && !(x.IsDeleted ?? false));
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return question;
        }

        public async Task<tbSurveyQuestion> CreateNewQuestion(dtoSurveyQuestion question)
        {
            tbSurveyQuestion newQuestion = new tbSurveyQuestion();

            try
            {
                var questionCount = _db.tbSurveyQuestions.Where(x => x.SurveyId == question.SurveyId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false)).Count();
                newQuestion.SurveyId = question.SurveyId;
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

        public async Task<tbSurveyQuestion> UpdateQuestion(dtoSurveyQuestion question)
        {
            tbSurveyQuestion theQuestion = null;

            try
            {
                theQuestion = await _db.tbSurveyQuestions.FirstOrDefaultAsync(x => x.QuestionId == question.QuestionId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false));

                if (theQuestion != null)
                {
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

        public async Task DeleteQuestion(int questionId)
        {
            try
            {
                tbSurveyQuestion theQuestion = await _db.tbSurveyQuestions.FirstOrDefaultAsync(x => x.QuestionId == questionId && (x.IsActive ?? true) && !(x.IsDeleted ?? false));

                if (theQuestion != null)
                {
                    theQuestion.IsDeleted = true;
                    theQuestion.UpdatedDate = DateTime.UtcNow;
                    // Set all items (under this question) to delete
                    var itemList = await GetQuestionItemsByQuestionId(questionId);
                    foreach (var item in itemList)
                    {
                        await DeleteItem(item.QuestionItemId);
                    }

                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }
        }

        public async Task DeactiveQuestion(int questionId)
        {
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
                itemList = await _db.tbSurveyQuestionItems.Where(x => x.QuestionId == questionId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false)).ToListAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return itemList;
        }

        public async Task<tbSurveyQuestionItem> GetQuestionItemByItemId(int itemId)
        {
            tbSurveyQuestionItem item = new tbSurveyQuestionItem();

            try
            {
                item = await _db.tbSurveyQuestionItems.FirstOrDefaultAsync(x => x.QuestionItemId == itemId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false));
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return item;
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

        public async Task DeleteItem(int itemId)
        {
            try
            {
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

        public async Task DeactiveItem(int itemId)
        {
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
