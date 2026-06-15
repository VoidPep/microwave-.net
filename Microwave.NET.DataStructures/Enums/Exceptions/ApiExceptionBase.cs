namespace Microwave.NET.DataStructures.Enums.Exceptions;

public class ApiExceptionBase : Exception
{
    public int StatusCode { get; }

    public ApiExceptionBase(string message, int statusCode = 400)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public ApiExceptionBase(string message, Exception innerException, int statusCode = 400)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}