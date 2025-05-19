import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { provideRouter } from '@angular/router';
import { provideToastr } from 'ngx-toastr';

import { routes } from './app.routes';
import {authInInterceptor} from './core/interceptors/auth-in.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideAnimationsAsync(),
    provideToastr(),
    provideHttpClient(withInterceptors([authInInterceptor])),
    provideRouter(routes)
  ]
};
