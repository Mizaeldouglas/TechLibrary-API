using System.Net;

namespace TechLibrary.Exception;

public abstract class TechLibraryExeception : SystemException
{
    public abstract List<string> GetErrorMessages();
    public abstract HttpStatusCode GetHttpStatusCode();
    
}