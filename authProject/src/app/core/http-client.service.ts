import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class HttpClientService {
  constructor(private readonly _http: HttpClient) {}

  private getHeader(): HttpHeaders {
    const token: string | null = sessionStorage.getItem('token');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
    return headers;
  }

  get<T>(url: string, params?: any): Observable<T> {
    return this._http
      .get<T>(`${environment.baseurl}/${url}`, {
        headers: this.getHeader(),
        params,
      })
      .pipe(catchError(this.handleError));
  }

  post<T>(url: string, body: T): Observable<T> {
    return this._http
      .post<T>(`${environment.baseurl}/${url}`, body, {
        headers: this.getHeader(),
      })
      .pipe(catchError(this.handleError));
  }

  put<T>(url: string, body: T): Observable<T> {
    return this._http
      .put<T>(`${environment.baseurl}/${url}`, body, {
        headers: this.getHeader(),
      })
      .pipe(catchError(this.handleError));
  }

  delete<T>(url: string): Observable<T> {
    return this._http
      .delete<T>(`${environment.baseurl}/${url}`, {
        headers: this.getHeader(),
      })
      .pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    return throwError((): Error => new Error('Something wrong happened'));
  }
}
