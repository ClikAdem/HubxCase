namespace Movie.Domain.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string message, string title = "Business Validation Exception", int status = 400)
        : base(message)
    {
        Title = title;
        ExceptionMessage = message;
        Status = status;
    }

    public BusinessException() : base()
    {
        Title = string.Empty;
        ExceptionMessage = string.Empty;
    }

    public BusinessException(string? message) : base(message)
    {
        ExceptionMessage = message ?? string.Empty;
        Title = "Business Validation Exception";
    }

    public BusinessException(string? message, Exception? innerException) : base(message, innerException)
    {
        ExceptionMessage = message ?? string.Empty;
        Title = "Business Validation Exception";
    }

    public string Title { get; set; }
    public int Status { get; set; }
    public string ExceptionMessage { get; set; }
}