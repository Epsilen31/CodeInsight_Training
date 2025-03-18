import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { IUser } from '../../../models/user';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-detail',
  standalone: false,
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.scss'],
})
export class UserDetailComponent implements OnInit {
  userId!: number;
  user?: IUser;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly userService: UserService,
    private readonly toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.userId = Number(this.route.snapshot.paramMap.get('id'));
    this.fetchUserDetails();
  }

  fetchUserDetails(): void {
    this.userService.getUserById(this.userId).subscribe({
      next: (data: any) => {
        this.user = data.user;
        console.log('User details:', this.user);
      },
      error: (error) => {
        console.error('Error fetching user details:', error);
        this.toastr.error('Error fetching users:', error.message);
      },
    });
  }

  goBack(): void {
    this.router.navigate(['/billing-subscription/user/get-users']);
  }
}
