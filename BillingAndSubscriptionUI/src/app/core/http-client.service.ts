import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpClientService {
  constructor(private readonly _http: HttpClient) {}

  private getHeader(): HttpHeaders {
    const token: string | null = sessionStorage.getItem('token');
    let headers = new HttpHeaders();
    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);
    }
    return headers;
  }

  get<T>(url: string, params?: any): Observable<T> {
    return this._http.get<T>(`${environment.baseurl}/${url}`, {
      headers: this.getHeader(),
      params
    });
  }

  post<T>(url: string, body: any): Observable<T> {
    let headers = this.getHeader();

    if (body instanceof FormData) {
      headers = headers.delete('Content-Type');
    }

    return this._http.post<T>(`${environment.baseurl}/${url}`, body, {
      headers
    });
  }

  put<T>(url: string, body: T): Observable<T> {
    return this._http.put<T>(`${environment.baseurl}/${url}`, body, {
      headers: this.getHeader()
    });
  }

  delete<T>(url: string): Observable<T> {
    return this._http.delete<T>(`${environment.baseurl}/${url}`, {
      headers: this.getHeader()
    });
  }
}
