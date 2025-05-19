import {CanActivateFn, Router} from '@angular/router';
import {AuthService} from '../services/auth.service';
import {inject} from '@angular/core';
import {catchError, map, of} from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authService = inject(AuthService);

  const returnUrl = state.url;

  return authService.checkAuthStatus().pipe(
    map(result => {
      if (result.isAuthenticated) {
        return true;
      }

      const validReturnUrl = returnUrl && returnUrl !== '/' ? returnUrl : '/';
      console.log('Guard: returnUrl a ser usada:', validReturnUrl);

      router.navigate(['/auth/login'], {
        queryParams: { returnUrl: validReturnUrl },
        queryParamsHandling: 'merge'
      });

      return false;
    }),
    catchError(error => {
      console.error('Guard: Erro ao verificar autenticação:', error);

      const validReturnUrl = returnUrl && returnUrl !== '/' ? returnUrl : '/';

      router.navigate(['/auth/signin'], {
        queryParams: { returnUrl: validReturnUrl },
        queryParamsHandling: 'merge'
      });

      return of(false);
    })
  );
};
