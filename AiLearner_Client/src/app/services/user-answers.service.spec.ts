import { TestBed } from '@angular/core/testing';

import { UserAnswersService } from './user-answers.service';

describe('UserAnswersService', () => {
  let service: UserAnswersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserAnswersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
