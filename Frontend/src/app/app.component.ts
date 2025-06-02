import { Component } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MenubarModule } from 'primeng/menubar';
import { ButtonModule } from 'primeng/button';
import { ToolbarModule } from 'primeng/toolbar';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet, 
    CommonModule,
    MenubarModule,
    ButtonModule,
    ToolbarModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Sistema de Gestión';
  
  menuItems: MenuItem[] = [];

  constructor(private router: Router) {
    this.initializeMenu();
  }

  initializeMenu() {
    this.menuItems = [
      {
        label: 'Productos',
        icon: 'pi pi-shopping-cart',
        items: [
          {
            label: 'Lista de Productos',
            icon: 'pi pi-list',
            command: () => this.navigateTo('/productos')
          },
          {
            label: 'Nuevo Producto',
            icon: 'pi pi-plus',
            command: () => this.navigateTo('/producto/nuevo')
          }
        ]
      },
      {
        label: 'Transacciones',
        icon: 'pi pi-money-bill',
        items: [
          {
            label: 'Lista de Transacciones',
            icon: 'pi pi-list',
            command: () => this.navigateTo('/transacciones')
          },
          {
            label: 'Nueva Transacción',
            icon: 'pi pi-plus',
            command: () => this.navigateTo('/transaccion/nueva')
          }
        ]
      }
    ];
  }

  navigateTo(route: string) {
    this.router.navigate([route]);
  }
}