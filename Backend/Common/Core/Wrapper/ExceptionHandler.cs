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
            return Response<T>.Success(result, successMessage ?? "Operaci�n completada con �xito.");
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
            ArgumentException argEx => $"Argumento inv�lido: {argEx.Message}",
            InvalidOperationException _ => ex.Message,
            DbUpdateException dbEx => GetDbUpdateErrorMessage(dbEx),
            MySqlException sqlEx => GetMySqlErrorMessage(sqlEx),
            ValidationException _ => $"Error de validaci�n: {ex.Message}",
            UnauthorizedAccessException _ => "No tiene permiso para realizar esta acci�n.",
            TaskCanceledException _ or OperationCanceledException _ => "La operaci�n fue cancelada.",
            TimeoutException _ => "La operaci�n ha excedido el tiempo de espera.",
            _ => "Ha ocurrido un error inesperado. Por favor, int�ntelo de nuevo m�s tarde."
        };
    }

    private static string GetDbUpdateErrorMessage(DbUpdateException ex)
    {
        return ex.InnerException switch
        {
            MySqlException sqlEx => GetMySqlErrorMessage(sqlEx),
            _ when ex.InnerException?.Message?.Contains("Duplicate entry", StringComparison.OrdinalIgnoreCase) == true
                => "Ya existe un registro con esa informaci�n. Por favor, utilice datos �nicos.",
            _ => "Ocurri� un error al actualizar la base de datos. Por favor, revise los datos e int�ntelo de nuevo."
        };
    }

    private static string GetMySqlErrorMessage(MySqlException ex)
    {
        return ex.Number switch
        {
            1062 => "Ya existe un registro con esa informaci�n. Por favor, utilice datos �nicos.",
            1045 => "Error de autenticaci�n en la base de datos.",
            1146 => "La tabla solicitada no existe.",
            1216 or 1217 => "Violaci�n de restricci�n de clave for�nea.",
            1451 => "No se puede realizar la operaci�n porque este registro est� siendo utilizado en otra parte del sistema.",
            1452 => "La informaci�n proporcionada no coincide con los registros existentes. Por favor, verifique los datos.",
            2006 => "Se perdi� la conexi�n con el servidor MySQL.",
            _ => $"Ocurri� un error en la base de datos (C�digo: {ex.Number}). Por favor, revise los datos e int�ntelo de nuevo."
        };
    }
}