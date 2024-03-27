import { Routes } from '@angular/router';

import { HomePageComponent } from './pages/home-page/home-page.component';
import { SignupPageComponent } from './pages/signup-page/signup-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { TestPageComponent } from './pages/test-page/test-page.component';
import { StudyHubComponent } from './pages/study-hub/study-hub.component';
import { MaterialSelectionComponent } from './components/study-hub/material-selection/material-selection.component';
import { NewMaterialComponent } from './components/study-hub/new-material/new-material.component';
import { MaterialsScreenComponent } from './components/study-hub/materials-screen/materials-screen.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';


export const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'home', component: HomePageComponent },
  { path: 'signup', component: SignupPageComponent },
  { path: 'login', component: LoginPageComponent },
  {
    path: 'study-hub',
    component: StudyHubComponent,
    children: [
      { path: '', component: MaterialSelectionComponent },
      { path: 'new-material', component: NewMaterialComponent },
      { path: 'materials', component: MaterialsScreenComponent },
    ],
  },
  { path: 'test', component: TestPageComponent },
  { path: '**', component: PageNotFoundComponent },
];
