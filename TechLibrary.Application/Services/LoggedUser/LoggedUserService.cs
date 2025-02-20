using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using TechLibrary.Domain.Entities;
using TechLibrary.Infrastructure.Data;

namespace TechLibrary.Application.Services.LoggedUser;

  public class LoggedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    public User User(TechLibraryDbContext dbContext)
    {
        var authentication = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

        var token = authentication["Bearer ".Length..].Trim();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

        var userId = Guid.Parse(identifier);

        return dbContext.Users.First(user => user.Id == userId);
        
    }
}