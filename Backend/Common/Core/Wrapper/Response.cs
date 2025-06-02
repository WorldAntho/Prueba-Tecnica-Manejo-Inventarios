using System.Net;
using System.Text.Json.Serialization;

namespace Backend.Common.Core.Wrapper;
public class Response
{
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; protected set; }
    [JsonPropertyName("statusCode")]
    public HttpStatusCode StatusCode { get; protected set; }

    [JsonPropertyName("message")]
    public string Message { get; protected set; }

    [JsonPropertyName("errors")]
    public IReadOnlyDictionary<string, string[]>? Errors { get; protected set; }

    protected Response()
    {
        IsSuccess = true;
        StatusCode = HttpStatusCode.OK;
        Message = "Operación exitosa";
    }

    protected Response(HttpStatusCode statusCode, string message)
    {
        IsSuccess = ((int)statusCode).Between(200, 299);
        StatusCode = statusCode;
        Message = message;
    }

    protected Response(ApiException exception)
        : this(HttpStatusCode.BadRequest, exception.Message)
    {
    }

    public static Response Success(string? message = null)
    {
        return new Response(HttpStatusCode.OK, message ?? "Operación exitosa");
    }

    public static Response Failure(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new Response(statusCode, errorMessage);
    }

    public static Response ValidationFailure(IReadOnlyDictionary<string, string[]> errors)
    {
        return new Response(HttpStatusCode.UnprocessableEntity, "Error de validación")
        {
            Errors = errors
        };
    }
}

public sealed class Response<T> : Response
{
    [JsonPropertyName("data")]
    public T? Data { get; private set; }

    private Response() : base()
    {
    }

    private Response(T? data, HttpStatusCode statusCode, string message)
        : base(statusCode, message)
    {
        Data = data;
    }

    private Response(ApiException exception)
        : base(exception)
    {
    }

    public static Response<T> Success(T data, string? message = null)
    {
        return new Response<T>(data, HttpStatusCode.OK, message ?? "Operación exitosa");
    }

    public static new Response<T> Failure(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new Response<T>(default, statusCode, errorMessage);
    }

    public static new Response<T> ValidationFailure(IReadOnlyDictionary<string, string[]> errors)
    {
        return new Response<T>(default, HttpStatusCode.UnprocessableEntity, "Error de validación")
        {
            Errors = errors
        };
    }

    public static implicit operator Response<T>(T data) => Success(data);
}