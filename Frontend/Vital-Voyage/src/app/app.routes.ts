import { Routes } from '@angular/router';
import { UserLogin } from './login/user-login/user-login';
import { UserRegister } from './register/user-register/user-register';

export const routes: Routes = [
    {path: 'login', component: UserLogin},
    {path: 'register', component: UserRegister},
    {path: '', redirectTo:'login', pathMatch: 'full'},
    {path: '**', redirectTo:'login', pathMatch: 'full'}
];
