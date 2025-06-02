import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { RutasBackend } from '../../shared/utils/RutasBackend';
import { TipoTransaccionDTO, TipoTransaccionCommand } from '../models/Entidades';
import { IResponse } from '../models/IResponse';
import { AxiosService } from './axios.service';

@Injectable({ providedIn: 'root' })
export class TiposTransaccionService {
  constructor(private axios: AxiosService) {}

  getTiposTransaccion(): Observable<any> {
    return from(
      this.axios.getAxiosInstanceJSONAuth().get<IResponse<TipoTransaccionDTO[]>>(
        RutasBackend.tipoTransaccion.getTipoTransacciones
      )
    );
  }

  createTipoTransaccion(tipo: TipoTransaccionCommand): Observable<any> {
    return from(
      this.axios.getAxiosInstanceJSONAuth().post<IResponse<TipoTransaccionDTO>>(
        RutasBackend.tipoTransaccion.postTipoTransacciones,
        tipo
      )
    );
  }
}