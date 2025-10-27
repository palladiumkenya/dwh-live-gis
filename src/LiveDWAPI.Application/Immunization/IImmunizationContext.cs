using LiveDWAPI.Domain.Immunization;
using Microsoft.EntityFrameworkCore;

namespace LiveDWAPI.Application.Immunization;

public interface IImmunizationContext
{
    DbSet<DimVaccine> DimVaccines { get; }
    DbSet<DimRegion> DimRegions{ get; }
    DbSet<DimAgeGroup> DimAgeGroups { get; }
    DbSet<DimSex> DimSex { get; }
    DbSet<FactAggregateVaccination> FactAggregateVaccinations { get; }
    Task Commit(CancellationToken cancellationToken);
}