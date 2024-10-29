import { ResolveFn } from '@angular/router';

export const memberDetailedDryrunResolver: ResolveFn<boolean> = (route, state) => {
  return true;
};
