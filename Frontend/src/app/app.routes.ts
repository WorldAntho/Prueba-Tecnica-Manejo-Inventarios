import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'productos',
    loadComponent: () => import('./modules/products/pages/ListaProductos/productos-list.component').then(m => m.ProductosListComponent)
  },
  {
    path: 'productos/nuevo',
    loadComponent: () => import('./modules/products/components/producto-form.component').then(m => m.ProductoFormComponent),
  },
  {
    path: 'productos/editar/:id',
    loadComponent: () => import('./modules/products/components/producto-form.component').then(m => m.ProductoFormComponent),
  },
  {
    path: 'transacciones',
    loadComponent: () => import('./modules/transactions/pages/ListaTransacciones/transacciones-list.component').then(m => m.TransaccionesListComponent)
  },
  {
    path: 'transacciones/nueva',
    loadComponent: () => import('./modules/transactions/components/transaccion-form.component').then(m => m.TransaccionFormComponent),
  },
  {
    path: 'transacciones/editar/:id',
    loadComponent: () => import('./modules/transactions/components/transaccion-form.component').then(m => m.TransaccionFormComponent),
  },
  {
    path: '',
    redirectTo: '/productos',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: '/productos'
  }
];
