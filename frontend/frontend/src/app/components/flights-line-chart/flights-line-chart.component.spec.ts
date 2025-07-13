import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightsLineChartComponent } from './flights-line-chart.component';

describe('FlightsLineChartComponent', () => {
  let component: FlightsLineChartComponent;
  let fixture: ComponentFixture<FlightsLineChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FlightsLineChartComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FlightsLineChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
