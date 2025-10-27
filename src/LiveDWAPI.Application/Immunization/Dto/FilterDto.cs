namespace LiveDWAPI.Application.Immunization.Dto;

public class FilterDto
{
    public string? Vaccine { get;set;  }
    // Place
    public string[]? FacilityName { get; set; }
    // Person
    public string[]? Sex { get; set; }
    public string[]? AgeGroup { get; set; }
    public int Limit = -1;
    public bool HasVaccine() => !string.IsNullOrWhiteSpace(Vaccine);
    public bool HasFacilityName() => FacilityName?.Length > 0;
    public bool HasSex() => Sex?.Length > 0;
    public bool HasAgeGroup() => AgeGroup?.Length > 0;
}