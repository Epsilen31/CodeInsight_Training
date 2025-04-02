import { Injectable } from '@angular/core';
import { HttpClientService } from '../core/http-client.service';
import { Observable } from 'rxjs';
import { RouteKey } from '../shared/constants/routeKey';
import { IDashboardResponseData } from '../models/dashboard';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  constructor(private readonly _httpClientService: HttpClientService) {}

  getDashboardData(): Observable<IDashboardResponseData> {
    return this._httpClientService.get(`${RouteKey.GET_ANALYTICS_INFO_URL}`);
  }
}
