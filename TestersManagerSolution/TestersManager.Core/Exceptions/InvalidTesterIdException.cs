namespace Exceptions;

public class InvalidTesterIdException : ArgumentException
{
    public InvalidTesterIdException()
    {
    }

    public InvalidTesterIdException(string? message) : base(message)
    {
    }

    public InvalidTesterIdException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}