using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Domain.Entities;
using TechLibrary.Exception;
using TechLibrary.Infrastructure.Security.Cryptography;
using TechLibrary.Persistence.Abstractions;

namespace TechLibrary.Application.UseCases.Users.Register;

public class RegisterUserUsecase
{
    private readonly ITechLibraryDbContext _dbContext;
    private readonly BCryptAlgorithm _bcryptAlgorithm;

    public RegisterUserUsecase(ITechLibraryDbContext dbContext, BCryptAlgorithm bcrypAlgorithm)
    {
        _dbContext = dbContext;
        _bcryptAlgorithm = bcrypAlgorithm;
    }

    public ResponseRegisteredUserJson Execute(RequestUserJson request)
    {
        ValidateRequest(request);
        
        var user = new User
        {
            Email = request.Email,
            Name = request.Name,
            Password = _bcryptAlgorithm.HashPassword(request.Password)
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name
        };
    }

    private void ValidateRequest(RequestUserJson request)
    {
        var validator = new RegisterUserValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var errorMensages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMensages);
        }
    }
}