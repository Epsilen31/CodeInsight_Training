import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserWithBillingComponent } from './user-with-billing.component';

describe('UserWithBillingComponent', () => {
  let component: UserWithBillingComponent;
  let fixture: ComponentFixture<UserWithBillingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserWithBillingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserWithBillingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
