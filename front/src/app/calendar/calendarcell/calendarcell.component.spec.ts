import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarcellComponent } from './calendarcell.component';

describe('CalendarcellComponent', () => {
  let component: CalendarcellComponent;
  let fixture: ComponentFixture<CalendarcellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CalendarcellComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CalendarcellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
