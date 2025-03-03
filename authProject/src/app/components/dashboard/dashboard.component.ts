import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {
  storedUser: string | null = sessionStorage.getItem('user');
  user: string = this.storedUser ? JSON.parse(this.storedUser).name : undefined;
}
