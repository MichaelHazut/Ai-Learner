import { Routes } from '@angular/router';

import { HomePageComponent } from './pages/home-page/home-page.component';
import { SignupPageComponent } from './pages/signup-page/signup-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { StudyHubComponent } from './pages/study-hub/study-hub.component';
import { MaterialSelectionComponent } from './components/study-hub/material-selection/material-selection.component';
import { NewMaterialComponent } from './components/study-hub/new-material/new-material.component';
import { MaterialsScreenComponent } from './components/study-hub/materials-screen/materials-screen.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { MaterialComponent } from './components/study-hub/material/material.component';
import { ExamComponent } from './components/exam/exam.component';
import { ExamResultComponent } from './components/exam/exam-result/exam-result.component';
import { ExamRetryComponent } from './components/exam/exam-retry/exam-retry.component';
import { authGuard } from './auth/auth-guard';
import { LogoutComponent } from './components/logout/logout.component';
import { AboutPageComponent } from './pages/About-page/about-page/about-page.component';
import { PrivacyPolicyPageComponent } from './pages/privacy-policy-page/privacy-policy-page.component';

export const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'home', component: HomePageComponent },
  { path: 'signup', component: SignupPageComponent },
  { path: 'login', component: LoginPageComponent },
  { path: 'about', component: AboutPageComponent },
  { path: 'privacy-policy', component: PrivacyPolicyPageComponent },
  { path: 'signout', component: LogoutComponent, canActivate: [authGuard]},
  {
    path: 'study-hub',
    component: StudyHubComponent,
    canActivate: [authGuard],
    children: [
      { path: '', component: MaterialSelectionComponent },
      { path: 'new-material', component: NewMaterialComponent },
      { path: 'materials', component: MaterialsScreenComponent },
      { path: 'materials/:id', component: MaterialComponent },
      { path: 'materials/:id/exam', component: MaterialsScreenComponent },
      { path: 'materials/:id/exam/:questionIndex', component: ExamComponent },
      { path: 'materials/:id/result', component: ExamResultComponent },
      { path: 'materials/:id/exam/retry/:questionIndex', component: ExamRetryComponent },
    ],
  },
  { path: '**', component: PageNotFoundComponent },
];
