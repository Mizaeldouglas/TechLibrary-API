using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TechLibrary.Exception;

namespace TechLibrary.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is TechLibraryExeception techLibraryExeception)
        {
            context.HttpContext.Response.StatusCode = (int)techLibraryExeception.GetHttpStatusCode();
            context.Result = new ObjectResult(new
            {
                errors = techLibraryExeception.GetErrorMessages()
            });
            
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new {
                errors = new List<string> { "Erro desconhecido" }
            });
        }
    }
}