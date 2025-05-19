import { Routes } from '@angular/router';
import { HOME_ROUTES } from './features/home/home.routers';
import { USER_ROUTER } from './features/Users/user.routers';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';
import {LOGIN_ROUTES} from './features/login/login.routes';

export const routes: Routes = [
  {
    path: '',
    children: HOME_ROUTES,
    pathMatch: 'full',
  },
  {
    path: 'auth/login',
    children: LOGIN_ROUTES
  },
  {
    path: 'users',
    children: USER_ROUTER
  },
  { path: '**', component: PageNotFoundComponent },
];
