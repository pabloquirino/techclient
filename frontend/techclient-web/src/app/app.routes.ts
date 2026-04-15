import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { ChatComponent } from './features/chat/chat/chat.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'chat', component: ChatComponent },
];