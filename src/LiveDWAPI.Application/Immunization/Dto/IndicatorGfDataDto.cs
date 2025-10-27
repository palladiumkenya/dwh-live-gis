using LiveDWAPI.Domain.Immunization;

namespace LiveDWAPI.Application.Immunization.Dto
{
    public class VaccineDataDto
    {
        public List<FactFacilityPointDto> FacilityPoints { get; set; }= new ();

        public VaccineDataDto()
        {
        }
        
        public VaccineDataDto(List<FactAggregateVaccination> vaccines)
        {
            FacilityPoints = FactFacilityPointDto.Generate(vaccines);
        }
    }
}
