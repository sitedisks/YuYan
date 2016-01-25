using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using YuYan.Data.DbContext;
using YuYan.Interface.DbContext;
using YuYan.Data.Repository;
using YuYan.Service;

namespace YuYan.API
{
    public class Bootstrapper
    {
        private static IContainer _container;

        public static void Run() { SetAutoFacContainer(); }

        private static void SetAutoFacContainer()
        {
            var container = GetContainer();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        public static IContainer GetContainer()
        {

            if (_container != null) return _container;

            var containerBuilder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            // Autofac
            containerBuilder.RegisterModule<AutofacWebTypesModule>();
            containerBuilder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            containerBuilder.RegisterWebApiFilterProvider(config);

            // register db context
            containerBuilder.RegisterType<YuYanDBContext>()
                .As<IYuYanDBContext>()
                .WithParameter("connectionString", "YuYanDbAzureContext")
                .InstancePerLifetimeScope();
            // register repository
            containerBuilder.RegisterType<YuYanDBRepository>().AsImplementedInterfaces();
            // register services
            containerBuilder.RegisterType<YuYanService>().AsImplementedInterfaces().InstancePerDependency();

            _container = containerBuilder.Build();
            return _container;
        }
    }
}