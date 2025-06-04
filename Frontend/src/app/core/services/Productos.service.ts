import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { RutasBackend } from '../../shared/utils/RutasBackend';
import { ProductosDTO, ProductosCommand } from '../models/Entidades';
import { IResponse } from '../models/IResponse';
import { AxiosService } from './axios.service';

@Injectable({ providedIn: 'root' })
export class ProductosService {
  constructor(private axios: AxiosService) {}

  getProducts(){
    return from(this.axios.getAxiosInstanceJSONAuth().get<IResponse<ProductosDTO[]>>(RutasBackend.producto.getproductos));
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
