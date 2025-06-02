import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { RutasBackend } from '../../shared/utils/RutasBackend';
import { TransaccionesDTO, TransaccionesCommand } from '../models/Entidades';
import { IResponse } from '../models/IResponse';
import { AxiosService } from './axios.service';

@Injectable({ providedIn: 'root' })
export class TransaccionesService {
  constructor(private axios: AxiosService) {}

  getTransactions(page?: number, size?: number, filters?: any): Observable<any> {
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
        data: TransaccionesDTO[];
        totalRecords: number;
        currentPage: number;
        totalPages: number;
      }>>(
        `${RutasBackend.transacciones.gettransacciones}?${params.toString()}`
      )
    );
  }

  getTransactionById(id: number): Observable<any> {
    return from(
      this.axios.getAxiosInstanceJSONAuth().get<IResponse<TransaccionesDTO>>(
        `${RutasBackend.transacciones.gettransacciones}/${id}`
      )
    );
  }

  createTransaction(transaction: TransaccionesCommand): Observable<any> {
    return from(
      this.axios.getAxiosInstanceJSONAuth().post<IResponse<TransaccionesDTO>>(
        RutasBackend.transacciones.posttransacciones,
        transaction
      )
    );
  }

  updateTransaction(id: number, transaction: TransaccionesCommand): Observable<any> {
    return from(
      this.axios.getAxiosInstanceJSONAuth().put<IResponse<TransaccionesDTO>>(
        `${RutasBackend.transacciones.posttransacciones}/${id}`,
        transaction
      )
    );
  }

  deleteTransaction(id: number): Observable<any> {
    return from(
      this.axios.getAxiosInstanceJSONAuth().delete<IResponse<boolean>>(
        `${RutasBackend.transacciones.deleteTransacciones}/${id}`
      )
    );
  }
}
