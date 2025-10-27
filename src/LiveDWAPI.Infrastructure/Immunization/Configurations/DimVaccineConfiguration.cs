using LiveDWAPI.Domain.Immunization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDWAPI.Infrastructure.Immunization.Configurations;

public class DimVaccineConfiguration : IEntityTypeConfiguration<DimVaccine>
{
    public void Configure(EntityTypeBuilder<DimVaccine> builder)
    {
        builder.ToView("v_vaccine", "universal_semantic_layer").HasNoKey();
    }
}