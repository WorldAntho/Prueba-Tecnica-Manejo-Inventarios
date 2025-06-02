namespace Backend.Application.DTO
{
    public record TipoTransaccionDTO
    {
        public int IdTipoTransaccion { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool? Activo { get; set; }
    }
}
