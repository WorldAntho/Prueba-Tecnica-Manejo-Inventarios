import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService, ConfirmationService } from 'primeng/api';
import { TransaccionesDTO, ProductosDTO, TipoTransaccionDTO } from '../../../../core/models/Entidades';
import { ProductosService } from '../../../../core/services/Productos.service';
import { TiposTransaccionService } from '../../../../core/services/Tipos-transaccion.service';
import { TransaccionesService } from '../../../../core/services/Transacciones.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DropdownModule } from 'primeng/dropdown';
import { ImportsModule } from '../../../../core/models/Imports';

@Component({
  selector: 'app-transacciones-list',
    imports: [
    CommonModule,
    FormsModule,
    DropdownModule,
    ImportsModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './transacciones-list.component.html',
  styleUrls: ['./transacciones-list.component.css']
})
export class TransaccionesListComponent implements OnInit {
  transacciones: TransaccionesDTO[] = [];
  productos: ProductosDTO[] = [];
  tiposTransaccion: TipoTransaccionDTO[] = [];
  loading = false;
  totalRecords = 0;
  first = 0;
  rows = 10;

  // Filtros
  filters = {
    fechaInicio: null,
    fechaFin: null,
    idTipoTransaccion: null,
    idProducto: null,
    numeroDocumento: '',
    activo: null
  };

  // Opciones para filtros
  activeOptions = [
    { label: 'Todos', value: null },
    { label: 'Activos', value: true },
    { label: 'Inactivos', value: false }
  ];

  constructor(
    private transaccionesService: TransaccionesService,
    private productosService: ProductosService,
    private tiposTransaccionService: TiposTransaccionService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadCatalogos();
    this.loadTransacciones();
  }

  loadCatalogos() {
    // Cargar productos
    this.productosService.getProducts().subscribe({
      next: (response) => {
        if (response.data.isSuccess) {
          this.productos = response.data.data.data || response.data.data;
        }
      }
    });

    // Cargar tipos de transacción
    this.tiposTransaccionService.getTiposTransaccion().subscribe({
      next: (response) => {
        if (response.data.isSuccess) {
          this.tiposTransaccion = response.data.data;
        }
      }
    });
  }

  loadTransacciones(event?: any) {
    this.loading = true;
    
    const page = event ? Math.floor(event.first / event.rows) + 1 : 1;
    const size = event ? event.rows : this.rows;

    this.transaccionesService.getTransactions(page, size, this.filters).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.data.isSuccess) {
          this.transacciones = response.data.data.data;
          this.totalRecords = response.data.data.totalRecords;
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: response.data.message || 'Error al cargar transacciones'
          });
        }
      },
      error: (error) => {
        this.loading = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Error al cargar transacciones'
        });
      }
    });
  }

  applyFilters() {
    this.first = 0;
    this.loadTransacciones();
  }

  clearFilters() {
    this.filters = {
      fechaInicio: null,
      fechaFin: null,
      idTipoTransaccion: null,
      idProducto: null,
      numeroDocumento: '',
      activo: null
    };
    this.first = 0;
    this.loadTransacciones();
  }

  createTransaction() {
    this.router.navigate(['/transacciones/crear']);
  }

  editTransaction(transaccion: TransaccionesDTO) {
    this.router.navigate(['/transacciones/editar', transaccion.idTransaccion]);
  }

  deleteTransaction(transaccion: TransaccionesDTO) {
    this.confirmationService.confirm({
      message: `¿Está seguro de eliminar la transacción #${transaccion.numeroDocumento}?`,
      header: 'Confirmar Eliminación',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.transaccionesService.deleteTransaction(transaccion.idTransaccion!).subscribe({
          next: (response) => {
            if (response.data.isSuccess) {
              this.messageService.add({
                severity: 'success',
                summary: 'Éxito',
                detail: 'Transacción eliminada correctamente'
              });
              this.loadTransacciones();
            } else {
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: response.data.message || 'Error al eliminar transacción'
              });
            }
          },
          error: (error) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'Error al eliminar transacción'
            });
          }
        });
      }
    });
  }

  getProductoName(idProducto: number): string {
    const producto = this.productos.find(p => p.idProducto === idProducto);
    return producto ? producto.nombre || '' : '';
  }

  getTipoTransaccionName(idTipo: number): string {
    const tipo = this.tiposTransaccion.find(t => t.idTipoTransaccion === idTipo);
    return tipo ? tipo.nombre || '' : '';
  }

  getStatusSeverity(activo: boolean): string {
    return activo ? 'success' : 'danger';
  }

  getStatusLabel(activo: boolean): string {
    return activo ? 'Activo' : 'Inactivo';
  }

  getTipoSeverity(idTipo: number): string {
    const tipo = this.tiposTransaccion.find(t => t.idTipoTransaccion === idTipo);
    if (tipo?.nombre?.toLowerCase().includes('entrada') || tipo?.nombre?.toLowerCase().includes('compra')) {
      return 'success';
    }
    if (tipo?.nombre?.toLowerCase().includes('salida') || tipo?.nombre?.toLowerCase().includes('venta')) {
      return 'info';
    }
    return 'secondary';
  }
}
