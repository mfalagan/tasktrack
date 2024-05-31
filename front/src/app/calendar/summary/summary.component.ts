import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DatePipe } from "../../pipes/date.pipe";

@Component({
    selector: 'app-summary',
    standalone: true,
    templateUrl: './summary.component.html',
    styleUrl: './summary.component.css',
    imports: [DatePipe]
})
export class SummaryComponent {
  @Input() events: any[] = [];
  @Input() day: Date = new Date(Date.now());
  @Output() closePopup = new EventEmitter<void>();

  constructor() { }

  emitClosePopup(): void {
    this.closePopup.emit();
  }
}
