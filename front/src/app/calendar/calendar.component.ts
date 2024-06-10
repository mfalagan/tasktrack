import { 
	Component,
	Input,
	SimpleChanges,
 } from '@angular/core';
import { CalendarCellComponent } from "./calendar-cell/calendarcell.component";
import { FormsModule } from '@angular/forms';
import { SummaryComponent } from "./summary/summary.component";

@Component({
    selector: 'app-calendar',
    standalone: true,
    templateUrl: './calendar.component.html',
    styleUrl: './calendar.component.css',
    imports: [CalendarCellComponent, FormsModule, SummaryComponent]
})
export class CalendarComponent {
	@Input() year: number = new Date(Date.now()).getFullYear();
	@Input() month: number = new Date(Date.now()).getMonth() + 1;
	days: Date[][] = [];
	month_name: string = "";

	day_selected: Date | null = null;
	// remove this line
	events = [{title: "hola", description: "adios", date: new Date(Date.now())}];

	ngOnChanges(changes: SimpleChanges) {
		this.updateCalendar();
	}
	ngOnInit() {
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

		this.month_name = [
			"January", 
			"February", 
			"March", 
			"April", 
			"May", 
			"June",
			"July", 
			"August", 
			"September", 
			"October", 
			"November", 
			"December"][this.month - 1];
	}

	showSummary(day: Date): void {
		this.day_selected = day;
	}
	hideSummary(): void {
		this.day_selected = null;
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