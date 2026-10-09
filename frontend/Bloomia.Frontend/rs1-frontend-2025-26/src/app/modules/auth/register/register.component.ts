import { Component, inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';

import { AuthApiService } from '../../../api-services/auth/auth-api.service';
import {
  RegistrationType,
  TherapistRegisterCommand,
  UserRegisterCommand
} from '../../../api-services/auth/auth-api.model';

import { ToasterService } from '../../../core/services/toaster.service';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnInit {

  private fb = inject(FormBuilder);
  private authApi = inject(AuthApiService);
  private router = inject(Router);
  private toaster = inject(ToasterService);

  isLoading = false;
  errorMessage = '';

  accountTypeForm = this.fb.group({
    accountType: ['CLIENT' as RegistrationType, Validators.required]
  });

  accountForm = this.fb.group({
    email: ['',
      [
        Validators.required,
        Validators.email,
        Validators.pattern(/@(gmail\.com|yahoo\.com|edu\.com)$/i)
      ]
    ],

    username: ['',
      [
        Validators.required,
        Validators.maxLength(30)
      ]
    ],

    password: ['',
      [
        Validators.required,
        Validators.minLength(6),
        Validators.pattern(/^(?=.*[A-Z])(?=.*[0-9]).+$/)
      ]
    ]
  });

  personalForm = this.fb.group({
    firstname: ['', [ Validators.required,  Validators.maxLength(50)]],
    lastname: ['', [Validators.required, Validators.maxLength(50)]],
    specialization: [''],
    description: ['',  Validators.maxLength(500)]
  });

  ngOnInit(): void {
    this.updateRoleValidators();

    this.accountTypeForm.get('accountType')?.valueChanges
      .subscribe(() => {
        this.updateRoleValidators();
      });
  }

  get accountType(): RegistrationType {
    return (this.accountTypeForm.get('accountType')?.value ?? 'CLIENT') as RegistrationType;
  }

  get isTherapist(): boolean {
    return this.accountType === 'THERAPIST';
  }

  register(): void {
    if (
      this.accountTypeForm.invalid ||
      this.accountForm.invalid ||
      this.personalForm.invalid
    ) {
      this.accountTypeForm.markAllAsTouched();
      this.accountForm.markAllAsTouched();
      this.personalForm.markAllAsTouched();

      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    if (this.isTherapist) {
      this.registerTherapist();
    } else {
      this.registerClient();
    }
  }

  private registerClient(): void {
    const account = this.accountForm.getRawValue();
    const personal = this.personalForm.getRawValue();

    const command: UserRegisterCommand = {
      email: account.email!,
      password: account.password!,
      username: account.username!,
      firstname: personal.firstname!,
      lastname: personal.lastname!,

      genderName: '',
      locationCityName: '',
      locationCountryName: '',
      languageName: '',
      dateOfBirth: null
    };

    this.authApi.registerClient(command).subscribe({
      next: () => {
        this.isLoading = false;

        this.toaster.success('Registration successful. You can now log in.');

        this.router.navigate(['/auth/login']);
      },

      error: (err) => {
        this.isLoading = false;

        this.errorMessage = 'Registration failed. Please check the entered information.';

        console.error('Client registration error:', err);
      }
    });
  }

  private registerTherapist(): void {
    const account = this.accountForm.getRawValue();
    const personal = this.personalForm.getRawValue();

    const command: TherapistRegisterCommand = {
      email: account.email!,
      password: account.password!,
      username: account.username!,
      firstname: personal.firstname!,
      lastname: personal.lastname!,
      specialization: personal.specialization!,
      description: personal.description ?? ''
    };

    this.authApi.registerTherapist(command).subscribe({
      next: () => {
        this.isLoading = false;

        this.toaster.success('Registration successful. You can now log in.');

        this.router.navigate(['/auth/login']);
      },

      error: (err) => {
        this.isLoading = false;

        this.errorMessage =
          'Registration failed. Please check the entered information.';

        console.error(
          'Therapist registration error:',
          err
        );
      }
    });
  }

  private updateRoleValidators(): void {
    const specialization = this.personalForm.get('specialization');

    if (this.isTherapist) {
      specialization?.setValidators([
        Validators.required,
        Validators.maxLength(100)
      ]);
    } else {
      specialization?.clearValidators();

      specialization?.setValue('');
      this.personalForm.get('description')?.setValue('');
    }

    specialization?.updateValueAndValidity();
  }

  getAccountError(controlName: 'email' | 'username' | 'password'): string {

    const control = this.accountForm.get(controlName);

    if (!control) {
      return '';
    }

    if (control.hasError('required')) {
      return 'This field is required.';
    }

    if (controlName === 'email' && control.hasError('email')) {
      return 'Enter a valid email address.';
    }

    if (controlName === 'email' && control.hasError('pattern')) {
      return 'Email domain must be gmail, yahoo or edu.';
    }

    if (control.hasError('maxlength')) {
      return 'Value is too long.';
    }

    if (control.hasError('minlength')) {
      return 'Password must contain at least 6 characters.';
    }

    if (control.hasError('pattern')) {
      return 'Password must contain an uppercase letter and a number.';
    }

    return '';
  }

  getPersonalError(controlName: | 'firstname' | 'lastname' | 'specialization' | 'description'): string {

    const control = this.personalForm.get(controlName);

    if (!control) {
      return '';
    }

    if (control.hasError('required')) {
      return 'This field is required.';
    }

    if (control.hasError('maxlength')) {
      return 'Value is too long.';
    }

    return '';
  }
}