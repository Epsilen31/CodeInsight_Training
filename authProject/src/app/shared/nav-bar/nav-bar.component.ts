import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  standalone: false,
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.scss',
})
export class NavBarComponent {
  storedUser = sessionStorage.getItem('user');
  user = this.storedUser ? JSON.parse(this.storedUser).name : 'Guest';

  constructor(private router: Router) {}

  logout(): void {
    sessionStorage.removeItem('user');
    sessionStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
