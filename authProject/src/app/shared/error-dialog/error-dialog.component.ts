import { Component, OnInit } from '@angular/core';
import { ErrorDialogService } from '../../services/error-dialog.service';

@Component({
  selector: 'app-error-dialog',
  templateUrl: './error-dialog.component.html',
  styleUrl: './error-dialog.component.scss',
  standalone: false
})
export class ErrorDialogComponent implements OnInit {
  visible: boolean = false;
  errorMessage: string = '';

  constructor(private readonly _errorService: ErrorDialogService) {}

  ngOnInit() {
    this._errorService.error$.subscribe((message: string): void => {
      if (message) {
        this.errorMessage = message;
        this.visible = true;
      }
    });
  }

  closeDialog(): void {
    this.visible = false;
  }
}
