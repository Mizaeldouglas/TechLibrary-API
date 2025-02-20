using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Application.UseCases.Users.Login.DoLogin;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly DoLoginUseCase _doLoginUseCase;

    public LoginController(DoLoginUseCase doLoginUseCase)
    {
        _doLoginUseCase = doLoginUseCase;
    }


    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status401Unauthorized)]
    public IActionResult DoLogin(RequestLoginJson request)
    {
        
        var response = _doLoginUseCase.Execute(request);
     
        return Ok(response);
    }
    
}