import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { AuthService } from '../../codegen/api/auth.service';
import { EventService } from '../../codegen';
import { Router } from '@angular/router';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css'
})
export class LandingComponent {
  loginForm: FormGroup;
  signupForm: FormGroup;

  constructor(private fb: FormBuilder, private authService : AuthService, private eventService : EventService, private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(4)]]
    });

    this.signupForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(4)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.matchingPasswordsValidator });
  }

  onLogin() {
    if (this.loginForm.valid) {
      this.authService.authLoginPost(this.loginForm.value, 'response').subscribe({
        next: (response) => {
          const token = response.body;
          localStorage.setItem('jwtToken', token);
          this.router.navigate(['/calendar']); 
        },
        error: (error) => {
          console.error('Login Error:', error);
        }
      });
    }
  }

  onSignup() {
    if (this.signupForm.valid) {
      this.authService.authRegisterPost(this.signupForm.value).subscribe({
        next: (response) => {
          console.log('Registration Successful:', response);
          this.authService.authLoginPost({
            username: this.signupForm.value.username,
            password: this.signupForm.value.password
          }, 'response').subscribe({
            next: (loginResponse) => {
              const token = loginResponse.body;
              localStorage.setItem('jwtToken', token);
              this.router.navigate(['/calendar']);
            },
            error: (loginError) => {
              console.error('Login Error after Signup:', loginError);
            }
          });
        },
        error: (signupError) => {
          console.error('Signup Error:', signupError);
        }
      });
    }
  }

  matchingPasswordsValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const formGroup = control as FormGroup;
    const password = formGroup.get('password')?.value;
    const confirmPassword = formGroup.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { notMatching: true };
  };
}
