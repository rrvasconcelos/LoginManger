import {HttpErrorResponse, HttpInterceptorFn} from '@angular/common/http';
import {inject} from '@angular/core';
import {Router} from '@angular/router';
import {catchError, throwError} from 'rxjs';
import {AuthService} from '../services/auth.service';

export const authInInterceptor: HttpInterceptorFn = (req, next) => {
  console.log('Auth In Interceptor');

  const router: Router = inject(Router);
  const authService: AuthService = inject(AuthService);

  const currentUrl = router.routerState.snapshot.url;

  console.log('currentUrl', currentUrl);

  const isAuthRequest = (
    req.url.includes('/identity/signin') ||
    req.url.includes('/identity/register') ||
    req.url.includes('/identity/logout') ||
    req.url.includes('/identity/verify')
  );

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !isAuthRequest) {
        const validReturnUrl = currentUrl && currentUrl !== '/' && !currentUrl.startsWith('/auth/login')
          ? currentUrl
          : '/';

        authService.updateAuthStatus(false);

        router.navigate(['/auth/login'], {
          queryParams: {returnUrl: validReturnUrl}
        });
      }
      return throwError(() => error);
    })
  );
};
