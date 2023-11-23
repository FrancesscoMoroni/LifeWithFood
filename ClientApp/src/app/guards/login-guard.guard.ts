import { CanActivateFn } from '@angular/router';


export const loginGuardGuard: CanActivateFn = (route, state) => {

  var jwt = localStorage.getItem('jwt') || '';

  return jwt == '' ? true : false;
};
