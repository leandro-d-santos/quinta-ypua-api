using Data.Common;
using Data.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Users.Mappings
{
    public sealed class UserEntityMapping : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("users");
            builder.Property(p => p.UserName).HasColumnName("userName").HasMaxLength(50).IsRequired();
            builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
            builder.Property(p => p.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
            builder.Property(p => p.Password).HasColumnName("password").HasMaxLength(36).IsRequired();
            MappingHelper.ConfigureOperationColumns(builder);
            builder.HasKey(p => p.UserName);
        }
    }
}