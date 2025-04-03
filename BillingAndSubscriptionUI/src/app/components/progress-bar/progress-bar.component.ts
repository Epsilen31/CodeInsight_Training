import { Component, Input, OnDestroy, OnInit, Output, EventEmitter, NgZone } from '@angular/core';

@Component({
  selector: 'app-progress-bar',
  templateUrl: './progress-bar.component.html',
  styleUrl: './progress-bar.component.scss',
  standalone: false
})
export class ProgressBarComponent implements OnInit, OnDestroy {
  @Input() progress: number = 0;
  @Input() total: number = 100;
  @Input() autoIncrement: boolean = false;
  @Input() intervalTime: number = 1000;
  @Output() progressComplete: EventEmitter<void> = new EventEmitter<void>();

  private interval: ReturnType<typeof setInterval> | undefined;

  constructor(private readonly _ngZone: NgZone) {}

  ngOnInit(): void {
    if (this.autoIncrement) {
      this.startAutoProgress();
    }
  }

  private startAutoProgress(): void {
    this._ngZone.runOutsideAngular((): void => {
      this.interval = setInterval((): void => {
        this._ngZone.run((): void => {
          this.progress += Math.floor(Math.random() * 10) + 1;

          if (this.progress >= this.total) {
            this.progress = this.total;
            clearInterval(this.interval);
            this.progressComplete.emit();
          }

          this.updateProgress();
        });
      }, this.intervalTime);
    });
  }

  private updateProgress(): void {
    if (this.progress > this.total) {
      this.progress = this.total;
    }
  }

  ngOnDestroy(): void {
    if (this.interval) {
      clearInterval(this.interval);
    }
  }
}
