namespace YuYan.Data.DbContext
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using YuYan.Domain.Database;
    using YuYan.Interface.DbContext;

    public class YuYanDBContext : DbContext, IYuYanDBContext
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

        public YuYanDBContext() : base("YuYanDbAzureContext") { }

        public YuYanDBContext(string connectionString) : base(connectionString) {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }


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
        public DbSet<ip2location_db3> ip2locations { get; set; }
        public DbSet<tbImage> tbImages { get; set; }
        public DbSet<tbImageType> tbImageTypes { get; set; }
        #endregion

        public System.Data.Entity.Core.Objects.ObjectContext BaseContext
        {
            get { return ((IObjectContextAdapter)this).ObjectContext; }
        }

    }
}
