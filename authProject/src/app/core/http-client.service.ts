import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, finalize, Observable, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { ErrorDialogService } from './../services/error-dialog.service';
import { LoadingService } from './../services/loading.service';
import { Params } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class HttpClientService {
  constructor(
    private readonly _http: HttpClient,
    private readonly _errorService: ErrorDialogService,
    private readonly _loadingService: LoadingService
  ) {}

  private getHeader(): HttpHeaders {
    const token: string | null = sessionStorage.getItem('token');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return headers;
  }
  get<T>(url: string, params?: Params): Observable<T> {
    this._loadingService.loadingOn();
    // this is till development period for checking the loading in working fine or not
    return new Observable<T>((observer) => {
      setTimeout((): void => {
        this._http
          .get<T>(`${environment.baseurl}/${url}`, {
            headers: this.getHeader(),
            params,
          })
          .pipe(
            finalize((): void => {
              this._loadingService.loadingOff(); //Stop Loading Spinner
            }),
            catchError((error) => {
              this._loadingService.loadingOff();
              return this.handleError(error);
            })
          )
          .subscribe({
            next: (response) => {
              observer.next(response);
              observer.complete();
            },
            error: (error: HttpErrorResponse): void => observer.error(error),
          });
      }, 3000);
    });
  }

  post<T>(url: string, body: T): Observable<T> {
    this._loadingService.loadingOn();
    return this._http
      .post<T>(`${environment.baseurl}/${url}`, body, {
        headers: this.getHeader(),
      })
      .pipe(
        finalize(() => this._loadingService.loadingOff()),
        catchError(this.handleError.bind(this))
      );
  }

  put<T>(url: string, body: T): Observable<T> {
    this._loadingService.loadingOn();
    return this._http
      .put<T>(`${environment.baseurl}/${url}`, body, {
        headers: this.getHeader(),
      })
      .pipe(
        finalize(() => this._loadingService.loadingOff()),
        catchError(this.handleError.bind(this))
      );
  }

  delete<T>(url: string): Observable<T> {
    this._loadingService.loadingOn();
    return this._http
      .delete<T>(`${environment.baseurl}/${url}`, {
        headers: this.getHeader(),
      })
      .pipe(
        finalize(() => this._loadingService.loadingOff()),
        catchError(this.handleError.bind(this))
      );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage: string = 'Something went wrong!';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Client Error: ${error.error.message}`;
    } else {
      errorMessage = `Server Error: ${error.status} - ${error.message}`;
    }
    this._errorService.showError(errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
