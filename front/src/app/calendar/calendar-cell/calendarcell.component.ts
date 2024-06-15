import { Component, Input } from '@angular/core';
import { EventEntry } from '../../../codegen';

@Component({
	selector: 'app-calendar-cell',
	standalone: true,
	imports: [],
	templateUrl: './calendarcell.component.html',
	styleUrl: './calendarcell.component.css'
})
export class CalendarCellComponent {
	@Input() day: Date = new Date(Date.now());
	@Input() events: EventEntry[] = [];
	dayNumber: number = 1;
	isToday: boolean = false;

	ngOnChanges() {
		this.dayNumber = this.day.getDate();
		this.isToday = this.day.toDateString() === (new Date(Date.now())).toDateString();
	}
}
