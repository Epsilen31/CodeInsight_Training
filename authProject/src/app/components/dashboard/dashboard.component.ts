import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {
  storedUser = sessionStorage.getItem('user');
  user = this.storedUser ? JSON.parse(this.storedUser).name : undefined;
}
