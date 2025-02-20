using Microsoft.AspNetCore.Mvc;
using TechLibrary.Application.UseCases.Users.Register;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly RegisterUserUsecase _registerUserUsecase;

    public UsersController(RegisterUserUsecase registerUserUsecase)
    {
        _registerUserUsecase = registerUserUsecase;
    }

   

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
    public IActionResult Post(RequestUserJson request)
    {
        var response = _registerUserUsecase.Execute(request);
        return Created(string.Empty, response);
    }
}