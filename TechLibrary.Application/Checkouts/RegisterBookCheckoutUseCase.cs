using TechLibrary.Application.Services.LoggedUser;
using TechLibrary.Exception;
using TechLibrary.Infrastructure.Data;

namespace TechLibrary.Application.Checkouts;

public class RegisterBookCheckoutUseCase
{
    private readonly TechLibraryDbContext _dbContext;
    private readonly LoggedUserService _loggedUser;
    
    private const int MAX_LOAN_DAYS = 7;

    public RegisterBookCheckoutUseCase(LoggedUserService loggedUser, TechLibraryDbContext dbContext)
    {
        this._dbContext = dbContext;
        _loggedUser = loggedUser;
    }

    public void Execute(Guid bookId)
    {

        Validate(_dbContext, bookId);

        var user = _loggedUser.User(_dbContext);

        var entity = new Domain.Entities.Checkout
        {
            UserId = user.Id,
            BookId = bookId,
            ExpectedReturnDate = DateTime.UtcNow.AddDays(MAX_LOAN_DAYS)
        };

        _dbContext.Checkouts.Add(entity);

        _dbContext.SaveChanges();
    }

    private void Validate(TechLibraryDbContext dbContext, Guid bookId)
    {
        var book = dbContext.Books.FirstOrDefault(book => book.Id == bookId);
        if (book is null)
            throw new NotFoundException("Livro não encontrado!");

        var amountBookNotReturned = dbContext.Checkouts.Count(checkout => checkout.BookId == bookId && checkout.ReturnedDate == null);

        if(amountBookNotReturned == book.Amount)
            throw new ConflictException("Livro não está disponível para empréstimo!");
    }
}