import { Routes } from '@angular/router';
import { Orders } from './orders';
import { OrderDetail } from './order-detail/order-detail';

export const OrdersRoutes: Routes = [
  {
    path: '',
    component: Orders,
  },
  {
    path: ':id',
    component: OrderDetail,
  },
];