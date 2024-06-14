using Data.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Common
{
    public static class MappingHelper
    {
        public static void ConfigureOperationColumns<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : Entity
        {
            builder.Property(p => p.CreatedAt).HasColumnName("createdAt");
            builder.Property(p => p.UpdatedAt).HasColumnName("updatedAt");
            builder.Property(p => p.DeletedAt).HasColumnName("deletedAt");
        }
    }
}