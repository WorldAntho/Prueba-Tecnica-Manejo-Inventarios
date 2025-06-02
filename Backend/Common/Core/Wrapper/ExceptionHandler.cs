using Backend.Common.Core.Wrapper;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.ComponentModel.DataAnnotations;

public static class ExceptionHandler
{
    public static async Task<Response<T>> HandleExceptionAsync<T>(
        Func<Task<T>> action,
        string? successMessage = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(action);

        try
        {
            var result = await action().ConfigureAwait(false);
            return Response<T>.Success(result, successMessage ?? "Operación completada con éxito.");
        }
        catch (Exception ex)
        {
            string errorMessage = GetSpecificErrorMessage(ex);
            return Response<T>.Failure(errorMessage);
        }
    }

    public static string GetSpecificErrorMessage(Exception ex)
    {
        return ex switch
        {
            ArgumentNullException argEx => $"Valor requerido no proporcionado: {argEx.ParamName}",
            ArgumentException argEx => $"Argumento inválido: {argEx.Message}",
            InvalidOperationException _ => ex.Message,
            DbUpdateException dbEx => GetDbUpdateErrorMessage(dbEx),
            MySqlException sqlEx => GetMySqlErrorMessage(sqlEx),
            ValidationException _ => $"Error de validación: {ex.Message}",
            UnauthorizedAccessException _ => "No tiene permiso para realizar esta acción.",
            TaskCanceledException _ or OperationCanceledException _ => "La operación fue cancelada.",
            TimeoutException _ => "La operación ha excedido el tiempo de espera.",
            _ => "Ha ocurrido un error inesperado. Por favor, inténtelo de nuevo más tarde."
        };
    }

    private static string GetDbUpdateErrorMessage(DbUpdateException ex)
    {
        return ex.InnerException switch
        {
            MySqlException sqlEx => GetMySqlErrorMessage(sqlEx),
            _ when ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true
                => "Ya existe un registro con esa información. Por favor, utilice datos únicos.",
            _ => "Ocurrió un error al actualizar la base de datos. Por favor, revise los datos e inténtelo de nuevo."
        };
    }

    private static string GetMySqlErrorMessage(MySqlException ex)
    {
        return ex.Number switch
        {
            1062 => "Ya existe un registro con esa información. Por favor, utilice datos únicos.",
            1045 => "Error de autenticación en la base de datos.",
            1146 => "La tabla solicitada no existe.",
            1216 or 1217 => "Violación de restricción de clave foránea.",
            1451 => "No se puede realizar la operación porque este registro está siendo utilizado en otra parte del sistema.",
            1452 => "La información proporcionada no coincide con los registros existentes. Por favor, verifique los datos.",
            2006 => "Se perdió la conexión con el servidor MySQL.",
            _ => $"Ocurrió un error en la base de datos (Código: {ex.Number}). Por favor, revise los datos e inténtelo de nuevo."
        };
    }
}