namespace backend.Models
{
    public class FlightDayStatsDto
    {
        public DateTime Date { get; set; }
        public int DeparturesCount { get; set; }
        public int ArrivalsCount { get; set; }
    }
}