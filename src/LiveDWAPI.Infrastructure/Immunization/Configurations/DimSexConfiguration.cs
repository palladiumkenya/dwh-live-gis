using LiveDWAPI.Domain.Immunization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDWAPI.Infrastructure.Immunization.Configurations;

public class DimSexConfiguration : IEntityTypeConfiguration<DimSex>
{
    public void Configure(EntityTypeBuilder<DimSex> builder)
    {
        builder.ToView("v_sex", "universal_semantic_layer").HasNoKey();
    }
}