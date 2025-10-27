using LiveDWAPI.Domain.Immunization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDWAPI.Infrastructure.Immunization.Configurations;

public class DimRegionConfiguration : IEntityTypeConfiguration<DimRegion>
{
    public void Configure(EntityTypeBuilder<DimRegion> builder)
    {
        builder.ToView("v_region_facility", "universal_semantic_layer").HasNoKey();
    }
}