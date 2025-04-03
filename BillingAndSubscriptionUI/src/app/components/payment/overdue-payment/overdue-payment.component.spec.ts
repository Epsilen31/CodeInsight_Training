import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OverduePaymentComponent } from './overdue-payment.component';

describe('OverduePaymentComponent', () => {
  let component: OverduePaymentComponent;
  let fixture: ComponentFixture<OverduePaymentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [OverduePaymentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OverduePaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
