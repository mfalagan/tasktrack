import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DatePipe } from "../../pipes/date.pipe";
import { EditEventComponent } from "../edit-event/edit-event.component";
import { EventData, EventEntry } from '../../../codegen';

@Component({
  selector: 'app-summary',
  standalone: true,
  templateUrl: './summary.component.html',
  styleUrl: './summary.component.css',
  imports: [DatePipe, EditEventComponent]
})
export class SummaryComponent {
  @Input() events: EventEntry[] = [];
  @Input() day: Date = new Date(Date.now());
  @Output() closePopup = new EventEmitter<void>();
  @Output() deleteEvent = new EventEmitter<EventEntry>();
  @Output() updateEvent = new EventEmitter<EventEntry>();
  @Output() addEvent = new EventEmitter<EventData>();
  eventToEdit: EventData | null = null;

  constructor() { }

  emitClosePopup(): void {
    this.eventToEdit = null;
    this.closePopup.emit();
  }

  editEvent(event: EventData): void {
    this.eventToEdit = event;
  }
  cancelEdit(): void {
    this.eventToEdit = null;
  }
  persistEdit(event: EventData): void {
    if ('id' in event && typeof event.id === 'number') {
      this.updateEvent.emit(event as EventEntry);
    } else {
      this.addEvent.emit(event);
    }
    this.eventToEdit = null;
  }

  emitDeleteEvent(event: EventEntry): void {
    this.deleteEvent.emit(event);
  }

  startAddEvent(): void {
    this.eventToEdit = {
      title: '',
      description: '',
      priority: 1,
      dueDate: {
        year: this.day.getFullYear(),
        month: this.day.getMonth(),
        day: this.day.getDate()
      }
    } as EventData;
  }
}
