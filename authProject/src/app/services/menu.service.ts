import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { IMenu, IMenuResponse } from '../models/sidebar';
import { HttpClientService } from '../core/http-client.service';
import { RouteKey } from '../shared/constants/routeKey';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  constructor(private readonly _httpClient: HttpClientService) {}

  getAllMenu(): Observable<IMenu[]> {
    return this._httpClient
      .get<IMenuResponse>(`${RouteKey.GET_ALL_MENU_ITEMS_URL}`)
      .pipe(map((response: IMenuResponse): IMenu[] => response.menu));
  }
}
