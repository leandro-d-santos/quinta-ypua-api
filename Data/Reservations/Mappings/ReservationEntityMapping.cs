using Data.Common;
using Data.Reservations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Reservations.Mappings
{
    public sealed class ReservationEntityMapping : IEntityTypeConfiguration<ReservationEntity>
    {
        public void Configure(EntityTypeBuilder<ReservationEntity> builder)
        {
            builder.ToTable("reservations");
            builder.Property(p => p.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(p => p.CheckIn).HasColumnName("checkIn").IsRequired();
            builder.Property(p => p.CheckOut).HasColumnName("checkOut").IsRequired();
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