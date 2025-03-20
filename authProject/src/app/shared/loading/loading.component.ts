import {
  Component,
  ContentChild,
  Input,
  OnInit,
  OnDestroy,
  TemplateRef,
} from '@angular/core';
import { LoadingService } from '../../services/loading.service';
import { Subscription, tap } from 'rxjs';
import {
  Event,
  RouteConfigLoadEnd,
  RouteConfigLoadStart,
  Router,
} from '@angular/router';

@Component({
  selector: 'app-loading',
  standalone: false,
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss'],
})
export class LoadingComponent implements OnInit, OnDestroy {
  isLoading: boolean = false;
  private loadingSubscription!: Subscription;

  @Input() detectRouteTransitions: boolean = false;

  @ContentChild('loading') customLoadingIndicator: TemplateRef<{
    message: string;
  }> | null = null;

  constructor(
    private readonly _loadingService: LoadingService,
    private readonly _router: Router
  ) {}

  ngOnInit(): void {
    this.loadingSubscription = this._loadingService.loading$.subscribe(
      (state: boolean): void => {
        this.isLoading = state;
      }
    );

    if (this.detectRouteTransitions) {
      this._router.events
        .pipe(
          tap((event: Event): void => {
            if (event instanceof RouteConfigLoadStart) {
              this._loadingService.loadingOn();
            } else if (event instanceof RouteConfigLoadEnd) {
              this._loadingService.loadingOff();
            }
          })
        )
        .subscribe();
    }
  }

  ngOnDestroy(): void {
    if (this.loadingSubscription) {
      this.loadingSubscription.unsubscribe();
    }
  }
}
