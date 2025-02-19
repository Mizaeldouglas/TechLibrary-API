using System.Net;

namespace TechLibrary.Exception;

public class ErrorOnValidationException : TechLibraryExeception
{
    private readonly List<string> _errorMessages;

    public ErrorOnValidationException(List<string> errorMessages)
    {
        _errorMessages = errorMessages;
    }

    public override List<string> GetErrorMessages() => _errorMessages;
    public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.BadRequest;
}