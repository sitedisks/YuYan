using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YuYan.Domain.Database;
using YuYan.Interface.DbContext;

namespace YuYan.Data.DbContext
{
    public class YuYanDBContext : System.Data.Entity.DbContext, IYuYanDBContext
    {
        static YuYanDBContext()
        {
            Database.SetInitializer<YuYanDBContext>(null);
        }

        public static YuYanDBContext Create()
        {
            var db = new YuYanDBContext();
            return db;
        }

        public YuYanDBContext() : base("YuYanDBAzureContext") { }

        public YuYanDBContext(string connectionString) : base(connectionString) { }


        #region entities
        public DbSet<tbUser> tbUsers { get; set; }
        public DbSet<tbSession> tbSessions { get; set; }
        public DbSet<tbSurvey> tbSurveys { get; set; }
        public DbSet<tbSurveyQuestion> tbSurveyQuestions { get; set; }
        public DbSet<tbSurveyQuestionItem> tbSurveyQuestionItems { get; set; }
        public DbSet<tbSurveyClient> tbSurveyClients { get; set; }
        public DbSet<tbSurveyClientAnswer> tbSurveyClientAnswers { get; set; }
        public DbSet<tbSurveyShare> tbSurveyShares { get; set; }
        public DbSet<tbSurveyResult> tbSurveyResults { get; set; }
        #endregion

        public System.Data.Entity.Core.Objects.ObjectContext BaseContext
        {
            get { return ((IObjectContextAdapter)this).ObjectContext; }
        }

    }
}
