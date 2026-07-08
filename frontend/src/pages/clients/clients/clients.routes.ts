import { Routes } from '@angular/router';
import { ClientsComponent } from './clients';
import { ClientDetail } from './client-detail/client-detail';

export const ClientsRoutes: Routes = [
  {
    path: '',
    component: ClientsComponent,
  },
  {
    path: ':id',
    component: ClientDetail,
  },
];