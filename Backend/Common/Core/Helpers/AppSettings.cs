namespace Backend.Common.Core.Helpers
{
    public class AppSettings
    {
        public string? OriginCors { get; set; }
        public string? Secret { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
    }
}