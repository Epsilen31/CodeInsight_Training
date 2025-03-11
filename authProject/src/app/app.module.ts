import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeng/themes/aura';
import { ButtonModule } from 'primeng/button';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NavBarComponent } from './shared/nav-bar/nav-bar.component';
import { provideHttpClient } from '@angular/common/http';
import { SharedModule } from './shared/shared.module';
import { AuthModule } from './components/auth/auth.module';
import { BillingComponent } from './components/billing/billing.component';
import { SubscriptionComponent } from './components/subscription/subscription.component';
import { PaymentsComponent } from './components/payments/payments.component';
import { UserComponent } from './components/user/user.component';
import { BillingSubscriptionComponent } from './main/billing-subscription.component';
import { ErrorDialogService } from './services/error-dialog.service';
import { RouterModule } from '@angular/router';
import { LoadingComponent } from './shared/loading/loading.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    NavBarComponent,
    BillingComponent,
    SubscriptionComponent,
    PaymentsComponent,
    UserComponent,
    BillingSubscriptionComponent,
    LoadingComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    AuthModule,
    ButtonModule,
    RouterModule,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [
    provideHttpClient(),
    provideAnimationsAsync(),
    providePrimeNG({ theme: { preset: Aura } }),
    ErrorDialogService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
