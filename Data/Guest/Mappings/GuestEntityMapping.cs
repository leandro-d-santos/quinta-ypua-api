using Data.Common;
using Data.Guests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Guests.Mappings
{
    public sealed class GuestEntityMapping : IEntityTypeConfiguration<GuestEntity>
    {
        public void Configure(EntityTypeBuilder<GuestEntity> builder)
        {
            builder.ToTable("guests");
            builder.Property(p => p.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(p => p.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
            builder.Property(p => p.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
            builder.Property(p => p.CPF).HasColumnName("cpf").HasMaxLength(11).IsRequired();
            builder.Property(p => p.PhoneNumber).HasColumnName("phoneNumber").HasMaxLength(22).IsRequired();
            MappingHelper.ConfigureOperationColumns(builder);
            builder.HasKey(p => p.Id);
        }
    }
}