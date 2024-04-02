import { TestBed } from '@angular/core/testing';

import { ExamDataService } from './exam-data.service';

describe('ExamDataService', () => {
  let service: ExamDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExamDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
