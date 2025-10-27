using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LiveDWAPI.Application.Immunization.Queries;

public enum FilterType
{
    Indicator,Region,Age,Sex
}
public class GetAggregateFiltersQuery:IRequest<Result<object>>
{
    public FilterType Type { get; }

    public GetAggregateFiltersQuery(FilterType type)
    {
        Type = type;
    }
}

public class GetAggregateFiltersQueryHandler:IRequestHandler<GetAggregateFiltersQuery,Result<object>>
{
    private readonly IImmunizationContext _context;

    public GetAggregateFiltersQueryHandler(IImmunizationContext context)
    {
        _context = context;
    }

    public async Task<Result<object>> Handle(GetAggregateFiltersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            object? data = null;

            if (request.Type == FilterType.Indicator)
            {
               data = await _context.DimVaccines
                    .Where(x => x.VaccineName != null)
                    .OrderBy(x => x.VaccineName)
                    .ToListAsync(cancellationToken);

            }

            if (request.Type == FilterType.Region)
            {
                data = await _context.DimRegions
                    .Where(x => x.FacilityName != null)
                    .OrderBy(x => x.FacilityName)
                    .ToListAsync(cancellationToken);
              
            }
            
            if (request.Type == FilterType.Age)
            {
                data = await _context.DimAgeGroups
                    .Where(x => x.AgeGroup != null)
                    .ToListAsync(cancellationToken);
              
            }
            
            if (request.Type == FilterType.Sex)
            {
                data = await _context.DimSex
                    .Where(x => x.Sex != null)
                    .ToListAsync(cancellationToken);
              
            }
            
            return Result.Success(data);
        }
        catch (Exception e)
        {
            Log.Error(e,"Load Filters error!");
            return Result.Failure<Dictionary<Enum, dynamic>>(e.Message);
        }
    }
}