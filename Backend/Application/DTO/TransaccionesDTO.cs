namespace Backend.Application.DTO
{
    public record TransaccionesDTO
    {
        public int IdTransaccion { get; set; }
        public DateTime Fecha { get; set; }
        public int IdTipoTransaccion { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal? PrecioTotal { get; set; }
        public string Detalle { get; set; }
        public string NumeroDocumento { get; set; }
        public bool? Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
