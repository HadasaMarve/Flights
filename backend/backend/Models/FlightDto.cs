using System;

namespace backend.Models
{
    public class FlightDto
    {
        public int Id { get; set; }                   // _id
        public string OperatorCode { get; set; }      // CHOPER
        public string FlightNumber { get; set; }      // CHFLTN
        public string OperatorName { get; set; }      // CHOPERD
        public DateTime? ScheduledTakeoff { get; set; } // CHSTOL
        public DateTime? ActualTakeoff { get; set; }    // CHPTOL
        public string ArrivalOrDeparture { get; set; }  // CHAORD ("A"=Arrival, "D"=Departure)
        public string AirportCode { get; set; }         // CHLOC1
        public string AirportNameEn { get; set; }       // CHLOC1D
        public string AirportNameHe { get; set; }       // CHLOC1TH
        public string AirportName { get; set; }         // CHLOC1T
        public string CountryHe { get; set; }           // CHLOC1CH
        public string CountryEn { get; set; }           // CHLOCCT
        public int? Terminal { get; set; }              // CHTERM
        public string StatusEn { get; set; }            // CHRMINE
        public string StatusHe { get; set; }            // CHRMINH
    }
}
