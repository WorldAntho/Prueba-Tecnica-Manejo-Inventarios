using Backend.Common.Core.Wrapper;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Net;
using System.Text.Json;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = exception switch
        {
            DbUpdateException dbEx => HandleDbUpdateException(dbEx),
            MySqlException mySqlEx => HandleMySqlException(mySqlEx),
            _ => HandleGenericException(exception)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            }));
    }

    private static Response<object> HandleDbUpdateException(DbUpdateException ex)
    {
        string errorMessage = ex.InnerException switch
        {
            MySqlException sqlEx => GetMySqlErrorMessage(sqlEx),
            _ when ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true
                => "Ya existe un registro con esa informaci�n",
            _ => "Error al actualizar la base de datos"
        };

        return Response<object>.Failure(errorMessage, HttpStatusCode.Conflict);
    }

    private static Response<object> HandleMySqlException(MySqlException ex)
    {
        return Response<object>.Failure(
            GetMySqlErrorMessage(ex),
            HttpStatusCode.Conflict);
    }

    private static Response<object> HandleGenericException(Exception exception)
    {
        return Response<object>.Failure(
            GetGenericErrorMessage(exception),
            GetStatusCode(exception));
    }

    private static string GetMySqlErrorMessage(MySqlException ex)
    {
        return ex.Number switch
        {
            1062 => "Ya existe un registro con esa informaci�n",
            1045 => "Error de autenticaci�n con la base de datos",
            1146 => "Recurso no encontrado",
            1216 or 1217 => "Violaci�n de restricci�n de integridad referencial",
            1451 => "Registro en uso por otros datos del sistema",
            1452 => "Referencia a registro inexistente",
            2006 => "Conexi�n con MySQL interrumpida",
            _ => $"Error en la base de datos (C�digo: {ex.Number})"
        };
    }

    private static string GetGenericErrorMessage(Exception exception)
    {
        return exception switch
        {
            UnauthorizedAccessException => "Acceso no autorizado",
            ArgumentNullException argEx => $"Par�metro requerido faltante: {argEx.ParamName}",
            ArgumentException argEx => $"Argumento inv�lido: {argEx.Message}",
            InvalidOperationException => "Operaci�n no v�lida en el estado actual",
            _ => "Ocurri� un error inesperado"
        };
    }

    private static HttpStatusCode GetStatusCode(Exception exception)
    {
        return exception switch
        {
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            ArgumentException => HttpStatusCode.BadRequest,
            DbUpdateException => HttpStatusCode.Conflict,
            MySqlException => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
        };
    }
}