using LiveDWAPI.Domain.Immunization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDWAPI.Infrastructure.Immunization.Configurations;

public class DimAgeGroupConfiguration : IEntityTypeConfiguration<DimAgeGroup>
{
    public void Configure(EntityTypeBuilder<DimAgeGroup> builder)
    {
        builder.ToView("v_age_group", "universal_semantic_layer").HasNoKey();
    }
}