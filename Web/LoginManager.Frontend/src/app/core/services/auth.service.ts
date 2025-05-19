import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import {catchError, Observable, of, tap, throwError} from 'rxjs';
import {CheckAuthStatusResponse} from '../models/check-auth-status-response';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private IsLogged = signal<boolean>(false);
  public IsLoggedIn = this.IsLogged.asReadonly();

  private readonly API_URL = `${environment.apiUrl}/v1/identity`;

  constructor(private http: HttpClient) {}

  //login

  //logout

  public checkAuthStatus(): Observable<CheckAuthStatusResponse> {
    return this.http.get<CheckAuthStatusResponse>(`${this.API_URL}/verify`, {withCredentials: true})
      .pipe(
        tap(response => {
          this.IsLogged.set(response.isAuthenticated);
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 401 || error.status === 403) {
            this.IsLogged.set(false);
            return of({
              isAuthenticated: false
            } as CheckAuthStatusResponse);
          }
          return throwError(() => error);
        })
      );
  }

  updateAuthStatus(isLoggedIn: boolean) {
    this.IsLogged.update(isLoggedIn => isLoggedIn);
  }
}
