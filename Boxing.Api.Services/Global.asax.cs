using Autofac;
using Autofac.Integration.WebApi;
using Boxing.Api.Services.Filters;
using Boxing.Core.Services.Interfaces;
using Boxing.Core.Services.Services;
using Boxing.Core.Sql;
using FluentValidation.WebApi;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;


namespace Boxing.Api.Services
{
    public static class AutofacContainer
    {
        public static IContainer container;
    }
    
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;

            config.MapHttpAttributeRoutes();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.Filters.Add(new BadRequestExceptionAttribute());
            config.Filters.Add(new NotFoundExceptionAttribute());
            config.Filters.Add(new IncorrectCredentialsAttribute());
            config.Filters.Add(new ValidationFilterAttribute());
            config.Filters.Add(new ForbiddenExceptionAttribute());

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            //FilterConfig.RegisterFilters(config);

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            RegisterServices(builder);
            //RegisterContext(builder);

            var container = builder.Build();
            AutofacContainer.container = container;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            FluentValidationModelValidatorProvider.Configure(config);

            config.EnsureInitialized();

            BoxingContext.SetInitializer();
            //ConfigureMappings();

            //GlobalConfiguration.Configure(WebApiConfig.Register);
        }
        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<UsersService>()
                .As<IUsersService>()
                .InstancePerRequest();
            
            builder.RegisterType<LoginsService>()
                .As<ILoginsService>()
                .InstancePerRequest();

            builder.RegisterType<MatchesService>()
                .As<IMatchesService>()
                .InstancePerRequest();

            builder.RegisterType<BoxingContext>()
                .SingleInstance()
                .AsSelf()
                .As<DbContext>();

        }
    }
}
