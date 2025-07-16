namespace backend.Models
{
    public class FlightHourStatsDto
    {
        public DateTime Hour { get; set; }
        public int DeparturesCount { get; set; }
        public int ArrivalsCount { get; set; }
    }

}
