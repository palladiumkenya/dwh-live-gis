using LiveDWAPI.Domain.Immunization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDWAPI.Infrastructure.Immunization.Configurations;

public class FactAggregateVaccinationConfiguration:IEntityTypeConfiguration<FactAggregateVaccination>
{
    public void Configure(EntityTypeBuilder<FactAggregateVaccination> builder)
    {
        builder.ToTable(
            "aggregate_combined_indicators",
            "universal_semantic_layer", 
            t=>t.ExcludeFromMigrations()).HasNoKey();
        builder.Property(e => e.Sex).HasColumnName("gender");
        builder.Property(e => e.AgeGroup).HasColumnName("age_group_category");
        builder.Property(e => e.Vaccine).HasColumnName("vaccine_name");
        builder.Property(e => e.AssessmentPeriod).HasColumnName("period");
        builder.Property(e => e.Numerator).HasColumnName("metric");
        builder.Property<string?>("indicator");
    }
}