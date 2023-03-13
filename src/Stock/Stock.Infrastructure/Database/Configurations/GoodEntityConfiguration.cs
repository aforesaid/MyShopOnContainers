using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Database.Configurations;

public class GoodEntityConfiguration : IEntityTypeConfiguration<GoodEntity>
{
    public void Configure(EntityTypeBuilder<GoodEntity> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
    }
}