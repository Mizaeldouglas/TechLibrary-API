using System.Net;

namespace TechLibrary.Exception;

public class InvalidLoginException : TechLibraryExeception
{
    public override List<string> GetErrorMessages()
    {
        return ["email or password is incorrect"];
    }

    public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Unauthorized;
}