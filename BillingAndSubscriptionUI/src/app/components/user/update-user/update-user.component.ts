import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserService } from '../../../services/user.service';
import { IUpdateUser, IUserDetail } from '../../../models/user';
import { IErrorResponse } from '../../../models/error';
import { ToastService } from '../../../services/toast.service';
import { ThemeService } from '../../../services/theme.service';
import { RedirectKey } from '../../../shared/constants/redirectionKey';

@Component({
  selector: 'app-update-user',
  standalone: false,
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.scss']
})
export class UpdateUserComponent implements OnInit, OnDestroy {
  userId!: number;
  user!: IUpdateUser;
  isDarkMode: boolean = false;
  private themeSubscription!: Subscription;

  constructor(
    private readonly _route: ActivatedRoute,
    private readonly _router: Router,
    private readonly _userService: UserService,
    private readonly _toastService: ToastService,
    private readonly _themeService: ThemeService
  ) {}

  ngOnInit(): void {
    this.themeSubscription = this._themeService.theme$.subscribe((isDark: boolean) => {
      this.isDarkMode = isDark;
    });

    this.userId = Number(this._route.snapshot.paramMap.get('id'));
    this.getUserDetails();
  }

  ngOnDestroy(): void {
    if (this.themeSubscription) {
      this.themeSubscription.unsubscribe();
    }
  }

  getUserDetails(): void {
    this._userService.getUserById(this.userId).subscribe({
      next: (data: IUserDetail): void => {
        this.user = data.user;
      },
      error: (error: IErrorResponse): void => {
        this._toastService.showError(`Error fetching user: ${error.message}`);
      }
    });
  }

  updateUser(): void {
    const { role, ...rest } = this.user;
    const userToUpdate = { ...rest };
    this._userService.updateUser(this.userId, userToUpdate).subscribe({
      next: (): void => {
        this._toastService.showSuccess('Updated user successfully! redirecting to the user page');
      },
      error: (error: IErrorResponse): void => {
        this._toastService.showError(`Error updating user: ${error.message}`);
      }
    });
  }

  cancel(): void {
    this._router.navigate([`${RedirectKey.REDIRECT_TO_ALL_USER_DETAILS}`]);
  }
}
