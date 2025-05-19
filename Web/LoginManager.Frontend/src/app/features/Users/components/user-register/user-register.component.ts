import { Component, inject, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { CpfValidator } from '../../../../shared/directives/cpf-validator.directive';
import { UserCreateRequest } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-user-register',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    NgxSpinnerModule,
  ],
  templateUrl: './user-register.component.html',
  styleUrl: './user-register.component.scss',
})
export class UserRegisterComponent implements OnInit {
  fb: FormBuilder = inject(FormBuilder);
  userService: UserService = inject(UserService);
  toastr: ToastrService = inject(ToastrService);
  spinner: NgxSpinnerService = inject(NgxSpinnerService);

  hidePassword = true;
  hideConfirmPassword = true;

  form!: FormGroup;

  ngOnInit(): void {
    this.form = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        document: ['', [Validators.required, CpfValidator]],
        password: ['', [Validators.required, Validators.maxLength(50)]],
        confirmPassword: ['', [Validators.required]],
      },
      { validators: passwordMatchValidator }
    );
  }

  async onSubmit() {
    if (!this.form.valid) return;

    const request: UserCreateRequest = this.form.value;

    try {
      this.spinner.show();

      await this.userService.register(request);

      this.spinner.hide();
      this.toastr.success(
        'Cadastro realizado com sucesso!',
        'Usuário Cadastrado'
      );
    } catch (error: any) {
      this.spinner.hide();

      if (error.error.status === 400)
        this.toastr.error(error.error.detail, 'Validação');
      else this.toastr.error(error.error.detail, 'Ocorreu um erro');
    }
  }

  get email(): FormControl {
    return this.form.get('email') as FormControl;
  }

  get document(): FormControl {
    return this.form.get('document') as FormControl;
  }

  get password(): FormControl {
    return this.form.get('password') as FormControl;
  }

  get confirmPassword(): FormControl {
    return this.form.get('confirmPassword') as FormControl;
  }

  get passwordMatch(): boolean {
    return this.form.hasError('passwordMismatch');
  }
}

export const passwordMatchValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const password = control.get('password');
  const confirmPassword = control.get('confirmPassword');

  if (
    password?.value &&
    confirmPassword?.value &&
    password.value !== confirmPassword.value
  ) {
    confirmPassword?.setErrors({ passwordMismatch: true });
    return { passwordMismatch: true };
  }

  if (confirmPassword?.hasError('passwordMismatch')) {
    const errors = { ...confirmPassword.errors };
    delete errors['passwordMismatch'];

    confirmPassword.setErrors(Object.keys(errors).length ? errors : null);
  }

  return null;
};
