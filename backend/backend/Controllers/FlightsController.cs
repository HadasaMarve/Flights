using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.Services;
using System;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly FlightApiService _flightApiService;
        
        public FlightsController(FlightApiService flightApiService)
        {
            _flightApiService = flightApiService;
        }

        //returns all flights with filterings
        //GET /api/flights?status=%D7%A0%D7%97%D7%AA%D7%94
        //  {
        //  "id": 1,
        //  "operatorCode": "LIL",
        //  "flightNumber": "9128",
        //  "operatorName": "FLY LILI",
        //  "scheduledTakeoff": "2025-07-12T00:45:00",
        //  "actualTakeoff": "2025-07-12T01:14:00",
        //  "arrivalOrDeparture": "A",
        //  "airportCode": "BSL",
        //  "airportNameEn": "BASEL",
        //  "airportNameHe": "באזל",
        //  "airportName": "BASEL",
        //  "countryHe": "שוויץ",
        //  "countryEn": "SWITZERLAND",
        //  "terminal": 3,
        //  "statusEn": "LANDED",
        //  "statusHe": "נחתה"
        //},
        [HttpGet]
        public async Task<IActionResult> Get(
    [FromQuery] string? operatorCode = null,
    [FromQuery] string? flightNumber = null,
    [FromQuery] string? arrivalOrDeparture = null,
    [FromQuery] string? airportCode = null,
    [FromQuery] string? country = null,
    [FromQuery] DateTime? fromDate = null,
    [FromQuery] DateTime? toDate = null,
    [FromQuery] string? status = null)
        {
            var result = await _flightApiService.GetFlightsAsync(
                operatorCode, flightNumber, arrivalOrDeparture, airportCode, country, fromDate, toDate, status
            );
            return Ok(result);
        }


        //returns arrivals and departures count per day in the given date range
        //GET /api/flights/stats-per-day? from = 2025 - 07 - 05 & to = 2025 - 07 - 11
        //  {
        //  "date": "2025-07-12T00:00:00",
        //  "departuresCount": 101,
        //  "arrivalsCount": 98
        //},
        [HttpGet("stats-per-day")]
        public async Task<IActionResult> GetStatsPerDay(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            if (from > to)
                return BadRequest("from date must be before to date");

            var stats = await _flightApiService.GetFlightsStatsPerDayAsync(from, to);
            return Ok(stats);
        }


        //returns sum of flights for each company
        //GET /api/flights/company-stats
        //  {
        //  "operatorCode": "LY",
        //  "operatorName": "EL AL ISRAEL AIRLINES",
        //  "flightsCount": 503
        //},
        [HttpGet("company-stats")]
        public async Task<ActionResult<List<FlightCompanyStatsDto>>> GetCompanyStats()
        {
            var stats = await _flightApiService.GetFlightsCountByCompanyAsync();
            return Ok(stats);
        }
    }
}
