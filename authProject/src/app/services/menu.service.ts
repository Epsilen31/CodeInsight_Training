import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IMenu, IMenuResponse } from '../models/sidebar';

@Injectable({
  providedIn: 'root',
})
export class MenuService {
  constructor(private readonly _http: HttpClient) {}

  getAllMenu(): Observable<IMenu[]> {
    const token: string | null = sessionStorage.getItem('token');

    if (!token) {
      console.error('No authentication token found!');
      return new Observable<IMenu[]>();
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });

    return this._http
      .get<IMenuResponse>(`${environment.baseurl}/Menu/GetSidebarMenu`, {
        headers,
      })
      .pipe(map((response: IMenuResponse): IMenu[] => response.menu));
  }
}
