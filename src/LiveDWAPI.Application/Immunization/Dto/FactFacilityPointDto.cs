using LiveDWAPI.Domain.Immunization;

namespace LiveDWAPI.Application.Immunization.Dto;

public class FactFacilityPointDto
{
    public string? FacilityName { get; set; }
    public decimal? Lat { get; set; }
    public decimal? Long { get; set; }
    public int? Count { get; set; }
    public static List<FactFacilityPointDto> Generate(List<FactAggregateVaccination> vaccines)
    {
        var points = vaccines
            .GroupBy(x=>new{x.FacilityName})
            .Select(g=> new FactFacilityPointDto()
            {
                FacilityName = g.Key.FacilityName,
                Lat= g.First().Lat,
                Long= g.First().Long,
                Count = g.Sum(x=>x.Numerator)
            })
            .ToList();
        
        return points;
    }
}