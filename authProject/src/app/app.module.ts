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

@NgModule({
  declarations: [AppComponent, DashboardComponent, NavBarComponent],
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
