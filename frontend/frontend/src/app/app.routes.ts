import { Routes } from '@angular/router';
import { FlightsChartComponent } from './components/flights-chart/flights-chart.component';
import { HomeComponentComponent } from './components/home-component/home-component.component';
import { TableComponent } from './components/table/table.component';

export const appRoutes: Routes = [
  { path: 'flights-bar-plot', component: FlightsChartComponent },
  { path: 'flights-table', component: TableComponent },
  { path: '', component: HomeComponentComponent},
];
