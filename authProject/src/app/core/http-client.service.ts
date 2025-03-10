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
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  }

  get<T>(url: string, params?: any): Observable<T> {
    this._loadingService.show();
    return this._http
      .get<T>(`${environment.baseurl}/${url}`, {
        headers: this.getHeader(),
        params,
      })
      .pipe(
        finalize(() => this._loadingService.hide()), // Stop loading after request completes
        catchError(this.handleError.bind(this))
      );
  }

  post<T>(url: string, body: T): Observable<T> {
    this._loadingService.show();
    return this._http
      .post<T>(`${environment.baseurl}/${url}`, body, {
        headers: this.getHeader(),
      })
      .pipe(
        finalize(() => this._loadingService.hide()),
        catchError(this.handleError.bind(this))
      );
  }

  put<T>(url: string, body: T): Observable<T> {
    this._loadingService.show();
    return this._http
      .put<T>(`${environment.baseurl}/${url}`, body, {
        headers: this.getHeader(),
      })
      .pipe(
        finalize(() => this._loadingService.hide()),
        catchError(this.handleError.bind(this))
      );
  }

  delete<T>(url: string): Observable<T> {
    this._loadingService.show();
    return this._http
      .delete<T>(`${environment.baseurl}/${url}`, {
        headers: this.getHeader(),
      })
      .pipe(
        finalize(() => this._loadingService.hide()),
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
    console.log('errorMessage', errorMessage);
    this._errorService.showError(errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
