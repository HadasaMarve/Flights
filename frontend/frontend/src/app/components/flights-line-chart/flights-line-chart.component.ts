import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxChartsModule, ScaleType } from '@swimlane/ngx-charts';


@Component({
  selector: 'app-flights-line-chart',
  imports: [CommonModule, NgxChartsModule],
  templateUrl: './flights-line-chart.component.html',
  styleUrl: './flights-line-chart.component.css'
})
export class FlightsLineChartComponent {

  @Input() data: any[] = [];

  view: [number, number] = [800, 400];

  showXAxis = true;
  showYAxis = true;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = 'תאריך';
  showYAxisLabel = true;
  yAxisLabel = 'מספר טיסות';

  colorScheme = {
    name: 'cool',
    selectable: true,
    group: ScaleType.Ordinal,
    domain: ['#42A5F5', '#66BB6A']
  };
}
