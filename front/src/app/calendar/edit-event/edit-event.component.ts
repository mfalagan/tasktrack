import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { EventData } from '../../../codegen';

@Component({
  selector: 'app-edit-event',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './edit-event.component.html',
  styleUrl: './edit-event.component.css'
})
export class EditEventComponent {
  @Input() event: EventData | null = null;
  @Output() updateEvent = new EventEmitter<EventData>();
  @Output() cancelEdit = new EventEmitter<void>();

  eventForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.eventForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', [Validators.required]],
      priority: ['', [Validators.required, Validators.min(1), Validators.max(5)]]
    });
  }

  ngOnChanges() {
    this.eventForm.reset({
      title: this.event?.title || '',
      description: this.event?.description || '',
      priority: this.event?.priority || 1
    });
  }


  submit() {
    if (this.eventForm.valid) {
      const updatedEvent: EventData = {
        ...this.event,
        ...this.eventForm.value
      };
      console.log("edit-event emmitting ", updatedEvent);
      this.updateEvent.emit(updatedEvent);
    }
  }

  cancel() {
    this.cancelEdit.emit();
  }
}
