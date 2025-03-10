import { Component } from '@angular/core';
import { LoadingService } from '../../../services/loading.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-loading',
  standalone: false,
  templateUrl: './loading.component.html',
  styleUrl: './loading.component.scss',
})
export class LoadingComponent {
  loading$: Observable<boolean>;

  constructor(private readonly _loadingService: LoadingService) {
    this.loading$ = this._loadingService.loading$;
  }
}
