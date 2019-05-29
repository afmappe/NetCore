using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCore.Library.Identity.Commands;
using NetCore.Library.Identity.Repositories;
using NetCore.Library.Infrastructure.Data;
using NetCore.Library.Infrastructure.Data.Repositories;
using System.Data.Common;
using System.Data.SqlClient;

namespace NetCore.Test
{
    [TestClass]
    public class Testbase
    {
        protected ServiceProvider Provider;

        protected IMediator Mediator
        {
            get { return Provider.GetRequiredService<IMediator>(); }
        }

        [TestInitialize]
        public void Initialize()
        {
            var services = new ServiceCollection();

            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddMediatR(typeof(CreateUserCommand));
            Provider = services.BuildServiceProvider();
        }

        private static void CreateContext()
        {
            DbContextOptionsBuilder<NetCoreContext> contextOptionsBuilder = new DbContextOptionsBuilder<NetCoreContext>();

            LoggerFactory loggerFactory = new LoggerFactory(); // this will allow us to add loggers so we can actually inspect what code and queries EntityFramework produces.

            DbConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = "localhost",
                IntegratedSecurity = true,
                InitialCatalog = "NETCoreTest",
            };
            SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            contextOptionsBuilder.UseLoggerFactory(loggerFactory); // register the loggers inside the context options builder, this way, entity framework logs the queries
            contextOptionsBuilder.UseSqlServer(connection); // we're telling entity framework to use the SQLite connection we created.
            contextOptionsBuilder.EnableSensitiveDataLogging(); // this will give us more insight when something does go wrong. It's ok to use it here since it's a testing project, but be careful about enabling this in production.

            NetCoreContext context = new NetCoreContext(contextOptionsBuilder.Options);
        }
    }
}