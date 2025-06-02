import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService, ConfirmationService } from 'primeng/api';
import { ProductosDTO, CategoriasDTO } from '../../../../core/models/Entidades';
import { CategoriasService } from '../../../../core/services/Categorias.service';
import { ProductosService } from '../../../../core/services/Productos.service';
import { ImportsModule } from '../../../../core/models/Imports';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DropdownModule } from 'primeng/dropdown';

@Component({
  selector: 'app-productos-list',
  imports: [
    CommonModule,
    FormsModule,
    DropdownModule,
    ImportsModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './productos-list.component.html',
  styleUrls: ['./productos-list.component.css']
})
export class ProductosListComponent implements OnInit {
  productos: ProductosDTO[] = [];
  categorias: CategoriasDTO[] = [];
  loading = false;
  totalRecords = 0;
  first = 0;
  rows = 10;

  // Filtros
  filters = {
    nombre: '',
    idCategoria: null,
    activo: null,
    precioMin: null,
    precioMax: null
  };

  // Opciones para filtros
  activeOptions = [
    { label: 'Todos', value: null },
    { label: 'Activos', value: true },
    { label: 'Inactivos', value: false }
  ];

  constructor(
    private productosService: ProductosService,
    private categoriasService: CategoriasService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadCategorias();
    this.loadProductos();
  }

  loadCategorias() {
    this.categoriasService.getCategorias().subscribe({
      next: (response) => {
        if (response.data.isSuccess) {
          this.categorias = response.data.data;
        }
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Error al cargar categorías'
        });
      }
    });
  }

  loadProductos(event?: any) {
    this.loading = true;
    
    const page = event ? Math.floor(event.first / event.rows) + 1 : 1;
    const size = event ? event.rows : this.rows;

    this.productosService.getProducts(page, size, this.filters).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.data.isSuccess) {
          this.productos = response.data.data.data;
          this.totalRecords = response.data.data.totalRecords;
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: response.data.message || 'Error al cargar productos'
          });
        }
      },
      error: (error) => {
        this.loading = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Error al cargar productos'
        });
      }
    });
  }

  applyFilters() {
    this.first = 0;
    this.loadProductos();
  }

  clearFilters() {
    this.filters = {
      nombre: '',
      idCategoria: null,
      activo: null,
      precioMin: null,
      precioMax: null
    };
    this.first = 0;
    this.loadProductos();
  }

  createProduct() {
    this.router.navigate(['/productos/crear']);
  }

  editProduct(producto: ProductosDTO) {
    this.router.navigate(['/productos/editar', producto.idProducto]);
  }

  deleteProduct(producto: ProductosDTO) {
    this.confirmationService.confirm({
      message: `¿Está seguro de eliminar el producto "${producto.nombre}"?`,
      header: 'Confirmar Eliminación',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.productosService.deleteProduct(producto.idProducto!).subscribe({
          next: (response) => {
            if (response.data.isSuccess) {
              this.messageService.add({
                severity: 'success',
                summary: 'Éxito',
                detail: 'Producto eliminado correctamente'
              });
              this.loadProductos();
            } else {
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: response.data.message || 'Error al eliminar producto'
              });
            }
          },
          error: (error) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'Error al eliminar producto'
            });
          }
        });
      }
    });
  }

  getCategoriaName(idCategoria: number): string {
    const categoria = this.categorias.find(c => c.idCategoria === idCategoria);
    return categoria ? categoria.nombre || '' : '';
  }

  getStatusSeverity(activo: boolean): string {
    return activo ? 'success' : 'danger';
  }

  getStatusLabel(activo: boolean): string {
    return activo ? 'Activo' : 'Inactivo';
  }
}