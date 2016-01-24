﻿using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using YuYan.Domain.Database;
using YuYan.Interface.Repository;
using YuYan.Interface.DbContext;
using System.Collections.Generic;

namespace YuYan.Data.Repository
{
    public class YuYanDBRepository : IYuYunDBRepository
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
