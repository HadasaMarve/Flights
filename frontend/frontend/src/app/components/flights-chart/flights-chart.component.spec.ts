import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightsChartComponent } from './flights-chart.component';

describe('FlightsChartComponent', () => {
  let component: FlightsChartComponent;
  let fixture: ComponentFixture<FlightsChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FlightsChartComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FlightsChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
