using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using backend.Models;
using System;

namespace backend.Services
{
    public class FlightApiService
    {
        private readonly HttpClient _httpClient;

        public FlightApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // פונקציה שמביאה את כל הטיסות מה-API וממפה ל-FlightDto
        public async Task<List<FlightDto>> FetchAllFlightsAsync()
        {
            string url = "https://data.gov.il/api/3/action/datastore_search?resource_id=e83f763b-b7d7-479e-b172-ae981ddc6de5&limit=10000";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var rawJson = await response.Content.ReadAsStringAsync();

            var doc = JsonDocument.Parse(rawJson);
            var records = doc.RootElement
                .GetProperty("result")
                .GetProperty("records")
                .EnumerateArray();

            var list = new List<FlightDto>();
            foreach (var record in records)
            {
                var dto = new FlightDto
                {
                    Id = record.GetProperty("_id").GetInt32(),
                    OperatorCode = record.GetProperty("CHOPER").GetString(),
                    FlightNumber = record.GetProperty("CHFLTN").GetString(),
                    OperatorName = record.GetProperty("CHOPERD").GetString(),
                    ScheduledTakeoff = ParseDateTime(record, "CHSTOL"),
                    ActualTakeoff = ParseDateTime(record, "CHPTOL"),
                    ArrivalOrDeparture = record.GetProperty("CHAORD").GetString(),
                    AirportCode = record.GetProperty("CHLOC1").GetString(),
                    AirportNameEn = record.GetProperty("CHLOC1D").GetString(),
                    AirportNameHe = record.GetProperty("CHLOC1TH").GetString(),
                    AirportName = record.GetProperty("CHLOC1T").GetString(),
                    CountryHe = record.GetProperty("CHLOC1CH").GetString(),
                    CountryEn = record.GetProperty("CHLOCCT").GetString(),
                    Terminal = record.TryGetProperty("CHTERM", out var term) && term.ValueKind == JsonValueKind.Number ? term.GetInt32() : (int?)null,
                    StatusEn = record.GetProperty("CHRMINE").GetString(),
                    StatusHe = record.GetProperty("CHRMINH").GetString(),
                };
                list.Add(dto);
            }

            return list;
        }

        // פונקציה לסינון טיסות לפי פרמטרים
        public async Task<List<FlightDto>> GetFlightsAsync(
            string operatorCode = null,
            string flightNumber = null,
            string arrivalOrDeparture = null,
            string airportCode = null,
            string country = null,
            DateTime? from = null,
            DateTime? to = null,
            string status = null)
        {
            var list = await FetchAllFlightsAsync();

            // סינונים
            if (!string.IsNullOrEmpty(operatorCode))
                list = list.Where(x => x.OperatorCode == operatorCode).ToList();

            if (!string.IsNullOrEmpty(flightNumber))
                list = list.Where(x => x.FlightNumber == flightNumber).ToList();

            if (!string.IsNullOrEmpty(arrivalOrDeparture))
                list = list.Where(x => x.ArrivalOrDeparture == arrivalOrDeparture.ToUpper()).ToList();

            if (!string.IsNullOrEmpty(airportCode))
                list = list.Where(x => x.AirportCode == airportCode).ToList();

            if (!string.IsNullOrEmpty(country))
                list = list.Where(x => (x.CountryEn != null && x.CountryEn.ToUpper().Contains(country.ToUpper()))
                                    || (x.CountryHe != null && x.CountryHe.Contains(country))).ToList();

            if (from.HasValue)
                list = list.Where(x => x.ScheduledTakeoff.HasValue && x.ScheduledTakeoff.Value >= from.Value).ToList();

            if (to.HasValue)
                list = list.Where(x => x.ScheduledTakeoff.HasValue && x.ScheduledTakeoff.Value <= to.Value).ToList();

            if (!string.IsNullOrEmpty(status))
                list = list.Where(x => (x.StatusEn != null && x.StatusEn.ToUpper().Contains(status.ToUpper()))
                                   || (x.StatusHe != null && x.StatusHe.Contains(status))).ToList();

            return list;
        }

        // דוגמה לפונקציה נוספת שמחזירה רק טיסות שנחתו
        //public async Task<List<FlightDto>> GetLandedFlightsAsync()
        //{
        //    var allFlights = await FetchAllFlightsAsync();
        //    return allFlights.Where(f => f.StatusEn != null && f.StatusEn.ToUpper().Contains("LANDED")).ToList();
        //}

        private DateTime? ParseDateTime(JsonElement record, string propertyName)
        {
            if (!record.TryGetProperty(propertyName, out var element))
                return null;
            if (element.ValueKind != JsonValueKind.String)
                return null;
            if (DateTime.TryParse(element.GetString(), out var result))
                return result;
            return null;
        }

        //returns arrivals and departures count per day in the given date range
        public async Task<List<FlightDayStatsDto>> GetFlightsStatsPerDayAsync(DateTime from, DateTime to)
        {
            var flights = await FetchAllFlightsAsync();

            // נבנה רשימה של ימים בטווח
            var days = Enumerable.Range(0, (to.Date - from.Date).Days + 1)
                .Select(offset => from.Date.AddDays(offset))
                .ToList();

            var result = new List<FlightDayStatsDto>();

            foreach (var day in days)
            {
                var departures = flights.Count(f =>
                    f.ScheduledTakeoff.HasValue &&
                    f.ScheduledTakeoff.Value.Date == day &&
                    f.ArrivalOrDeparture == "D");

                var arrivals = flights.Count(f =>
                    f.ScheduledTakeoff.HasValue &&
                    f.ScheduledTakeoff.Value.Date == day &&
                    f.ArrivalOrDeparture == "A");

                result.Add(new FlightDayStatsDto
                {
                    Date = day,
                    DeparturesCount = departures,
                    ArrivalsCount = arrivals
                });
            }

            return result;
        }

        //returns sum of flights for each company
        public async Task<List<FlightCompanyStatsDto>> GetFlightsCountByCompanyAsync()
        {
            var flights = await FetchAllFlightsAsync();

            var result = flights
                .GroupBy(f => new { f.OperatorCode, f.OperatorName })
                .Select(g => new FlightCompanyStatsDto
                {
                    OperatorCode = g.Key.OperatorCode,
                    OperatorName = g.Key.OperatorName,
                    FlightsCount = g.Count()
                })
                .OrderByDescending(x => x.FlightsCount)
                .ToList();

            return result;
        }


        //returns sum of flights for each country by a company
        public async Task<List<FlightCountByCountryDto>> GetFlightsCountPerCountryByCompanyAsync(string operatorName)
        {
            var flights = await FetchAllFlightsAsync();

            var result = flights
                .Where(f => f.OperatorName.Equals(operatorName, StringComparison.OrdinalIgnoreCase))
                .GroupBy(f => f.CountryEn)
                .Select(g => new FlightCountByCountryDto
                {
                    Country = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            return result;
        }


        //returns sum of flights for each company by a country
        public async Task<List<FlightCountByCompanyDto>> GetFlightsCountPerCompanyByCountryAsync(string country)
        {
            var flights = await FetchAllFlightsAsync();

            var result = flights
                .Where(f => f.CountryEn.Equals(country, StringComparison.OrdinalIgnoreCase))
                .GroupBy(f => f.OperatorName)
                .Select(g => new FlightCountByCompanyDto
                {
                    Company = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            return result;
        }

        //returns arrivals and departures count per hour in the given hours range
        public async Task<List<FlightHourStatsDto>> GetFlightsStatsPerHourAsync(DateTime from, DateTime to)
        {
            // קבלת התאריך הנוכחי
            var now = DateTime.UtcNow;

            // הגדרת טווח הזמן ל-24 שעות האחרונות
            var startOfDay = now.AddDays(-1);

            // קבלת כל הטיסות ביממה האחרונה
            var flights = await FetchAllFlightsAsync();
            var filteredFlights = flights.Where(f =>
                f.ScheduledTakeoff.HasValue &&
                f.ScheduledTakeoff.Value >= startOfDay &&
                f.ScheduledTakeoff.Value <= now).ToList();

            // נבנה רשימה של שעות בטווח
            var hours = Enumerable.Range(0, (int)(to - from).TotalHours + 1)
                .Select(offset => from.AddHours(offset))
                .ToList();

            var result = new List<FlightHourStatsDto>();

            foreach (var hour in hours)
            {
                var departures = filteredFlights.Count(f =>
                    f.ScheduledTakeoff.Value >= hour &&
                    f.ScheduledTakeoff.Value < hour.AddHours(1) &&
                    f.ArrivalOrDeparture == "D");

                var arrivals = filteredFlights.Count(f =>
                    f.ScheduledTakeoff.Value >= hour &&
                    f.ScheduledTakeoff.Value < hour.AddHours(1) &&
                    f.ArrivalOrDeparture == "A");

                result.Add(new FlightHourStatsDto
                {
                    Hour = hour,
                    DeparturesCount = departures,
                    ArrivalsCount = arrivals
                });
            }

            return result;
        }



        // returns list of all countries
        public async Task<List<string>> GetAllCountriesAsync()
        {
            var allFlights = await FetchAllFlightsAsync();
            return allFlights.Select(f=>f.CountryEn).Distinct().ToList();
        }

    }
}