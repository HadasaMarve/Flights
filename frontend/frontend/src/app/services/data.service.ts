import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


export interface FlightStatsPerDay {
  date: string;
  departuresCount: number;
  arrivalsCount: number;
}


@Injectable({
  providedIn: 'root'
})


// export class DataService {

//   private apiBaseUrl = 'https://localhost:7280/api/Flights/stats-per-day';

//   constructor(private http: HttpClient) {}

//   getData(): Observable<any> {

//     this.http.get<any>(this.apiUrl).subscribe(data => {
//       console.log(' נתונים מה-API:', data); 
//     });
//     return this.http.get(this.apiUrl);
//   }
// }
export class DataService {
  private apiBaseUrl = 'https://localhost:7280/api/Flights/stats-per-day';

  constructor(private http: HttpClient) {}

  getFlightsStats(from: string, to: string): Observable<FlightStatsPerDay[]> {
    const url = `${this.apiBaseUrl}?from=${encodeURIComponent(from)}&to=${encodeURIComponent(to)}`;
    return this.http.get<FlightStatsPerDay[]>(url);
  }
}




