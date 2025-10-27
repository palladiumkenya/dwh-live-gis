using CSharpFunctionalExtensions;
using LiveDWAPI.Application.Immunization.Dto;
using LiveDWAPI.Domain.Immunization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LiveDWAPI.Application.Immunization.Queries;

public class GetAggregateFilteredQuery:IRequest<Result<List<FactAggregateVaccination>>>
{
    public FilterDto Filter { get;  }

    public GetAggregateFilteredQuery(FilterDto filter)
    {
        Filter = filter;
    }
}

public class GetAggregateFilteredQueryHandler:IRequestHandler<GetAggregateFilteredQuery,Result<List<FactAggregateVaccination>>>
{
    private readonly IImmunizationContext _context;

    public GetAggregateFilteredQueryHandler(IImmunizationContext context)
    {
        _context = context;
    }

    public async Task<Result<List<FactAggregateVaccination>>> Handle(GetAggregateFilteredQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IQueryable<FactAggregateVaccination> query = _context.FactAggregateVaccinations
                .Where(x=>x.FacilityName!=null);

            // Indicator
            if (request.Filter.HasVaccine())
                query = query.Where(x => 
                    x.Vaccine != null &&
                    x.Vaccine.ToLower()==request.Filter.Vaccine!.ToLower());

            // Place
           
            if (request.Filter.HasFacilityName())
                query = query.Where(x =>request.Filter.FacilityName!.Contains(x.FacilityName));
            
            // Person
            if (request.Filter.HasSex())
                query = query.Where(x =>request.Filter.Sex!.Contains(x.Sex));
            
            if (request.Filter.HasAgeGroup())
                query = query.Where(x =>request.Filter.AgeGroup!.Contains(x.AgeGroup));

            if (request.Filter.Limit > 0)
                query = query.Take(request.Filter.Limit);
            
            var indicators = await query.ToListAsync(cancellationToken);
            
            var sorted = indicators
                .OrderByDescending(x => x.Numerator)
                .ThenBy(x=>x.FacilityName)
                .ToList();
            return Result.Success(sorted);
        }
        catch (Exception e)
        {
            Log.Error(e,"Load Aggregate Indicators error!");
            return Result.Failure<List<FactAggregateVaccination>>(e.Message);
        }
    }
}