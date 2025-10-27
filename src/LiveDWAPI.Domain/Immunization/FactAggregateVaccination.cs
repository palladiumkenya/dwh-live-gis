namespace LiveDWAPI.Domain.Immunization
{
    public class DimVaccine
    {
        public string? VaccineName { get; set; }
    }

    public class DimRegion
    {
        public string? FacilityName { get; set; }
    }
    
    public class DimSex
    {
        public string? Sex { get; set; }
    }
    
    public class DimAgeGroup
    {
        public string? AgeGroup { get; set; }
    }
    
    
    public class FactAggregateVaccination
    {
        public string? Vaccine { get; set; }
        public int? Numerator { get; set; }
     
        public DateTime? AssessmentPeriod { get; set; }
        public string? Sex { get; set; }
        public string? AgeGroup { get; set; }
        public string? FacilityName { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public decimal? Lat => GetLat();
        public decimal? Long => GetLong();
     
        private decimal? GetLat()
        {
            decimal.TryParse(Latitude, out var lat);
            return lat;
        }
        private decimal? GetLong()
        {
            decimal.TryParse(Longitude, out var lng);
            return lng;
        }
    }
}
