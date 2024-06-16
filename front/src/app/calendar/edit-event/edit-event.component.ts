import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { EventData, PriorityLevel } from '../../../codegen';

@Component({
  selector: 'app-edit-event',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './edit-event.component.html',
  styleUrl: './edit-event.component.css'
})
export class EditEventComponent implements OnChanges {
  @Input() event: EventData | null = null;
  @Output() updateEvent = new EventEmitter<EventData>();
  @Output() cancelEdit = new EventEmitter<void>();

  eventForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.eventForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', [Validators.required]],
      priority: ['', [Validators.required, Validators.min(0), Validators.max(3)]]
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
      const formValue = this.eventForm.value;
      formValue.priority = +formValue.priority as PriorityLevel;

      const updatedEvent: EventData = {
        ...this.event,
        ...formValue
      };
      this.updateEvent.emit(updatedEvent);
    }
  }

  cancel() {
    this.cancelEdit.emit();
  }
}
