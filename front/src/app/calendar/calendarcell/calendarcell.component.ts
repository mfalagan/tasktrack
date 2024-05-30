import { Component, Input } from '@angular/core';

@Component({
	selector: 'app-calendarcell',
	standalone: true,
	imports: [],
	templateUrl: './calendarcell.component.html',
	styleUrl: './calendarcell.component.css'
})
export class CalendarcellComponent {
	@Input() day: Date = new Date(Date.now());
	dayNumber: number = 1;
	isToday: boolean = false;

	ngOnChanges() {
		this.dayNumber = this.day.getDate();
		this.isToday = this.day.toDateString() === (new Date(Date.now())).toDateString();
	}
}