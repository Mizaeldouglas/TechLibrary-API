using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Application.Checkouts;
using TechLibrary.Application.Services.LoggedUser;
using TechLibrary.Infrastructure.Data;

namespace TechLibrary.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckoutsController : ControllerBase
{
    private readonly LoggedUserService _loggedUser;
    private readonly TechLibraryDbContext _dbContext;

    public CheckoutsController(LoggedUserService loggedUser, TechLibraryDbContext dbContext)
    {
        _loggedUser = loggedUser;
        _dbContext = dbContext;
    }

    [HttpPost]
    [Route("{bookId}")]
    public IActionResult BookCheckout(Guid bookId)
    {
        var useCase = new RegisterBookCheckoutUseCase(_loggedUser, _dbContext);

        useCase.Execute(bookId);

        return NoContent();
    }
}