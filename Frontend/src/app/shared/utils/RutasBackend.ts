export const RutasBackend = {
    adjuntos: {
        getadjuntos: '/api/Adjuntos/ver-adjuntos',
        postadjuntos: '/api/Adjuntos/enviar-adjuntos',
        deleteadjuntos: '/api/Adjuntos/eliminar-adjunto',
    },
    categoria: {
        getcategorias: '/api/Categoria/ver-categorias',
        postcategorias: '/api/Categoria/enviar-categoria',
        deletecategorias: '/api/Categoria/eliminar-categoria',
    },
    historialStock: {
        getHistorialStock: '/api/HistorialStock/ver-historial-stock',
        postHistorialStock: '/api/HistorialStock/enviar-historial-stock',
        deleteHistorialStock: '/api/HistorialStock/eliminar-historial-stock',
    },
    producto: {
        getproductos: '/api/Producto/ver-productos',
        postproductos: '/api/Producto/enviar-producto',
        deleteproductos: '/api/Producto/eliminar-producto',
    },
    tipoTransaccion: {
        getTipoTransacciones: '/api/TipoTransaccion/ver-tipos-transaccion',
        postTipoTransacciones: '/api/TipoTransaccion/enviar-tipo-transaccion',
        deleteTipoTransacciones: '/api/TipoTransaccion/eliminar-tipo-transaccion',
    },
    transacciones: {
        gettransacciones: '/api/Transacciones/ver-transacciones',
        posttransacciones: '/api/Transacciones/enviar-transaccion',
        deleteTransacciones: '/api/Transacciones/eliminar-transaccion',
    }
}