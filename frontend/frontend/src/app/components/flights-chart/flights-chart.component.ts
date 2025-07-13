import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxChartsModule, ScaleType } from '@swimlane/ngx-charts';
import { DataService, FlightStatsPerDay } from '../../services/data.service';
import type { Color } from '@swimlane/ngx-charts';
import { FlightsLineChartComponent } from '../flights-line-chart/flights-line-chart.component';

@Component({
  selector: 'app-flights-chart',
  standalone: true,
  imports: [CommonModule, NgxChartsModule,FlightsLineChartComponent],
  templateUrl: './flights-chart.component.html',
  styleUrls: ['./flights-chart.component.css']
})
export class FlightsChartComponent implements OnInit {
  data: any[] = [];
  view: [number, number] = [800, 400];
  chartType: 'bar' | 'line' = 'bar';

  // אפשרויות תצוגה
  showXAxis = true;
  showYAxis = true;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = 'תאריך';
  showYAxisLabel = true;
  yAxisLabel = 'מספר טיסות';

  colorScheme: Color = {
    name: 'customScheme',
    selectable: true,
    group: ScaleType.Ordinal,
    domain: ['#4afffe', '#33c0fc']   };

  selectedRange = 'week';

  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.loadData(this.selectedRange);
  }

  loadData(range: string) {
    this.selectedRange = range;
    const now = new Date();
    let from = new Date();

    if (range === 'week') {
         from.setDate(now.getDate() - 7);
         this.chartType = 'bar';
    }
 
    else if (range === 'month') from.setMonth(now.getMonth() - 1);
    else if (range === 'halfYear') from.setMonth(now.getMonth() - 6);
    else if (range === 'year') {
      from.setFullYear(now.getFullYear() - 1);
      this.chartType = 'line'; 
    } else {
      this.chartType = 'bar';
    }
    const fromStr = from.toISOString().split('T')[0];
    const toStr = now.toISOString().split('T')[0];


    this.dataService.getFlightsStats(fromStr, toStr).subscribe(apiData => {
      this.data = this.mapToGroupedData(apiData);
    });
  }

  mapToGroupedData(apiData: FlightStatsPerDay[]): any[] {
    return apiData.map((day: FlightStatsPerDay) => ({
      name: new Date(day.date).toLocaleDateString('he-IL', { weekday: 'short', month: 'short', day: 'numeric' }),
      series: [
        { name: 'המראות', value: day.departuresCount },
        { name: 'נחיתות', value: day.arrivalsCount }
      ]
    }));
  }
}
