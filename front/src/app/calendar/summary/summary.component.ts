import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DatePipe } from "../../pipes/date.pipe";
import { EventInternal } from '../../../models/Event';
import { EditEventComponent } from "../edit-event/edit-event.component";

@Component({
    selector: 'app-summary',
    standalone: true,
    templateUrl: './summary.component.html',
    styleUrl: './summary.component.css',
    imports: [DatePipe, EditEventComponent]
})
export class SummaryComponent {
  @Input() events: EventInternal[] | undefined = [];
  @Input() day: Date = new Date(Date.now());
  @Output() closePopup = new EventEmitter<void>();
  @Output() deleteEvent = new EventEmitter<EventInternal>();
  @Output() addEvent = new EventEmitter<EventInternal>();
  eventToEdit: EventInternal | null = null;

  constructor() { }

  emitClosePopup(): void {
    this.eventToEdit = null;
    this.closePopup.emit();
  }

  editEvent(event: EventInternal): void {
    this.eventToEdit = event;
  }
  cancelEdit(): void {
    this.eventToEdit = null;
  }
  persistEdit(event: EventInternal): void {
    this.eventToEdit = null;
    this.addEvent.emit(event);
  }

  emitDeleteEvent(event: EventInternal): void {
    this.deleteEvent.emit(event);
  }

  startAddEvent(): void {
    this.eventToEdit = { id: null, title: '', description: '', priority: 1, date: this.day };
  }
}
