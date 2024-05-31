import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { CalendarComponent } from './calendar/calendar.component';

@Component({
	selector: 'app-root',
	standalone: true,
	imports: [RouterOutlet, FormsModule, CalendarComponent],
	templateUrl: './app.component.html',
	styleUrl: './app.component.css'
})
export class AppComponent {
	title = 'front';
	events = 
		[
			{
				title: "prueba", 
				description: "evento de prueba", 
				date: new Date(Date.now())
			}
		];
}
