import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgxChartsModule, ScaleType } from '@swimlane/ngx-charts';
import { DataService, FlightStatsPerDay } from '../../services/data.service';
import type { Color } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-flights-chart',
  standalone: true,
  imports: [CommonModule, NgxChartsModule,FormsModule],
  templateUrl: './flights-chart.component.html',
  styleUrls: ['./flights-chart.component.css']
})
export class FlightsChartComponent implements OnInit {
  data: any[] = [];
  view: [number, number] = [800, 400];
  chartType: 'bar' | 'line' = 'bar';

  // מדינות
  countries: string[] = [];
  selectedCountry: string = '';

  // אפשרויות תצוגה
  showXAxis = true;
  showYAxis = true;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = 'Hour';
  showYAxisLabel = true;
  yAxisLabel = 'Number of Flights';

  colorScheme: Color = {
    name: 'customScheme',
    selectable: true,
    group: ScaleType.Ordinal,
    domain: ['#4afffe', '#33c0fc']   };

  selectedRange = '1h';

  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    // this.loadCountries();     
    this.loadData(this.selectedRange);
  }

  // loadCountries() {
  //   this.dataService.getCountries().subscribe(countries => {
  //     this.countries = countries;
  //     console.log(countries)
  //     this.selectedCountry = countries[0]; 
  //     this.loadData(this.selectedRange);
  //   });
  // }

  loadData(range: string) {
    this.selectedRange = range;
    const now = new Date();
    let from = new Date();

    if (range === '1h') from.setHours(now.getHours() - 1);
    else if (range === '12h') from.setHours(now.getHours() - 12);
    else if (range === '24h') from.setHours(now.getHours() - 24);


    const fromStr = from.toISOString();
    const toStr = now.toISOString();

    console.log(fromStr,toStr)

    this.dataService.getFlightsStats(fromStr, toStr).subscribe(apiData => {
      console.log(this.data)
      this.data = this.mapToGroupedData(apiData);
    });
  }

  mapToGroupedData(apiData: any[]): any[] {
    return apiData.map((entry: any) => ({
      name: new Date(entry.hour).toLocaleTimeString('en-GB', {
        hour: '2-digit',
        minute: '2-digit'
      }),
      series: [
        { name: 'Departures', value: Number(entry.departuresCount) || 0 },
        { name: 'Arrivals', value: Number(entry.arrivalsCount) || 0 }
      ]
    }));
  }
  
}
