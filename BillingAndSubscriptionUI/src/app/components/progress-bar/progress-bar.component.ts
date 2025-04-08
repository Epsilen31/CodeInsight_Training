import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-progress-bar',
  templateUrl: './progress-bar.component.html',
  styleUrl: './progress-bar.component.scss',
  standalone: false
})
export class ProgressBarComponent implements OnChanges {
  @Input() progress: number = 0;
  @Input() total: number = 100;
  @Output() progressComplete: EventEmitter<void> = new EventEmitter<void>();

  ngOnChanges(changes: SimpleChanges): void {
    if (this.progress >= this.total) {
      this.progress = this.total;
      this.progressComplete.emit();
    }
  }
}
