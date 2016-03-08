using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using YuYan.Domain.Database;

namespace YuYan.Interface.DbContext
{
    public interface IYuYanDBContext : IDisposable
    {
        Database Database { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbEntityEntry Entry(object entity);
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        ObjectContext BaseContext { get; }

        #region entities
        DbSet<tbUser> tbUsers { get; set; }
        DbSet<tbSession> tbSessions { get; set; }
        DbSet<tbSurvey> tbSurveys { get; set; }
        DbSet<tbSurveyQuestion> tbSurveyQuestions { get; set; }
        DbSet<tbSurveyQuestionItem> tbSurveyQuestionItems { get; set; }
        DbSet<tbSurveyClient> tbSurveyClients { get; set; }
        DbSet<tbSurveyClientAnswer> tbSurveyClientAnswers { get; set; }
        DbSet<tbSurveyShare> tbSurveyShares { get; set; }
        DbSet<tbSurveyResult> tbSurveyResults { get; set; }
        #endregion
    }
}
