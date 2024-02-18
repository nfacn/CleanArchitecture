namespace CleanArchitecture.Application.Common.Exceptions;
public class NotFoundException : Exception
{
    public NotFoundException(string message = "NotFound") : base(message) { }
}

