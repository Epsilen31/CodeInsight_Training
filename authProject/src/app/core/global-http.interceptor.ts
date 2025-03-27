import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { LoadingService } from '../services/loading.service';
import { ErrorDialogService } from '../services/error-dialog.service';

@Injectable()
export class GlobalHttpInterceptor implements HttpInterceptor {
  constructor(
    private readonly loadingService: LoadingService,
    private readonly errorDialogService: ErrorDialogService
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.loadingService.loadingOn();

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage: string;
        if (error.error instanceof ErrorEvent) {
          errorMessage = `Client Error: ${error.error.message}`;
        } else {
          errorMessage = `Server Error: ${error.status} - ${error.message}`;
        }
        this.errorDialogService.showError(errorMessage);
        return throwError(() => error);
      }),
      finalize(() => {
        this.loadingService.loadingOff();
      })
    );
  }
}
