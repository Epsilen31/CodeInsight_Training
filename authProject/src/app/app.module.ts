import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeng/themes/aura';
import { ButtonModule } from 'primeng/button';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { provideHttpClient } from '@angular/common/http';
import { SharedModule } from './shared/shared.module';
import { AuthModule } from './components/auth/auth.module';
import { BillingSubscriptionComponent } from './main/billing-subscription.component';
import { ErrorDialogService } from './services/error-dialog.service';
import { RouterModule } from '@angular/router';
import { LoadingComponent } from './shared/loading/loading.component';
import { NavBarComponent } from './shared/nav-bar/nav-bar.component';
import { UserModule } from './modules/user/user.module';
import { ToastrModule } from 'ngx-toastr';
import { SubscriptionModule } from './modules/subscription/subscription.module';
import { PaymentModule } from './modules/payment/payment.module';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    BillingSubscriptionComponent,
    LoadingComponent,
    NavBarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    SharedModule,
    AuthModule,
    ButtonModule,
    RouterModule,
    UserModule,
    SubscriptionModule,
    PaymentModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 5000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
      progressBar: true,
      progressAnimation: 'increasing',
      closeButton: true,
      maxOpened: 3,
      enableHtml: true
    })
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  providers: [
    provideHttpClient(),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: Aura,
        options: {
          darkModeSelector: '.my-app-dark'
        }
      }
    }),
    ErrorDialogService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
