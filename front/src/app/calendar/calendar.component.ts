import { 
	Component,
	Input,
	SimpleChanges,
 } from '@angular/core';
import { CalendarcellComponent } from "./calendarcell/calendarcell.component";

@Component({
		selector: 'app-calendar',
		standalone: true,
		templateUrl: './calendar.component.html',
		styleUrl: './calendar.component.css',
		imports: [CalendarcellComponent]
})
export class CalendarComponent {
	@Input() year: number = new Date().getFullYear();
	@Input() month: number = new Date().getMonth() + 1; // Months are 1-indexed for user-friendliness
	days: Date[][] = [];

	ngOnChanges(changes: SimpleChanges) {
		this.updateCalendar();
	}

	updateCalendar(): void {
		this.days = [];
		const firstDayOfMonth = new Date(this.year, this.month - 1, 1);
		const lastDayOfMonth = new Date(this.year, this.month, 0);
		let dayToDraw = this.getStartDay(firstDayOfMonth);

		let keepDrawing = true;
		while (keepDrawing) {
			let week: Date[] = [];
			for (let i = 0; i < 7; ++i) {
				if (dayToDraw >= lastDayOfMonth && dayToDraw.getDay() === 0)
						keepDrawing = false;
				
				week.push(new Date(dayToDraw));
				dayToDraw.setDate(dayToDraw.getDate() + 1);
			}
			this.days.push(week);
		}
	}

	getStartDay(firstDayOfMonth: Date): Date {
		const dayOfWeek = firstDayOfMonth.getDay(); // 0 (Sunday) to 6 (Saturday)
		return new Date(firstDayOfMonth.setDate(firstDayOfMonth.getDate() - (dayOfWeek ? dayOfWeek - 1 : 6)));
	}

	goToNextMonth(): void {
		if (this.month === 12) {
			this.month = 1;
			this.year++;
		} else {
			this.month++;
		}
		this.updateCalendar();
	}

	goToPreviousMonth(): void {
		if (this.month === 1) {
			this.month = 12;
			this.year--;
		} else {
			this.month--;
		}
		this.updateCalendar();
	}
}