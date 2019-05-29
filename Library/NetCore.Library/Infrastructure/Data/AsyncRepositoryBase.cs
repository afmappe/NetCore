using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCore.Library.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Library.Infrastructure.Data
{
    public abstract class AsyncRepositoryBase<TEntityType, TContextType> :
         IAsyncRepository<TEntityType>
         where TEntityType : class
         where TContextType : DbContext
    {
        protected TContextType CreateContext()
        {
            TContextType context = default(TContextType);

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

            context = (TContextType)Activator.CreateInstance(typeof(TContextType), contextOptionsBuilder.Options);
            return context;
        }

        public async Task Create(TEntityType record)
        {
            using (TContextType context = CreateContext())
            {
                context.Entry(record).State = EntityState.Added;
                await context.SaveChangesAsync();
            }
        }

        public async Task Create(IEnumerable<TEntityType> list)
        {
            if (list != null && list.Any())
            {
                using (TContextType context = CreateContext())
                {
                    context.ChangeTracker.AutoDetectChangesEnabled = false;
                    context.Set<TEntityType>().AddRange(list);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task Delete(TEntityType record)
        {
            using (TContextType context = CreateContext())
            {
                context.Entry(record).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }

        public async Task Delete(IEnumerable<TEntityType> list)
        {
            if (list != null && list.Any())
            {
                using (TContextType context = CreateContext())
                {
                    context.ChangeTracker.AutoDetectChangesEnabled = false;

                    foreach (TEntityType record in list)
                    {
                        context.Entry(record).State = EntityState.Deleted;
                    }
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<TEntityType> Find(params object[] keys)
        {
            TEntityType record = default(TEntityType);

            try
            {
                using (TContextType context = CreateContext())
                {
                    record = await context.Set<TEntityType>().FindAsync(keys);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return record;
        }

        public async Task Update(TEntityType record)
        {
            try
            {
                using (TContextType context = CreateContext())
                {
                    context.Entry(record).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task Update(List<TEntityType> list)
        {
            try
            {
                if (list != null && list.Any())
                {
                    using (TContextType context = CreateContext())
                    {
                        context.ChangeTracker.AutoDetectChangesEnabled = false;

                        foreach (TEntityType record in list)
                        {
                            context.Entry(record).State = EntityState.Modified;
                        }

                        context.ChangeTracker.DetectChanges();
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}