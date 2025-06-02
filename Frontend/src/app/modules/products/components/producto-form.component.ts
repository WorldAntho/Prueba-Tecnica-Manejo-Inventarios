import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { CategoriasDTO, ProductosCommand } from '../../../core/models/Entidades';
import { CategoriasService } from '../../../core/services/Categorias.service';
import { ProductosService } from '../../../core/services/Productos.service';
import { CommonModule } from '@angular/common';
import { DropdownModule } from 'primeng/dropdown';
import { ImportsModule } from '../../../core/models/Imports';

@Component({
  selector: 'app-producto-form',
    imports: [
    CommonModule,
    FormsModule,
    DropdownModule,
    ImportsModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './producto-form.component.html',
  styleUrls: ['./producto-form.component.css']
})
export class ProductoFormComponent implements OnInit {
  productoForm!: FormGroup;
  categorias: CategoriasDTO[] = [];
  isEditMode = false;
  productId?: number;
  loading = false;
  submitting = false;

  constructor(
    private fb: FormBuilder,
    private productosService: ProductosService,
    private categoriasService: CategoriasService,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.initForm();
  }

  ngOnInit() {
    this.loadCategorias();
    this.checkEditMode();
  }

  initForm() {
    this.productoForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(100)]],
      descripcion: ['', [Validators.maxLength(500)]],
      idCategoria: [null, [Validators.required]],
      precio: [0, [Validators.required, Validators.min(0)]],
      stock: [0, [Validators.required, Validators.min(0)]],
      activo: [true]
    });
  }

  loadCategorias() {
    this.categoriasService.getCategorias().subscribe({
      next: (response) => {
        if (response.data.isSuccess) {
          this.categorias = response.data.data.filter((c: CategoriasDTO) => c.activo);
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

  checkEditMode() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.productId = +id;
      this.loadProduct();
    }
  }

  loadProduct() {
    if (!this.productId) return;

    this.loading = true;
    this.productosService.getProductById(this.productId).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.data.isSuccess) {
          const producto = response.data.data;
          this.productoForm.patchValue({
            nombre: producto.nombre,
            descripcion: producto.descripcion,
            idCategoria: producto.idCategoria,
            precio: producto.precio,
            stock: producto.stock,
            activo: producto.activo
          });
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Producto no encontrado'
          });
          this.router.navigate(['/productos']);
        }
      },
      error: (error) => {
        this.loading = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Error al cargar producto'
        });
        this.router.navigate(['/productos']);
      }
    });
  }

  onSubmit() {
    if (this.productoForm.invalid) {
      this.markFormGroupTouched();
      return;
    }

    this.submitting = true;
    const formData: ProductosCommand = {
      ...this.productoForm.value,
      idProducto: this.isEditMode ? this.productId : undefined,
      fechaCreacion: this.isEditMode ? undefined : new Date(),
      fechaActualizacion: this.isEditMode ? new Date() : undefined
    };

    const operation = this.isEditMode
      ? this.productosService.updateProduct(this.productId!, formData)
      : this.productosService.createProduct(formData);

    operation.subscribe({
      next: (response) => {
        this.submitting = false;
        if (response.data.isSuccess) {
          this.messageService.add({
            severity: 'success',
            summary: 'Éxito',
            detail: `Producto ${this.isEditMode ? 'actualizado' : 'creado'} correctamente`
          });
          setTimeout(() => {
            this.router.navigate(['/productos']);
          }, 1500);
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: response.data.message || `Error al ${this.isEditMode ? 'actualizar' : 'crear'} producto`
          });
        }
      },
      error: (error) => {
        this.submitting = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: `Error al ${this.isEditMode ? 'actualizar' : 'crear'} producto`
        });
      }
    });
  }

  onCancel() {
    this.router.navigate(['/productos']);
  }

  markFormGroupTouched() {
    Object.keys(this.productoForm.controls).forEach(key => {
      const control = this.productoForm.get(key);
      control?.markAsTouched();
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.productoForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const field = this.productoForm.get(fieldName);
    if (field?.errors) {
      if (field.errors['required']) return `${fieldName} es requerido`;
      if (field.errors['maxlength']) return `${fieldName} excede la longitud máxima`;
      if (field.errors['min']) return `${fieldName} debe ser mayor o igual a 0`;
    }
    return '';
  }
}
