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

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
    public IActionResult Post(RequestUserJson request)
    {
        try
        {
            var response = _registerUserUsecase.Execute(request);
            return Created(string.Empty, response);
        }
        catch (ErrorOnValidationException e)
        {
            return BadRequest(new ResponseErrorMessageJson
            {
                Errors = e.GetErrorMessages()
            });
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorMessageJson
            {
                Errors = new List<string> { "Erro desconhecido" }
            });
        }
    }
}