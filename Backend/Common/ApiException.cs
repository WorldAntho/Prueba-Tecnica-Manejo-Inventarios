using System.Globalization;

namespace Backend.Common
{
    public class ApiException : Exception
    {

        #region Ctor
        public ApiException()
            : base()
        { }
        public ApiException(string message)
            : base(message)
        { }
        public ApiException(Exception innerException, string message)
            : base(message, innerException)
        { }
        public ApiException(Exception innerException)
            : base("Error no definido. Consulte al departamento de soporte los errores internos para más detalles.", innerException)
        { }
        public ApiException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args)) { }
        #endregion

        #region Propiedades
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        #endregion
    }
}