using CSharpFunctionalExtensions;
using LiveDWAPI.Application.Immunization.Dto;
using LiveDWAPI.Domain.Immunization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LiveDWAPI.Application.Immunization.Queries;

public class GetAggregateFilteredExportQuery:IRequest<Result<IAsyncEnumerable<FactAggregateVaccination>>>
{
    public FilterDto Filter { get;  }

    public GetAggregateFilteredExportQuery(FilterDto filter)
    {
        Filter = filter;
    }
}

public class GetAggregateFilteredExportQueryHandler:IRequestHandler<GetAggregateFilteredExportQuery,Result<IAsyncEnumerable<FactAggregateVaccination>>>
{
    private readonly IImmunizationContext _context;

    public GetAggregateFilteredExportQueryHandler(IImmunizationContext context)
    {
        _context = context;
    }

    public async Task<Result<IAsyncEnumerable<FactAggregateVaccination>>> Handle(GetAggregateFilteredExportQuery request, CancellationToken cancellationToken)
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
            
            var indicators =  query.AsAsyncEnumerable();
           
            return Result.Success(indicators);
        }
        catch (Exception e)
        {
            Log.Error(e,"Load Aggregate Indicators error!");
            return Result.Failure<IAsyncEnumerable<FactAggregateVaccination>>(e.Message);
        }
    }
}