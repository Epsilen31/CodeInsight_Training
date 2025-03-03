import { TestBed } from '@angular/core/testing';

import { AuthHubService } from './auth-hub.service';

describe('AuthHubService', () => {
  let service: AuthHubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthHubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
