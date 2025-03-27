import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserService } from '../../../services/user.service';
import { IUpdateUser, IUserDetail } from '../../../models/user';
import { ToastrService } from 'ngx-toastr';
import { ThemeService } from '../../../services/theme.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.scss'],
  standalone: false
})
export class UserDetailComponent implements OnInit, OnDestroy {
  userId!: number;
  user?: IUpdateUser;
  isDarkMode: boolean = false;
  private readonly destroyed$ = new Subject<void>();

  constructor(
    private readonly _route: ActivatedRoute,
    private readonly _router: Router,
    private readonly _userService: UserService,
    private readonly _toastr: ToastrService,
    private readonly _themeService: ThemeService
  ) {}

  ngOnInit(): void {
    this._themeService.theme$.pipe(takeUntil(this.destroyed$)).subscribe((isDark: boolean) => {
      this.isDarkMode = isDark;
    });
    this.userId = Number(this._route.snapshot.paramMap.get('id'));
    this.fetchUserDetails();
  }

  fetchUserDetails(): void {
    this._userService.getUserById(this.userId).subscribe({
      next: (data: IUserDetail): void => {
        this.user = data.user;
      },
      error: (error: Error): void => {
        console.error('Error fetching user details:', error);
        this._toastr.error('Error fetching user details:', error.message);
      }
    });
  }

  redirectToAllUser(): void {
    this._router.navigate(['/billing-subscription/user/get-users']);
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
