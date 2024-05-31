import { Component, Input } from '@angular/core';

@Component({
	selector: 'app-calendar-cell',
	standalone: true,
	imports: [],
	templateUrl: './calendarcell.component.html',
	styleUrl: './calendarcell.component.css'
})
export class CalendarCellComponent {
	@Input() day: Date = new Date(Date.now());
	dayNumber: number = 1;
	events: number[] = this.generateRandomArray();
	isToday: boolean = false;

	generateRandomArray(): number[] {
		const length = Math.floor(Math.random() * 5);
		const array = Array.from({length}, () => 1 + Math.floor(Math.random() * 4));
		return array;
	}

	ngOnChanges() {
		this.dayNumber = this.day.getDate();
		this.isToday = this.day.toDateString() === (new Date(Date.now())).toDateString();
	}
}