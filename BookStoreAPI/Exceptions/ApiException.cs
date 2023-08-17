namespace BookStoreAPI.Exceptions;

public class ApiException : Exception
{
    public ApiException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
        Content = message;
    }

    public int StatusCode { get; }
    public string Content { get; }
}

public static class ApiExceptionHandler
{
    public static void ThrowIf(bool condition, int statusCode, string message)
    {
        if (condition) throw new ApiException(statusCode, message);
    }
}