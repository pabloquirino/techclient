namespace TechClient.Domain.Exceptions;

public class BusinessException : ArgumentException
{
    public BusinessException(string message) : base(message) { }
}