import { Routes } from '@angular/router';
import { Products } from './products';
import { ProductDetail } from './product-detail/product-detail';

export const ProductsRoutes: Routes = [
  {
    path: '',
    component: Products,
  },
  {
    path: ':id',
    component: ProductDetail,
  },
];