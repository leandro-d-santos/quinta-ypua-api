using Data.Common;
using Data.Financial.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Financial.Mappings
{
    public sealed class FinancialEntityMapping : IEntityTypeConfiguration<FinancialEntity>
    {
        public void Configure(EntityTypeBuilder<FinancialEntity> builder)
        {
            builder.ToTable("financial");
            builder.Property(p => p.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(p => p.ReservationValue).HasColumnName("reservationValue").IsRequired();
            builder.Property(p => p.AdditionalValue).HasColumnName("additionalValue").IsRequired();
            builder.Property(p => p.Payment).HasColumnName("payment").IsRequired();
            builder.Property(p => p.Status).HasColumnName("status").IsRequired();
            MappingHelper.ConfigureOperationColumns(builder);
            builder.HasKey(p => p.Id);
        }
    }
}