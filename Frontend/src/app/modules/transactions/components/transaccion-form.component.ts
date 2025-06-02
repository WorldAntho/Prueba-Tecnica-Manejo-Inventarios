import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ProductosDTO, TipoTransaccionDTO, TransaccionesCommand } from '../../../core/models/Entidades';
import { ProductosService } from '../../../core/services/Productos.service';
import { TiposTransaccionService } from '../../../core/services/Tipos-transaccion.service';
import { TransaccionesService } from '../../../core/services/Transacciones.service';
import { CommonModule } from '@angular/common';
import { DropdownModule } from 'primeng/dropdown';
import { ImportsModule } from '../../../core/models/Imports';

@Component({
  selector: 'app-transaccion-form',
  imports: [
    CommonModule,
    FormsModule,
    DropdownModule,
    ImportsModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './transaccion-form.component.html',
  styleUrls: ['./transaccion-form.component.css']
})
export class TransaccionFormComponent implements OnInit {
  transaccionForm!: FormGroup;
  productos: ProductosDTO[] = [];
  tiposTransaccion: TipoTransaccionDTO[] = [];
  isEditMode = false;
  transactionId?: number;
  loading = false;
  submitting = false;

  constructor(
    private fb: FormBuilder,
    private transaccionesService: TransaccionesService,
    private productosService: ProductosService,
    private tiposTransaccionService: TiposTransaccionService,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.initForm();
  }

  ngOnInit() {
    this.loadCatalogos();
    this.checkEditMode();
  }

  initForm() {
    this.transaccionForm = this.fb.group({
      fecha: [new Date(), [Validators.required]],
      idTipoTransaccion: [null, [Validators.required]],
      idProducto: [null, [Validators.required]],
      cantidad: [1, [Validators.required, Validators.min(1)]],
      precioUnitario: [0, [Validators.required, Validators.min(0)]],
      precioTotal: [{ value: 0, disabled: true }],
      detalle: [''],
      numeroDocumento: ['', [Validators.required]],
      activo: [true]
    });

    // Calcular precio total automáticamente
    this.transaccionForm.get('cantidad')?.valueChanges.subscribe(() => {
      this.calculateTotal();
    });

    this.transaccionForm.get('precioUnitario')?.valueChanges.subscribe(() => {
      this.calculateTotal();
    });
  }

  loadCatalogos() {
    // Cargar productos activos
    this.productosService.getProducts().subscribe({
      next: (response) => {
        if (response.data.isSuccess) {
          this.productos = (response.data.data.data || response.data.data)
            .filter((p: ProductosDTO) => p.activo && p.stock! > 0);
        }
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Error al cargar productos'
        });
      }
    });

    // Cargar tipos de transacción activos
    this.tiposTransaccionService.getTiposTransaccion().subscribe({
      next: (response) => {
        if (response.data.isSuccess) {
          this.tiposTransaccion = response.data.data.filter((t: TipoTransaccionDTO) => t.activo);
        }
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Error al cargar tipos de transacción'
        });
      }
    });
  }

  checkEditMode() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.transactionId = +id;
      this.loadTransaction();
    }
  }

  loadTransaction() {
    if (!this.transactionId) return;

    this.loading = true;
    this.transaccionesService.getTransactionById(this.transactionId).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.data.isSuccess) {
          const transaccion = response.data.data;
          this.transaccionForm.patchValue({
            fecha: new Date(transaccion.fecha),
            idTipoTransaccion: transaccion.idTipoTransaccion,
            idProducto: transaccion.idProducto,
            cantidad: transaccion.cantidad,
            precioUnitario: transaccion.precioUnitario,
            detalle: transaccion.detalle,
            numeroDocumento: transaccion.numeroDocumento,
            activo: transaccion.activo
          });
          this.calculateTotal();
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Transacción no encontrada'
          });
          this.router.navigate(['/transacciones']);
        }
      },
      error: (error) => {
        this.loading = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Error al cargar transacción'
        });
        this.router.navigate(['/transacciones']);
      }
    });
  }

  calculateTotal() {
    const cantidad = this.transaccionForm.get('cantidad')?.value || 0;
    const precioUnitario = this.transaccionForm.get('precioUnitario')?.value || 0;
    const total = cantidad * precioUnitario;
    this.transaccionForm.get('precioTotal')?.setValue(total);
  }

  onProductChange() {
    const productoId = this.transaccionForm.get('idProducto')?.value;
    if (productoId) {
      const producto = this.productos.find(p => p.idProducto === productoId);
      if (producto) {
        this.transaccionForm.get('precioUnitario')?.setValue(producto.precio);
      }
    }
  }

  onSubmit() {
    if (this.transaccionForm.invalid) {
      this.markFormGroupTouched();
      return;
    }

    // Validar stock disponible para salidas
    if (!this.isEditMode) {
      const cantidad = this.transaccionForm.get('cantidad')?.value;
      const productoId = this.transaccionForm.get('idProducto')?.value;
      const tipoId = this.transaccionForm.get('idTipoTransaccion')?.value;
      
      const producto = this.productos.find(p => p.idProducto === productoId);
      const tipo = this.tiposTransaccion.find(t => t.idTipoTransaccion === tipoId);
      
      if (tipo?.nombre?.toLowerCase().includes('salida') || tipo?.nombre?.toLowerCase().includes('venta')) {
        if (producto && producto.stock! < cantidad) {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: `Stock insuficiente. Disponible: ${producto.stock}`
          });
          return;
        }
      }
    }

    this.submitting = true;
    const formValue = this.transaccionForm.getRawValue();
    const formData: TransaccionesCommand = {
      ...formValue,
      idTransaccion: this.isEditMode ? this.transactionId : undefined,
      fechaCreacion: this.isEditMode ? undefined : new Date()
    };

    const operation = this.isEditMode
      ? this.transaccionesService.updateTransaction(this.transactionId!, formData)
      : this.transaccionesService.createTransaction(formData);

    operation.subscribe({
      next: (response) => {
        this.submitting = false;
        if (response.data.isSuccess) {
          this.messageService.add({
            severity: 'success',
            summary: 'Éxito',
            detail: `Transacción ${this.isEditMode ? 'actualizada' : 'creada'} correctamente`
          });
          setTimeout(() => {
            this.router.navigate(['/transacciones']);
          }, 1500);
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: response.data.message || `Error al ${this.isEditMode ? 'actualizar' : 'crear'} transacción`
          });
        }
      },
      error: (error) => {
        this.submitting = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: `Error al ${this.isEditMode ? 'actualizar' : 'crear'} transacción`
        });
      }
    });
  }

  onCancel() {
    this.router.navigate(['/transacciones']);
  }

  markFormGroupTouched() {
    Object.keys(this.transaccionForm.controls).forEach(key => {
      const control = this.transaccionForm.get(key);
      control?.markAsTouched();
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.transaccionForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const field = this.transaccionForm.get(fieldName);
    if (field?.errors) {
      if (field.errors['required']) return `${fieldName} es requerido`;
      if (field.errors['min']) return `${fieldName} debe ser mayor que 0`;
    }
    return '';
  }

  getSelectedProductStock(): number {
    const productoId = this.transaccionForm.get('idProducto')?.value;
    if (productoId) {
      const producto = this.productos.find(p => p.idProducto === productoId);
      return producto?.stock || 0;
    }
    return 0;
  }
}