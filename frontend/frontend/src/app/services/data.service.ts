import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Flight } from '../components/table/table.component';


export interface FlightStatsPerDay {
  date: string;
  departuresCount: number;
  arrivalsCount: number;
}


@Injectable({
  providedIn: 'root'
})

export class DataService {
  private apiBaseUrl = 'https://localhost:7280/api/Flights/stats-per-hour';

  constructor(private http: HttpClient) {}

  getFlightsStats(from: string, to: string): Observable<FlightStatsPerDay[]> {
    const url = `${this.apiBaseUrl}?from=${encodeURIComponent(from)}&to=${encodeURIComponent(to)}`;
    return this.http.get<FlightStatsPerDay[]>(url);
  }

  getCountries(): Observable<string[]> {
    return this.http.get<string[]>(`https://localhost:7280/api/Flights/countries`);
  }

  getAllFlights():Observable<Flight[]> {
    return this.http.get<Flight[]>(`https://localhost:7280/api/Flights`);
  }
}




