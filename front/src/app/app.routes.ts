import { Routes } from '@angular/router';
import { CalendarComponent } from './calendar/calendar.component';
import { LandingComponent } from './landing/landing.component';

export const routes: Routes = [
    { path: 'calendar', component: CalendarComponent },
    { path: '', component: LandingComponent}
];