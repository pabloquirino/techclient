namespace TechClient.Domain.Exceptions;

public class NotFoundException : KeyNotFoundException
{
    public NotFoundException(string entity, string key)
        : base($"{entity} not found: {key}") { }
}