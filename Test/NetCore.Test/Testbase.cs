using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCore.Library.Identity.Commands;
using NetCore.Library.Identity.Repositories;
using NetCore.Library.Infrastructure.Data;
using NetCore.Library.Infrastructure.Data.Repositories;
using NetCore.Library.Services;
using NetCore.Test.Helpers;
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
            var temp = TestHelper.GetApplicationConfiguration();
            var services = new ServiceCollection();

            services.AddScoped(typeof(IEncryptionService), typeof(EncryptionService));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddMediatR(typeof(CreateUserCommand));
            Provider = services.BuildServiceProvider();
        }

        private static void CreateContext()
        {
            DbContextOptionsBuilder<NetCoreContext> contextOptionsBuilder = new DbContextOptionsBuilder<NetCoreContext>();

            DbConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = "localhost",
                IntegratedSecurity = true,
                InitialCatalog = "NETCoreTest",
            };
            SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString);

            LoggerFactory loggerFactory = new LoggerFactory();

            contextOptionsBuilder.UseLoggerFactory(loggerFactory);
            contextOptionsBuilder.UseSqlServer(connection);
            contextOptionsBuilder.EnableSensitiveDataLogging();

            NetCoreContext context = new NetCoreContext(contextOptionsBuilder.Options);
        }
    }
}