using Data.Common;
using Data.Rooms.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Rooms.Mappings
{
    public sealed class RoomEntityMapping : IEntityTypeConfiguration<RoomEntity>
    {
        public void Configure(EntityTypeBuilder<RoomEntity> builder)
        {
            builder.ToTable("rooms");
            builder.Property(p => p.Id).HasColumnName("id").HasMaxLength(36).IsRequired();
            builder.Property(p => p.Number).HasColumnName("number").HasMaxLength(5).IsRequired();
            builder.Property(p => p.Floor).HasColumnName("floor").HasMaxLength(5).IsRequired();
            builder.Property(p => p.Description).HasColumnName("description").HasMaxLength(300).IsRequired();
            builder.Property(p => p.Capacity).HasColumnName("capacity").IsRequired();
            MappingHelper.ConfigureOperationColumns(builder);
            builder.HasKey(p => p.Id);
            builder.HasOne(e => e.Reservation)
                .WithOne(e => e.Room);
        }
    }
}