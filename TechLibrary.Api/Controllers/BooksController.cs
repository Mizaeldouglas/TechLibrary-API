using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Application.UseCases.Books;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly FilterBookUseCase _useCase;

    public BooksController(FilterBookUseCase useCase)
    {
        this._useCase = useCase;
    }

    [HttpGet("Filter")]
    [ProducesResponseType(typeof(ResponseBooksJson), StatusCodes.Status200OK)]
    public IActionResult Filter(int pageNumber, string? title)
    {
        

        var request = new RequestFilterBooksJson
        {
            PageNumber = pageNumber,
            Title = title
        };

        var result = _useCase.Execute(request);


        return Ok(result);
    }
}