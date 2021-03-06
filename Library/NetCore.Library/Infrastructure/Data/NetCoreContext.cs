﻿using Microsoft.EntityFrameworkCore;
using NetCore.Library.Identity;
using NetCore.Library.Infrastructure.Data.Mapper;

namespace NetCore.Library.Infrastructure.Data
{
    public class NetCoreContext : DbContext
    {
        public string Schema { get; set; }

        public DbSet<UserInfo> Users { get; set; }

        public NetCoreContext(DbContextOptions<NetCoreContext> options, string schema = null)
            : base(options)
        {
            Schema = !string.IsNullOrEmpty(schema) ? schema : "dbo";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfiguration(new UserMapper(Schema));
        }
    }
}