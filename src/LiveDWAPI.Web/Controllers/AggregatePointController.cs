using System.Globalization;
using CsvHelper;
using LiveDWAPI.Application.Immunization.Dto;
using LiveDWAPI.Application.Immunization.Queries;
using LiveDWAPI.Domain.Immunization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiveDWAPI.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AggregatePointController : ControllerBase
{
    private readonly IMediator _mediator;
    public AggregatePointController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("All")]
    [ProducesResponseType(typeof(VaccineDataDto), 200)]
    public async Task<IActionResult> GetDataPointsByQuery([FromQuery] FilterDto filter)
    {
        try
        {
            var res = await _mediator.Send(new GetAggregateFilteredQuery(filter));

            if (res.IsSuccess)
            {
                var points = new VaccineDataDto(res.Value);
                return Ok(points);
            }

            throw new Exception($"Error occured ${res.Error}");
        }
        catch (Exception e)
        {
            Log.Error(e, "Error loading");
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost("All")]
    [ProducesResponseType(typeof(VaccineDataDto), 200)]
    public async Task<IActionResult> GetDataPointsByBody([FromBody] FilterDto filter)
    {
        try
        {
            var res = await _mediator.Send(new GetAggregateFilteredQuery(filter));

            if (res.IsSuccess)
            {
                var points = new VaccineDataDto(res.Value);
                return Ok(points);
            }

            throw new Exception($"Error occured ${res.Error}");
        }
        catch (Exception e)
        {
            Log.Error(e, "Error loading");
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("Data")]
    [ProducesResponseType(typeof(VaccineDataDto), 200)]
    public async Task<IActionResult> GetDataByQuery([FromQuery] FilterDto filter)
    {
        try
        {
            filter.Limit = 50;
            var res = await _mediator.Send(new GetAggregateFilteredQuery(filter));

            if (res.IsSuccess)
            {
                var data = res.Value;
                return Ok(data);
            }

            throw new Exception($"Error occured ${res.Error}");
        }
        catch (Exception e)
        {
            Log.Error(e, "Error loading");
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost("Data")]
    [ProducesResponseType(typeof(VaccineDataDto), 200)]
    public async Task<IActionResult> GetDataByBody([FromBody] FilterDto filter)
    {
        try
        {
            filter.Limit = 50;
            var res = await _mediator.Send(new GetAggregateFilteredQuery(filter));

            if (res.IsSuccess)
            {
                var data = res.Value;
                return Ok(data);
            }

            throw new Exception($"Error occured ${res.Error}");
        }
        catch (Exception e)
        {
            Log.Error(e, "Error loading");
            return StatusCode(500, e.Message);
        }
    }
    
       [HttpGet("Data/Export")]
    public async Task<IActionResult> ExportDataByQuery([FromQuery] FilterDto filter)
    {
        // Set the content type and filename for the browser
        Response.ContentType = "text/csv";
        Response.Headers.Add("Content-Disposition", "attachment; filename=cs_export.csv");

        // Use a StreamWriter on the response body to avoid creating a MemoryStream
        await using var streamWriter = new StreamWriter(Response.Body, leaveOpen: true);
        await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

        // EF Core 6+ supports IAsyncEnumerable, which is crucial for streaming
        var res = await _mediator.Send(new GetAggregateFilteredExportQuery(filter));

        var data = res.Value;

        // Write the CSV header
        csvWriter.WriteHeader<FactAggregateVaccination>();
        await csvWriter.NextRecordAsync();

        // Write records one at a time as they are retrieved from the database
        await csvWriter.WriteRecordsAsync(data);

        // Explicitly flush to ensure all data is sent
        await streamWriter.FlushAsync();

        // This indicates the response is complete
        return new EmptyResult();
    }
    
    [HttpPost("Data/Export")]
    public async Task<IActionResult> ExportDataByBody([FromBody] FilterDto filter)
    {
        Log.Debug("Exporting...");   
        // Set the content type and filename for the browser
        Response.ContentType = "text/csv";
        Response.Headers.Add("Content-Disposition", "attachment; filename=cs_export.csv");

        // Use a StreamWriter on the response body to avoid creating a MemoryStream
        await using var streamWriter = new StreamWriter(Response.Body, leaveOpen: true);
        await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

        // EF Core 6+ supports IAsyncEnumerable, which is crucial for streaming
        var res = await _mediator.Send(new GetAggregateFilteredExportQuery(filter));

        var data = res.Value;

        // Write the CSV header
        csvWriter.WriteHeader<FactAggregateVaccination>();
        await csvWriter.NextRecordAsync();

        // Write records one at a time as they are retrieved from the database
        await csvWriter.WriteRecordsAsync(data);

        // Explicitly flush to ensure all data is sent
        await streamWriter.FlushAsync();
        
        Log.Debug("Exporting complete !!");  

        // This indicates the response is complete
        return new EmptyResult();
    }

}