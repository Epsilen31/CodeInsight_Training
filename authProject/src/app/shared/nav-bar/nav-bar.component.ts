import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  standalone: false,
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.scss',
})
export class NavBarComponent {
  userName = localStorage.getItem('user') || 'Guest';

  constructor(private router: Router) {}

  logout() {
    localStorage.removeItem('user');
    this.router.navigate(['/login']);
  }
}
