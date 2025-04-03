import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BillingSubscriptionComponent } from './billing-subscription.component';

describe('BillingSubscriptionComponent', () => {
  let component: BillingSubscriptionComponent;
  let fixture: ComponentFixture<BillingSubscriptionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BillingSubscriptionComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(BillingSubscriptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
