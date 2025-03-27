import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ErrorDialogService } from '../../services/error-dialog.service';

@Component({
  selector: 'app-error-dialog',
  templateUrl: './error-dialog.component.html',
  styleUrls: ['./error-dialog.component.css'],
  standalone: false
})
export class ErrorDialogComponent implements OnInit, OnDestroy {
  visible: boolean = false;
  errorMessage: string = '';

  private readonly onDestroy$: Subject<void> = new Subject<void>();

  constructor(
    private readonly _errorDialogService: ErrorDialogService,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this._errorDialogService.error$
      .pipe(takeUntil(this.onDestroy$))
      .subscribe((message: string) => {
        if (message) {
          this.errorMessage = message;
          this.visible = true;
          this.cdr.detectChanges(); // 🔥 Key to updating UI if async
        }
      });
  }

  ngOnDestroy(): void {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  closeDialog(): void {
    this.visible = false;
    this.errorMessage = '';
  }
}
