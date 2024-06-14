import { 
	Component,
	Input,
	SimpleChanges,
 } from '@angular/core';
import { CalendarCellComponent } from "./calendar-cell/calendarcell.component";
import { FormsModule } from '@angular/forms';
import { SummaryComponent } from "./summary/summary.component";
import { EventInternal } from "../../models/Event";
import { DayOfWeek, EventData, EventEntry, EventService, PriorityLevel } from '../../codegen';
import { Router } from '@angular/router';

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
	@Input() events: Map<string, EventInternal[]> = new Map<string, EventInternal[]>();
	days: Date[][] = [];
	month_name: string = "";

	day_selected: Date | null = null;

	constructor(private eventService: EventService, private router: Router) {
		this.updateCalendar();
	}

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

	addEvent(event: EventInternal): void {
		if (event.id) {
		  // Update existing event
		  this.eventService.eventsIdPut(event.id, this.toEventData(event)).subscribe({
			next: (updatedEvent) => {
			  console.log('Event updated:', this.toEventInternal(updatedEvent));
			  // Optionally update local event cache or UI components
			},
			error: (error) => {
			  console.error('Error updating event:', error);
			}
		  });
		  this.events.set(event.date.toString(), this.events.get(event.date.toString())?.map(e => e.id === event.id ? event : e) || []);
		} else {
		  // Create new event
		  this.eventService.eventsPost(this.toEventData(event)).subscribe({
			next: (newEvent) => {
			  console.log('Event added:', this.toEventInternal(newEvent));
			  // Optionally update local event cache or UI components
			},
			error: (error) => {
			  console.error('Error adding event:', error);
			}
		  });
		  this.events.set(event.date.toString(), this.events.get(event.date.toString())?.concat(event) || [event]);
		}
	  }
	  
	  deleteEvent(event: EventInternal): void {
		if (event.id) {
		  this.eventService.eventsIdDelete(event.id).subscribe({
			next: (response) => {
			  console.log('Event deleted:', response);
			  // Optionally update local event cache or UI components
			},
			error: (error) => {
			  console.error('Error deleting event:', error);
			}
		  });
		} else {
		  console.error('Attempted to delete an event without an ID');
		}
		this.events.set(event.date.toString(), this.events.get(event.date.toString())?.filter(e => e.id !== event.id) || []);
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

	toEventData(internal: EventInternal): EventData {
		return {
			title: internal.title,
			description: internal.description,
			dueDate: {
				year: internal.date.getFullYear(),
				month: internal.date.getMonth() + 1,  // JavaScript months are 0-indexed, .NET is 1-indexed
				day: internal.date.getDate(),
				dayOfWeek: internal.date.getDay() as DayOfWeek // Optional, if your backend requires it
				// dayOfYear and dayNumber are read-only in .NET and should not be sent
			},
			priority: internal.priority as PriorityLevel
		};
	}
	
	toEventInternal(entry: EventEntry): EventInternal {
		return {
			id: entry.id || null,
			title: entry.title || '',
			description: entry.description || '',
			date: new Date(entry.dueDate!.year!, entry.dueDate!.month! - 1, entry.dueDate!.day!),  // Assuming dueDate is always present
			priority: entry.priority as number  // Assuming priority uses same numbering
		};
	}

	onLogout() {
		localStorage.removeItem('jwtToken'); // Clear the JWT token
		this.router.navigate(['/']); // Redirect to home or login page
	  }
}