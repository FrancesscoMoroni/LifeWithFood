import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { UserAuthService } from '../services/user-auth.service';

export const adminGuardGuard: CanActivateFn = (route, state) => {
  var userAuthService = inject(UserAuthService);

  var result = userAuthService.userAuthorization("2");

  return result;
};
