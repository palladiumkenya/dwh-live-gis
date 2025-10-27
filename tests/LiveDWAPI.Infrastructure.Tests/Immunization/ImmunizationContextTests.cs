using LiveDWAPI.Infrastructure.Immunization;
using Microsoft.Extensions.DependencyInjection;

namespace LiveDWAPI.Infrastructure.Tests.Immunization;

[TestFixture]
public class ImmunizationContextTests
{
    private ImmunizationContext _context;
    
    [SetUp]
    public void SetUp()
    {
        _context = TestInitializer.ServiceProvider.GetRequiredService<ImmunizationContext>();
        _context.Initialize();
    }
    
    [Test,Order(1)]
    public void should_Map_Table()
    {
        Assert.That(_context.FactAggregateVaccinations.FirstOrDefault(),Is.Not.Null);
    }
    [Test,Order(2)]
    public void should_Map_Views()
    {
        Assert.That(_context.DimVaccines.FirstOrDefault(),Is.Not.Null);
        Assert.That(_context.DimRegions.FirstOrDefault(),Is.Not.Null);
        Assert.That(_context.DimAgeGroups.FirstOrDefault(),Is.Not.Null);
        Assert.That(_context.DimSex.FirstOrDefault(),Is.Not.Null);
    }
}