namespace Backend.Common.Core.Helpers
{
    public static class Mensaje
    {
        public const string NotFound = "No se ha encontrado el recurso especificado";

        public static string GetMessage(bool result, string accion)
        {
            if (string.IsNullOrWhiteSpace(accion))
            {
                throw new ArgumentException("El par�metro no puede ser nulo o vac�o", nameof(accion));
            }

            return result
                ? $"Se ha {accion.ToLowerInvariant()} el recurso correctamente"
                : $"Error, no se ha {accion.ToLowerInvariant()} el recurso";
        }

        public static string AlreadyExist(string atributo)
        {
            if (string.IsNullOrWhiteSpace(atributo))
            {
                throw new ArgumentException("El par�metro no puede ser nulo o vac�o", nameof(atributo));
            }

            return $"Error, no se ha podido crear el recurso porque {atributo.ToLowerInvariant()} ya existe";
        }
    }
}