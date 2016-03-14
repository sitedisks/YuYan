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

        public async Task<tbSession> CreateUpdateUserSession(tbUser user)
        {
            tbSession session = null;
            try
            {
                session = await _db.tbSessions.FirstOrDefaultAsync(x => x.UserId == user.UserId && !(x.IsDeleted ?? false));
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
                session.Expiry = DateTime.UtcNow.AddDays(1); // extend one day
                session.IsDeleted = false;
                session.IsActive = true;
                _db.tbSessions.Add(session);
                await _db.SaveChangesAsync();

            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return session;
        }

        public tbSession GetSessionBySessionId(Guid sessionId)
        {
            tbSession session = null;

            try
            {
                session = _db.tbSessions.FirstOrDefault(x => x.SessionId == sessionId
                     && (x.IsActive ?? true) && !(x.IsDeleted ?? false));
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return session;
        }

        public tbSession ExtendSession(tbSession session)
        {

            try
            {
                session.Expiry = DateTime.UtcNow.AddDays(1);
                session.UpdatedDate = DateTime.UtcNow;
                _db.SaveChanges();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return session;
        }

        public bool DeleteSession(tbSession session)
        {
            bool isDeleted = false;

            try
            {
                session.IsDeleted = true;
                session.IsActive = false;
                session.UpdatedDate = DateTime.UtcNow;

                _db.SaveChanges();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return isDeleted;
        }

        public tbUser GetUserBySessionId(Guid sessionId)
        {
            tbUser user = null;

            try
            {
                var session = _db.tbSessions.FirstOrDefault(x => x.SessionId == sessionId
                     && (x.IsActive ?? true) && !(x.IsDeleted ?? false));
                if (session != null)
                    user = _db.tbUsers.FirstOrDefault(x => x.UserId == session.UserId
                        && (x.IsActive ?? true) && !(x.IsDeleted ?? false));
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return user;
        }

        public async Task<tbUser> GetUserByEmail(string email)
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

        #region client
        public async Task<tbSurveyClient> SaveSurveyClient(dtoSurveyClient surveyClient)
        {
            tbSurveyClient sc = new tbSurveyClient();

            try
            {
                sc.Email = surveyClient.Email;
                sc.IPAddress = surveyClient.IPAddress;
                sc.SurveyId = surveyClient.SurveyId;
                sc.TotalScore = surveyClient.TotalScore;
                sc.City = surveyClient.City;
                sc.State = surveyClient.State;
                sc.Country = surveyClient.Country;
                sc.CreatedDate = DateTime.UtcNow;

                _db.tbSurveyClients.Add(sc);

                if (surveyClient.dtoClientAnswers.Count() > 0)
                {
                    foreach (dtoSurveyClientAnswer clientAnswer in surveyClient.dtoClientAnswers)
                    {
                        clientAnswer.ClientId = sc.ClientId;
                        await SaveClientAnswer(clientAnswer);
                    }
                }

                await _db.SaveChangesAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return sc;
        }

        public async Task<tbSurveyShare> SaveSurveyShare(dtoSurveyShare surveyShare)
        {
            tbSurveyShare sShare = new tbSurveyShare();

            try
            {
                sShare.SurveyId = surveyShare.SurveyId;
                sShare.IPAddress = surveyShare.IPAddress;
                sShare.VisitedDate = DateTime.UtcNow;

                _db.tbSurveyShares.Add(sShare);
                await _db.SaveChangesAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return sShare;
        }

        private async Task<tbSurveyClientAnswer> SaveClientAnswer(dtoSurveyClientAnswer clientAnswer)
        {
            tbSurveyClientAnswer ca = new tbSurveyClientAnswer();

            try
            {
                ca.ClientId = clientAnswer.ClientId;
                ca.QuestionId = clientAnswer.QuestionId;
                ca.QuestionItemId = clientAnswer.QuestionItemId;
                ca.IsChecked = clientAnswer.IsChecked;
                ca.CreatedDate = DateTime.UtcNow;

                _db.tbSurveyClientAnswers.Add(ca);
                await _db.SaveChangesAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return ca;
        }
        #endregion

        #region survey
        public async Task<IList<tbSurvey>> GetAllActiveSurveys()
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

        public async Task<IList<tbSurvey>> GetSurveysByUserId(Guid userId, int? page = null, int? row = null, bool? actived = null)
        {
            IList<tbSurvey> surveyList = new List<tbSurvey>();

            try
            {
                var sList = _db.tbSurveys.Where(x => x.UserId == userId && !(x.IsDeleted ?? false));

                if (actived.HasValue)
                    sList = sList.Where(x => x.IsActive == actived);

                sList = sList.OrderByDescending(x => x.UpdatedDate);

                // pagination
                if (row.HasValue)
                {
                    int skip = 0;
                    if (page.HasValue)
                        skip = (page.Value - 1) * row.Value;

                    sList = sList.Skip(skip).Take(row.Value);
                }

                surveyList = await sList.ToListAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return surveyList;
        }

        public async Task<int> GetTotalSurveyCountByUserId(Guid userId)
        {
            int surveyCount = 0;
            try
            {
                var surveyList = await _db.tbSurveys.Where(x => x.UserId == userId && (x.IsActive ?? true) && !(x.IsDeleted ?? false)).ToListAsync();
                surveyCount = surveyList.Count();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return surveyCount;
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

        public async Task<IList<tbSurveyShare>> GetSurveySharesBySurveyId(int surveyId)
        {
            IList<tbSurveyShare> surveyShareList = new List<tbSurveyShare>();

            try
            {
                surveyShareList = await _db.tbSurveyShares.Where(x => x.SurveyId == surveyId).ToListAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return surveyShareList;
        }

        public async Task<IList<tbSurveyClient>> GetSurveyClientBySurveyId(int surveyId)
        {
            IList<tbSurveyClient> surveyClientList = new List<tbSurveyClient>();

            try
            {
                surveyClientList = await _db.tbSurveyClients.Where(x => x.SurveyId == surveyId).ToListAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return surveyClientList;
        }

        public IDictionary<int, int> AnswerCount(int surveyId)
        {
            IDictionary<int, int> answerDic = new Dictionary<int, int>();

            try
            {
                var selectedClientAnswers = from sca in _db.tbSurveyClientAnswers
                                            join
                                                sc in _db.tbSurveyClients on sca.ClientId equals sc.ClientId
                                            where sc.SurveyId == surveyId
                                            && sca.IsChecked
                                            select sca;
                                            //group sca by sca.QuestionItemId into pair
                                            //select (new KeyValuePair<int, int>(pair.Key, pair.Count()));

                var answerPair = selectedClientAnswers.ToList().GroupBy(x => x.QuestionItemId).Select(x => new KeyValuePair<int, int>(x.Key, x.Count()));
                answerDic = answerPair.ToDictionary(x => x.Key, x => x.Value);
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return answerDic;
        }

        public async Task<tbSurvey> GetSurveyByUrlToken(string url)
        {
            tbSurvey survey = null;

            try
            {
                survey = await _db.tbSurveys.FirstOrDefaultAsync(x => x.URLToken == url
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
                newSurvey.Slug = survey.Slug;
                newSurvey.URLToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("/", "").Replace("+", "");
                newSurvey.UserId = survey.UserId;
                newSurvey.ShortDescription = survey.ShortDesc;
                newSurvey.LongDescription = survey.LongDesc;
                newSurvey.CreatedDate = DateTime.UtcNow;
                newSurvey.UpdatedDate = DateTime.UtcNow;
                newSurvey.IsActive = true;
                newSurvey.IsDeleted = false;

                _db.tbSurveys.Add(newSurvey);

                if (survey.dtoQuestions != null)
                {
                    if (survey.dtoQuestions.Count() > 0)
                    {
                        // do the question create
                        foreach (dtoSurveyQuestion question in survey.dtoQuestions)
                        {
                            question.SurveyId = newSurvey.SurveyId;
                            await CreateNewQuestion(question);
                        }
                    }
                }


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
        public async Task<IList<tbSurveyQuestion>> GetSurveyQuestionsBySurveyId(int surveyId)
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
                question = await _db.tbSurveyQuestions.FirstOrDefaultAsync(x => x.QuestionId == questionId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false));
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
                newQuestion.UpdatedDate = DateTime.UtcNow;
                newQuestion.IsActive = true;
                newQuestion.IsDeleted = false;

                _db.tbSurveyQuestions.Add(newQuestion);

                if (question.dtoItems != null)
                {
                    if (question.dtoItems.Count() > 0)
                    {
                        foreach (dtoSurveyQuestionItem item in question.dtoItems)
                        {
                            item.QuestionId = newQuestion.QuestionId;
                            await CreateNewItem(item);
                        }
                    }
                }

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
                    theQuestion.QuestionType = question.QuestionType;
                    theQuestion.UpdatedDate = DateTime.UtcNow;

                    if (theQuestion.tbSurveyQuestionItems != null)
                    {
                        if (theQuestion.tbSurveyQuestionItems.Count() > 0)
                        {
                            IList<int> deleteItemIds = theQuestion.tbSurveyQuestionItems
                                .Where(x => (x.IsActive ?? true) && !(x.IsDeleted ?? false))
                                .Select(x => x.QuestionItemId)
                                .Except(question.dtoItems.Select(z => z.QuestionItemId)).ToList();
                            foreach (int dItemId in deleteItemIds)
                            {
                                await DeleteItem(dItemId);
                                await DeactiveItem(dItemId);
                            }
                        }
                    }

                    if (question.dtoItems != null)
                    {
                        if (question.dtoItems.Count() > 0)
                        {
                            foreach (dtoSurveyQuestionItem item in question.dtoItems)
                            {
                                if (item.QuestionItemId != 0)
                                {
                                    //update
                                    await UpdateItem(item);
                                }
                                else
                                {
                                    //add new item
                                    await CreateNewItem(item);
                                }
                            }
                        }
                    }

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
                tbSurveyQuestion theQuestion = await _db.tbSurveyQuestions.FirstOrDefaultAsync(x => x.QuestionId == questionId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false));

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
        public async Task<IList<tbSurveyQuestionItem>> GetQuestionItemsByQuestionId(int questionId)
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
                newItem.ItemOrder = item.ItemOrder != 0 ? item.ItemOrder : itemCount + 1;
                newItem.Score = item.Score;
                newItem.CreatedDate = DateTime.UtcNow;
                newItem.UpdatedDate = DateTime.UtcNow;
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
                    theItem.ItemOrder = item.ItemOrder;
                    theItem.Score = item.Score;
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

        #region result
        public async Task<IList<tbSurveyResult>> GetSurveyResultsBySurveyId(int surveyId)
        {
            IList<tbSurveyResult> resultList = new List<tbSurveyResult>();

            try
            {
                resultList = await _db.tbSurveyResults.Where(x => x.SurveyId == surveyId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false)).ToListAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return resultList;
        }

        public async Task<tbSurveyResult> GetSurveyResultByResultId(int resultId)
        {
            tbSurveyResult result = new tbSurveyResult();

            try
            {
                result = await _db.tbSurveyResults.FirstOrDefaultAsync(x => x.SurveyResultId == resultId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false));
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return result;
        }

        public async Task<tbSurveyResult> CreateNewSurveyResult(dtoSurveyResult result)
        {
            tbSurveyResult newResult = new tbSurveyResult();

            try
            {
                newResult.MinScore = result.MinScore;
                newResult.MaxScore = result.MaxScore;
                newResult.SurveyId = result.SurveyId;
                newResult.Title = result.Title;
                newResult.Description = result.Description;
                newResult.CreatedDate = DateTime.UtcNow;
                newResult.UpdatedDate = DateTime.UtcNow;
                newResult.IsActive = true;
                newResult.IsDeleted = false;

                _db.tbSurveyResults.Add(newResult);
                await _db.SaveChangesAsync();
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return newResult;
        }

        public async Task<tbSurveyResult> UpdateSurveyResult(dtoSurveyResult result)
        {
            tbSurveyResult theResult = null;

            try
            {
                theResult = await _db.tbSurveyResults.FirstOrDefaultAsync(x => x.SurveyResultId == result.SurveyResultId
                      && (x.IsActive ?? true) && !(x.IsDeleted ?? false));

                if (theResult != null)
                {
                    theResult.MinScore = result.MinScore;
                    theResult.MaxScore = result.MaxScore;
                    theResult.SurveyId = result.SurveyId;
                    theResult.Title = result.Title;
                    theResult.Description = result.Description;
                    theResult.UpdatedDate = DateTime.UtcNow;

                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }

            return theResult;
        }

        public async Task DeleteSurveyResult(int resultId)
        {
            try
            {
                tbSurveyResult theResult = await _db.tbSurveyResults.FirstOrDefaultAsync(x => x.SurveyResultId == resultId
                      && (x.IsActive ?? true) && !(x.IsDeleted ?? false));

                if (theResult != null)
                {
                    theResult.IsDeleted = true;
                    theResult.IsActive = false;
                    theResult.UpdatedDate = DateTime.UtcNow;

                    await _db.SaveChangesAsync();
                }
            }
            catch (DataException dex)
            {
                throw new ApplicationException("Data error!", dex);
            }
        }

        public async Task DeactiveSurveyResult(int resultId)
        {
            try
            {
                tbSurveyResult theResult = await _db.tbSurveyResults.FirstOrDefaultAsync(x => x.SurveyResultId == resultId
                    && (x.IsActive ?? true) && !(x.IsDeleted ?? false));

                if (theResult != null)
                {
                    theResult.IsActive = false;
                    theResult.UpdatedDate = DateTime.UtcNow;
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
