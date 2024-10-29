import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { memberDetailedDryrunResolver } from './member-detailed---dryrun.resolver';

describe('memberDetailedDryrunResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => memberDetailedDryrunResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
