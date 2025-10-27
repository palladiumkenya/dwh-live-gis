using AutoMapper;
using LiveDWAPI.Application.Immunization.Queries;
using LiveDWAPI.Domain.Immunization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiveDWAPI.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilterController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public FilterController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("Indicator")]
    [ProducesResponseType(typeof(List<DimVaccine>), 200)]
    public async Task<IActionResult> GetIndicator()
    {
        try
        {
            var res = await _mediator.Send(new GetAggregateFiltersQuery(FilterType.Indicator));

            if (res.IsSuccess)
                return Ok(_mapper.Map<List<DimVaccine>>(res.Value));

            throw new Exception($"An error occured: {res.Error}");
        }
        catch (Exception e)
        {
            Log.Error(e, "Error loading");
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("Region")]
    [ProducesResponseType(typeof(List<DimRegion>), 200)]
    public async Task<IActionResult> GetRegions()
    {
        try
        {
            var res = await _mediator.Send(new GetAggregateFiltersQuery(FilterType.Region));

            if (res.IsSuccess)
                return Ok(_mapper.Map<List<DimRegion>>(res.Value));

            throw new Exception($"An error occured: {res.Error}");
        }
        catch (Exception e)
        {
            Log.Error(e, "Error loading");
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("Sex")]
    [ProducesResponseType(typeof(List<DimSex>), 200)]
    public async Task<IActionResult> GetSex()
    {
        try
        {
            var res = await _mediator.Send(new GetAggregateFiltersQuery(FilterType.Sex));

            if (res.IsSuccess)
                return Ok(_mapper.Map<List<DimSex>>(res.Value));

            throw new Exception($"An error occured: {res.Error}");
        }
        catch (Exception e)
        {
            Log.Error(e, "Error loading");
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("Age")]
    [ProducesResponseType(typeof(List<DimAgeGroup>), 200)]
    public async Task<IActionResult> GetAge()
    {
        try
        {
            var res = await _mediator.Send(new GetAggregateFiltersQuery(FilterType.Age));

            if (res.IsSuccess)
                return Ok(_mapper.Map<List<DimAgeGroup>>(res.Value));

            throw new Exception($"An error occured: {res.Error}");
        }
        catch (Exception e)
        {
            Log.Error(e, "Error loading");
            return StatusCode(500, e.Message);
        }
    }
    
}