import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

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
import { BillingSubscriptionComponent } from './billing-subscription.component';

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
    BillingSubscriptionComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    AuthModule,
  ],
  providers: [provideHttpClient()],
  bootstrap: [AppComponent],
})
export class AppModule {}
