using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;
using TechLibrary.Infrastructure.Security.Cryptography;
using TechLibrary.Infrastructure.Security.Tokens.Access;
using TechLibrary.Persistence.Abstractions;

namespace TechLibrary.Application.UseCases.Users.Login.DoLogin;

public class DoLoginUseCase
{
    private readonly BCryptAlgorithm _bcryptAlgorithm;
    private readonly ITechLibraryDbContext _dbContext;

    public DoLoginUseCase(ITechLibraryDbContext dbContext, BCryptAlgorithm bcryptAlgorithm)
    {
        _dbContext = dbContext;
        _bcryptAlgorithm = bcryptAlgorithm;
    }

    public ResponseRegisteredUserJson Execute(RequestLoginJson request)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Email.Equals(request.Email));
        if (user == null)
            throw new InvalidLoginException();
        
        var isPasswordValid = _bcryptAlgorithm.VerifyPassword(request.Password, user);

        if (!isPasswordValid)
            throw new InvalidLoginException();
        
        
        var tokenGenerator = new JwtTokenGenerator();
        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            AccessToken = tokenGenerator.GenerateToken(user)
        };
    }
}