using FluentValidation.Results;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Domain.Entities;
using TechLibrary.Exception;
using TechLibrary.Infrastructure.Security.Cryptography;
using TechLibrary.Infrastructure.Security.Tokens.Access;
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

        var tokenGenerator = new JwtTokenGenerator();
        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            AccessToken = tokenGenerator.GenerateToken(user)
        };
    }

    private void ValidateRequest(RequestUserJson request)
    {
        var validator = new RegisterUserValidator();
        var validationResult = validator.Validate(request);

        var existUserWithEmail = _dbContext.Users.Any(user => user.Email == request.Email);
        if (existUserWithEmail)
            validationResult.Errors.Add(
                new ValidationFailure("Email", "Já existe um usuário com este email"));

        if (!validationResult.IsValid)
        {
            var errorMensages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMensages);
        }
    }
}