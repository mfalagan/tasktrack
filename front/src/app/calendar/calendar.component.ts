import {
	Component,
	Input,
	SimpleChanges, OnChanges,
} from '@angular/core';
import { CalendarCellComponent } from "./calendar-cell/calendarcell.component";
import { FormsModule } from '@angular/forms';
import { SummaryComponent } from "./summary/summary.component";
import { DateDataFromDate, EventData, EventEntry, EventService, toDateString } from '../../codegen';
import { Router } from '@angular/router';

@Component({
	selector: 'app-calendar',
	standalone: true,
	templateUrl: './calendar.component.html',
	styleUrl: './calendar.component.css',
	imports: [CalendarCellComponent, FormsModule, SummaryComponent]
})
export class CalendarComponent implements OnChanges {
	@Input() year: number = new Date(Date.now()).getFullYear();
	@Input() month: number = new Date(Date.now()).getMonth() + 1;
	@Input() events: Map<string, EventEntry[]> = new Map<string, EventEntry[]>();
	days: Date[][] = [];
	month_name: string = "";

	day_selected: Date | null = null;

	constructor(private eventService: EventService, private router: Router) {
		this.updateCalendar();
		this.eventService.eventsGet().subscribe({
			next: (events) => {
				events.forEach(event => this.addLocalEvent(event));
			},
			error: (error) => {
				console.error('Error getting events:', error);
			}
		});
	}

	ngOnChanges() {
		this.updateCalendar();
	}

	updateCalendar(): void {
		this.days = [];
		const firstDayOfMonth = new Date(this.year, this.month - 1, 1);
		const lastDayOfMonth = new Date(this.year, this.month, 0);
		const dayToDraw = this.getStartDay(firstDayOfMonth);

		let keepDrawing = true;
		while (keepDrawing) {
			const week: Date[] = [];
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

	addEvent(event: EventData): void {
		this.eventService.eventsPost(event).subscribe({
			next: (newEvent) => {
				console.log('Event added:', newEvent);
				this.addLocalEvent(newEvent);
			},
			error: (error) => {
				console.error('Error adding event:', error);
			}
		});
	}
	addLocalEvent(event: EventEntry): void {
		this.events.set(toDateString(event.dueDate!), this.events.get(toDateString(event.dueDate!))?.concat(event) || [event]);
	}

	updateEvent(event: EventEntry): void {
		const id: number = event.id!;
		const eventData = event as EventData;

		console.log('Updating event:', eventData);

		this.eventService.eventsIdPut(id, eventData).subscribe({
			next: (updatedEvent) => {
				console.log('Event updated:', updatedEvent);
				this.updateLocalEvent(updatedEvent);
			},
			error: (error) => {
				console.error('Error updating event:', error);
			}
		});
	}
	updateLocalEvent(event: EventEntry): void {
		this.events.set(
			toDateString(event.dueDate!), 
			this.events.get(toDateString(event.dueDate!))?.map(e => e.id === event.id ? event : e) || []
		);
	}

	deleteEvent(event: EventEntry): void {
		let id: number = event.id!;

		this.eventService.eventsIdDelete(id).subscribe({
			next: (response) => {
				console.log('Event deleted:', response);
				this.deleteLocalEvent(event);
			},
			error: (error) => {
				console.error('Error deleting event:', error);
			}
		});
	}
	deleteLocalEvent(event: EventEntry): void {
		this.events.set(toDateString(event.dueDate!), this.events.get(toDateString(event.dueDate!))?.filter(e => e.id !== event.id) || []);
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

	onLogout() {
		localStorage.removeItem('jwtToken'); // Clear the JWT token
		this.router.navigate(['/']); // Redirect to home or login page
	}

	dateToKey(date: Date): string {
		return toDateString(DateDataFromDate(date));
	}
}