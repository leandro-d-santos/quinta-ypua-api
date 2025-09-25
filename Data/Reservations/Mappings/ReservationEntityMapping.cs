using Data.Common;
using Data.Reservations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Globalization;

namespace Data.Reservations.Mappings
{
    public sealed class ReservationEntityMapping : IEntityTypeConfiguration<ReservationEntity>
    {
        public void Configure(EntityTypeBuilder<ReservationEntity> builder)
        {
            builder.ToTable("reservations");
            builder.Property(p => p.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(p => p.CheckIn).HasColumnName("checkIn").HasColumnType("date").IsRequired().HasConversion(v => v.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), v => DateTime.Parse(v, CultureInfo.InvariantCulture));
            builder.Property(p => p.CheckOut).HasColumnName("checkOut").HasColumnType("date").IsRequired().HasConversion(v => v.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), v => DateTime.Parse(v, CultureInfo.InvariantCulture));
            builder.Property(p => p.NumberOfAdults).HasColumnName("numberOfAdults").IsRequired();
            builder.Property(p => p.NumberOfChildren).HasColumnName("numberOfChildren").IsRequired();
            builder.Property(p => p.RoomId).HasColumnName("roomId").HasMaxLength(36).IsRequired();
            builder.Property(p => p.GuestId).HasColumnName("guestId").HasMaxLength(36).IsRequired();
            MappingHelper.ConfigureOperationColumns(builder);
            builder.HasKey(p => p.Id);
            builder.HasOne(e => e.Room)
                .WithOne(e => e.Reservation)
                .IsRequired();
            builder.HasOne(e => e.Guest)
                .WithOne(e => e.Reservation)
                .IsRequired();
        }
    }
}