import { Component } from '@angular/core';
import { FlightsChartComponent } from './components/flights-chart/flights-chart.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';


@Component({
  selector: 'app-root',
  imports: [FlightsChartComponent, NgxChartsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  standalone: true
})
export class AppComponent {
  title = 'flightsProject';

  
}


