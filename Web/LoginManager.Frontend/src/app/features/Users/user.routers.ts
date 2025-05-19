import { Routes } from '@angular/router';
import {authGuard} from '../../core/guards/auth.guard';

export const USER_ROUTER: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./components/user-list/user-list.component').then(
        (m) => m.UserListComponent
      ),
    title: 'Register',
    canActivate: [authGuard]
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./components/user-register/user-register.component').then(
        (m) => m.UserRegisterComponent
      ),
    title: 'Register',
  },
];
