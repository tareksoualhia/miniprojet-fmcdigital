import { NavItem } from './nav-item/nav-item';

export const navItems: NavItem[] = [
  {
    navCap: 'Gestion commerciale',
  },
  {
    displayName: 'Clients',
    iconName: 'solar:users-group-rounded-line-duotone',
    route: '/clients',
  },
  {
    displayName: 'Produits',
    iconName: 'solar:box-line-duotone',
    route: '/products',
  },
  {
    displayName: 'Commandes',
    iconName: 'solar:cart-large-line-duotone',
    route: '/orders',
  },
];