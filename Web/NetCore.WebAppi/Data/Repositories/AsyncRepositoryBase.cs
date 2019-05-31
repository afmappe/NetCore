using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.WebAppi.Data.Repositories
{
    public abstract class AsyncRepositoryBase<TEntityType, TContextType> :
         IAsyncRepository<TEntityType>
         where TEntityType : class
         where TContextType : DbContext
    {
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

        protected TContextType CreateContext()
        {
            TContextType context = default(TContextType);

            DbContextOptionsBuilder<ApplicationDbContext> contextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            LoggerFactory loggerFactory = new LoggerFactory();

            DbConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = "localhost",
                IntegratedSecurity = true,
                InitialCatalog = "NETCoreTest",
            };
            SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString);

            contextOptionsBuilder.UseLoggerFactory(loggerFactory);
            contextOptionsBuilder.UseSqlServer(connection);
            contextOptionsBuilder.EnableSensitiveDataLogging();

            context = (TContextType)Activator.CreateInstance(typeof(TContextType), contextOptionsBuilder.Options, "dbo");
            return context;
        }
    }
}