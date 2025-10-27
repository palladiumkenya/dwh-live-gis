using AutoMapper;
using LiveDWAPI.Domain.Immunization;

namespace LiveDWAPI.Application.Immunization.Dto;

public class ImmunizationProfile:Profile
{
    public ImmunizationProfile()
    {
        CreateMap<FactAggregateVaccination, FactFacilityPointDto>();
    }
}