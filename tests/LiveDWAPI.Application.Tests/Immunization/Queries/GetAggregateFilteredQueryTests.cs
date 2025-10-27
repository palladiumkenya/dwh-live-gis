using LiveDWAPI.Application.Immunization.Dto;
using LiveDWAPI.Application.Immunization.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LiveDWAPI.Application.Tests.Immunization.Queries;

[TestFixture]
public class GetAggregateFilteredQueryTests
{
    private IMediator? _mediator;
    
    [SetUp]
    public void SetUp()
    {
        _mediator = TestInitializer.ServiceProvider.GetRequiredService<IMediator>();
    }

    [TestCase("BCG","Lumumba Sub County Hospital")]
    public async Task should_Read(string vaccine,string facility)
    {
        var filter = new FilterDto()
        {
            Vaccine = vaccine
        };
        var res =await _mediator!.Send(new GetAggregateFilteredQuery(filter));
        Assert.That(res.IsSuccess,Is.True);
        Assert.That(res.Value.Any(),Is.True);
    }
    
    [TestCase("BCG",1)]
    public async Task should_Read_Limit(string vaccine,int limit)
    {
        var filter = new FilterDto()
        {
            Vaccine = vaccine,Limit = limit
        };
        var res =await _mediator!.Send(new GetAggregateFilteredQuery(filter));
        Assert.That(res.IsSuccess,Is.True);
        Assert.That(res.Value.Count,Is.EqualTo(limit));
    }
}