namespace Backend.Application.DTO
{
    public record HistorialStockDTO
    {
        public int IdHistorial { get; set; }
        public int IdProducto { get; set; }
        public int StockAnterior { get; set; }
        public int StockNuevo { get; set; }
        public int? Diferencia { get; set; }
        public int? IdTransaccion { get; set; }
        public string Motivo { get; set; }
        public DateTime Fecha { get; set; }
    }
}
