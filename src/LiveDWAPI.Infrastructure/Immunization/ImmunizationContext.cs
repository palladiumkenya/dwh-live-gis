using System.Reflection;
using LiveDWAPI.Application.Immunization;
using LiveDWAPI.Domain.Immunization;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LiveDWAPI.Infrastructure.Immunization;

public class ImmunizationContext : DbContext, IImmunizationContext
{
    public DbSet<DimVaccine> DimVaccines => Set<DimVaccine>();
    public DbSet<DimRegion> DimRegions => Set<DimRegion>();
    public DbSet<DimAgeGroup> DimAgeGroups => Set<DimAgeGroup>();
    public DbSet<DimSex> DimSex => Set<DimSex>();
    public DbSet<FactAggregateVaccination> FactAggregateVaccinations => Set<FactAggregateVaccination>();


    public ImmunizationContext(DbContextOptions<ImmunizationContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public void Initialize()
    {
        try
        {
            Database.MigrateAsync().Wait();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while initialising the database.");
            throw;
        }
    }
    
    public Task Commit(CancellationToken cancellationToken)
    {
        return SaveChangesAsync(cancellationToken);
    }
}