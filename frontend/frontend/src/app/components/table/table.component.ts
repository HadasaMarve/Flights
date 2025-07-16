import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgxChartsModule, ScaleType } from '@swimlane/ngx-charts';
import { DataService, FlightStatsPerDay } from '../../services/data.service';

export interface Flight {
  id: number;
  operatorCode: string;
  flightNumber: string;
  operatorName: string;
  scheduledTakeoff: string; // ISO date
  actualTakeoff: string;    // ISO date
  arrivalOrDeparture: 'A' | 'D';
  airportCode: string;
  airportNameEn: string;
  airportNameHe: string;
  countryHe: string;
  countryEn: string;
  terminal: number;
  statusEn: string;
  statusHe: string;
}


@Component({
  selector: 'app-table',
  imports: [FormsModule,CommonModule],
  templateUrl: './table.component.html',
  styleUrl: './table.component.css'
})



export class TableComponent implements OnInit{

  flights: Flight[] = [];
  loading = true;

  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.dataService.getAllFlights().subscribe({
      next: (apiData) => {
        this.flights = apiData;
        this.loading = false;
      },
      error: (err) => {
        console.error('שגיאה בשליפת טיסות', err);
        this.loading = false;
      }
    });
  }

  getStatusClass(statusEn: string): string {
    switch (statusEn) {
      case 'LANDED':
      case 'DEPARTED':
        return 'status-ok';
      case 'DELAYED':
        return 'status-warn';
      case 'CANCELLED':
        return 'status-error';
      default:
        return '';
    }
  }
  

}
