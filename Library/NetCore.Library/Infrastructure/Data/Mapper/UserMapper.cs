using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCore.Library.Identity;

namespace NetCore.Library.Infrastructure.Data.Mapper
{
    public class UserMapper : IEntityTypeConfiguration<UserInfo>
    {
        private string Schema;

        public UserMapper(string schema)
        {
            Schema = schema;
        }

        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("CORE_USER", Schema);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.CreationDate).HasColumnName("creation_date");
            builder.Property(x => x.FullName).HasColumnName("full_name");
            builder.Property(x => x.UserName).HasColumnName("user_name");
        }
    }
}