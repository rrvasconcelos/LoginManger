import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { UserCreateRequest, UserCreateResponse } from '../models/user.model';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly API_URL = `${environment.apiUrl}/v1/identity`;

  constructor(private http: HttpClient) {}

  async register(request: UserCreateRequest): Promise<UserCreateResponse> {
    return await firstValueFrom(
      this.http.post<UserCreateResponse>(`${this.API_URL}/register`, request)
    );
  }
}
