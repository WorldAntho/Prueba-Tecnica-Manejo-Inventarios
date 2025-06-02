import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { RutasBackend } from '../../shared/utils/RutasBackend';
import { CategoriasDTO, CategoriaCommand } from '../models/Entidades';
import { IResponse } from '../models/IResponse';
import { AxiosService } from './axios.service';

@Injectable({ providedIn: 'root' })
export class CategoriasService {
  constructor(private axios: AxiosService) {}

  getCategorias(): Observable<any> {
    return from(
      this.axios.getAxiosInstanceJSONAuth().get<IResponse<CategoriasDTO[]>>(
        RutasBackend.categoria.getcategorias
      )
    );
  }

  createCategoria(categoria: CategoriaCommand): Observable<any> {
    return from(
      this.axios.getAxiosInstanceJSONAuth().post<IResponse<CategoriasDTO>>(
        RutasBackend.categoria.postcategorias,
        categoria
      )
    );
  }
}