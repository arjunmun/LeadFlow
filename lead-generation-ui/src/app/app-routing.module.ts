import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { LeadListComponent } from './components/lead-list/lead-list.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'leads', component: LeadListComponent },
  { path: '', redirectTo: '/leads', pathMatch: 'full' },
  { path: '**', redirectTo: '/leads' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
