import { Component, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';


@Component({
  selector: 'app-user-login',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './user-login.html',
  styleUrl: './user-login.css'
})
export class UserLogin {
  loginForm!: FormGroup;
  showPassword = signal(false);

  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      rememberMe: [false]
    });
  }

  togglePassword(): void {
    this.showPassword.update(v => !v);
  }

  submit(): void {
    if(this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      alert('Please fill all required fields correctly.');
    }
    alert('Login Successful!');
    console.log(this.loginForm.value);
  }
}
