import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-user-register',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './user-register.html',
  styleUrl: './user-register.css'
})
export class UserRegister {
  registrationForm !: FormGroup;

  constructor(private fb: FormBuilder) {
    this.registrationForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      gender: ['', Validators.required],
      address: [''],
      emergencyContactName: [''],
      emergencyContactPhone: [''],
      medicalHistory: [''],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
      agreeToTerms: [false, Validators.requiredTrue]
    }, { validators: this.passwordsMatchValidator });
  }

  passwordsMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordsMismatch: true };
  }

  onSubmit() {
    if(this.registrationForm.valid) {
      console.log(this.registrationForm.value);
      alert('Registration Successful!');
    }
    else {
      alert('Please fill all required fields correctly.');
    }
  }
}
