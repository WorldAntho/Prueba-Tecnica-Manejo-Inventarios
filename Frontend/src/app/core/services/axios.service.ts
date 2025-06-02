import { Injectable } from '@angular/core';
import axios, { AxiosInstance } from 'axios';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AxiosService {

  private readonly axiosInstanceJSONAuth: AxiosInstance;
  private readonly axiosInstanceJSONNoAuth: AxiosInstance;
  private readonly axiosInstanceFormDataAuth: AxiosInstance;

  constructor(
) {
    // Instancia para JSON sin autenticación
    this.axiosInstanceJSONNoAuth = axios.create({
      baseURL: environment.apiUrl,
      headers: { 'Content-Type': 'application/json' }
    });

    // Instancia para JSON con autenticación
    this.axiosInstanceJSONAuth = axios.create({
      baseURL: environment.apiUrl,
      headers: { 'Content-Type': 'application/json' }
    });

    // Instancia para multipart/form-data con autenticación
    this.axiosInstanceFormDataAuth = axios.create({
      baseURL: environment.apiUrl,
      headers: { 'Content-Type': 'multipart/form-data' }
    });

  }



  // Métodos para obtener las instancias de Axios
  public getAxiosInstanceJSONAuth() {
    return this.axiosInstanceJSONAuth;
  }

  public getAxiosInstanceJSONNoAuth() {
    return this.axiosInstanceJSONNoAuth;
  }

  public getAxiosInstanceFormDataAuth() {
    return this.axiosInstanceFormDataAuth;
  }

  
}