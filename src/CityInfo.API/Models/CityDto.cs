namespace CityInfo.ASP.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public List<PointOfInterestDto> PointsOfInterest { get; set; }
            = new List<PointOfInterestDto>();
        public int NumberOfPointsOfInterest => PointsOfInterest.Count;
    }
}
