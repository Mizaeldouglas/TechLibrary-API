using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Persistence.Abstractions;

namespace TechLibrary.Application.UseCases.Books;

public class FilterBookUseCase
{
    private readonly ITechLibraryDbContext _dbContext;

    public FilterBookUseCase(ITechLibraryDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    private const int PAGE_SIZE = 10;

    public ResponseBooksJson Execute(RequestFilterBooksJson request)
    {
        

        var skip = ((request.PageNumber - 1) * PAGE_SIZE);

        
        var query = _dbContext.Books.AsQueryable();

        if(string.IsNullOrWhiteSpace(request.Title) == false)
        {
            query = query.Where(book => book.Title.Contains(request.Title));
        }

        var books = query
            .OrderBy(book => book.Title).ThenBy(book => book.Author)
            .Skip(skip)
            .Take(PAGE_SIZE)
            .ToList();

        var totalCount = 0;
        if(string.IsNullOrWhiteSpace(request.Title))
            totalCount = _dbContext.Books.Count();
        else
            totalCount = _dbContext.Books.Count(book => book.Title.Contains(request.Title));

        return new ResponseBooksJson
        {
            Pagination = new ResponsePaginationJson
            {
                PageNumber = request.PageNumber,
                TotalCount = totalCount,
            },
            Books = books.Select(book => new ResponseBookJson
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
            }).ToList()
        };
    }
}