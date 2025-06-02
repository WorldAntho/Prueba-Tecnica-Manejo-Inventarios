namespace Backend.Application.DTO
{
    public record AdjuntosDTO
    {
        public int Idadjuntos { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public long? TamanioBytes { get; set; }
        public string Ruta { get; set; }
    }
}
