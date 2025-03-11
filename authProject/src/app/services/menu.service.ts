import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { IMenu, IMenuResponse } from '../models/sidebar';
import { HttpClientService } from '../core/http-client.service';

@Injectable({
  providedIn: 'root',
})
export class MenuService {
  constructor(private readonly _httpClient: HttpClientService) {}

  getAllMenu(): Observable<IMenu[]> {
    return this._httpClient
      .get<IMenuResponse>('Menu/GetSidebarMenu')
      .pipe(map((response: IMenuResponse): IMenu[] => response.menu));
  }
}
