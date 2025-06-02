
export interface AdjuntosCommand {
    idadjuntos?: number | undefined;
    archivo: string | undefined;
}

export interface AdjuntosDTO {
    idadjuntos?: number;
    nombreArchivo?: string | undefined;
    extension?: string | undefined;
    mimeType?: string | undefined;
    tamanioBytes?: number | undefined;
    ruta?: string | undefined;
}

export interface CategoriaCommand {
    idCategoria?: number | undefined;
    nombre?: string | undefined;
    descripcion?: string | undefined;
    activo?: boolean | undefined;
    fechaCreacion?: Date;
    fechaActualizacion?: Date | undefined;
}

export interface CategoriasDTO {
    idCategoria?: number;
    nombre?: string | undefined;
    descripcion?: string | undefined;
    activo?: boolean | undefined;
    fechaCreacion?: Date;
    fechaActualizacion?: Date | undefined;
}


export interface DeleteAdjuntoCommand {
    idadjuntos?: number;
}

export interface DeleteCategoriaCommand {
    idCategoria?: number;
}

export interface DeleteHistorialStockCommand {
    idHistorial?: number;
}

export interface DeleteProductoCommand {
    idProducto?: number;
}

export interface DeleteTipoTransaccionCommand {
    idTipoTransaccion?: number;
}

export interface DeleteTransaccionCommand {
    idTransaccion?: number;
}

export interface HistorialStockCommand {
    idHistorial?: number | undefined;
    idProducto?: number | undefined;
    stockAnterior?: number;
    stockNuevo?: number;
    diferencia?: number | undefined;
    idTransaccion?: number | undefined;
    motivo?: string | undefined;
    fecha?: Date;
}

export interface HistorialStockDTO {
    idHistorial?: number;
    idProducto?: number;
    stockAnterior?: number;
    stockNuevo?: number;
    diferencia?: number | undefined;
    idTransaccion?: number | undefined;
    motivo?: string | undefined;
    fecha?: Date;
}


export interface ProductosCommand {
    idProducto?: number | undefined;
    nombre?: string | undefined;
    descripcion?: string | undefined;
    idCategoria?: number;
    idadjuntos?: number;
    precio?: number;
    stock?: number;
    activo?: boolean | undefined;
    fechaCreacion?: Date;
    fechaActualizacion?: Date | undefined;
}

export interface ProductosDTO {
    idProducto?: number;
    nombre?: string | undefined;
    descripcion?: string | undefined;
    idCategoria?: number;
    idadjuntos?: number;
    precio?: number;
    stock?: number;
    activo?: boolean | undefined;
    fechaCreacion?: Date;
    fechaActualizacion?: Date | undefined;
}

export interface TipoTransaccionCommand {
    idTipoTransaccion?: number | undefined;
    nombre?: string | undefined;
    descripcion?: string | undefined;
    activo?: boolean | undefined;
}

export interface TipoTransaccionDTO {
    idTipoTransaccion?: number;
    nombre?: string | undefined;
    descripcion?: string | undefined;
    activo?: boolean | undefined;
}

export interface TransaccionesCommand {
    idTransaccion?: number | undefined;
    fecha?: Date;
    idTipoTransaccion?: number;
    idProducto?: number;
    cantidad?: number;
    precioUnitario?: number;
    precioTotal?: number | undefined;
    detalle?: string | undefined;
    numeroDocumento?: string | undefined;
    activo?: boolean | undefined;
    fechaCreacion?: Date;
}

export interface TransaccionesDTO {
    idTransaccion?: number;
    fecha?: Date;
    idTipoTransaccion?: number;
    idProducto?: number;
    cantidad?: number;
    precioUnitario?: number;
    precioTotal?: number | undefined;
    detalle?: string | undefined;
    numeroDocumento?: string | undefined;
    activo?: boolean | undefined;
    fechaCreacion?: Date;
}
