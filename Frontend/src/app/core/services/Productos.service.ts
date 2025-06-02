import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { RutasBackend } from '../../shared/utils/RutasBackend';
import { ProductosDTO, ProductosCommand } from '../models/Entidades';
import { IResponse } from '../models/IResponse';
import { AxiosService } from './axios.service';

@Injectable({ providedIn: 'root' })
export class ProductosService {
  constructor(private axios: AxiosService) {}

  getProducts(page?: number, size?: number, filters?: any): Observable<any> {
    const params = new URLSearchParams();
    if (page !== undefined) params.append('page', page.toString());
    if (size !== undefined) params.append('size', size.toString());
    if (filters) {
      Object.keys(filters).forEach(key => {
        if (filters[key] !== null && filters[key] !== undefined && filters[key] !== '') {
          params.append(key, filters[key].toString());
        }
      });
    }

    return from(
      this.axios.getAxiosInstanceJSONAuth().get<IResponse<{
        data: ProductosDTO[];
        totalRecords: number;
        currentPage: number;
        totalPages: number;
      }>>(
        `${RutasBackend.producto.getproductos}?${params.toString()}`
      )
    );
  }

  getProductById(id: number): Observable<any> {
    return from(
      this.axios.getAxiosInstanceJSONAuth().get<IResponse<ProductosDTO>>(
        `${RutasBackend.producto.getproductos}/${id}`
      )
    );
  }

  createProduct(product: ProductosCommand): Observable<any> {
    return from(
      this.axios.getAxiosInstanceJSONAuth().post<IResponse<ProductosDTO>>(
        RutasBackend.producto.postproductos,
        product
      )
    );
  }

  updateProduct(id: number, product: ProductosCommand): Observable<any> {
    return from(
      this.axios.getAxiosInstanceJSONAuth().put<IResponse<ProductosDTO>>(
        `${RutasBackend.producto.postproductos}/${id}`,
        product
      )
    );
  }

  deleteProduct(id: number): Observable<any> {
    return from(
      this.axios.getAxiosInstanceJSONAuth().delete<IResponse<boolean>>(
        `${RutasBackend.producto.deleteproductos}/${id}`
      )
    );
  }
}
