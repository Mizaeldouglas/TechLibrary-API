using TechLibrary.Domain.Entities;

namespace TechLibrary.Infrastructure.Security.Cryptography;

public class BCryptAlgorithm
{
    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    
    public bool VerifyPassword(string password, User user) => BCrypt.Net.BCrypt.Verify(password, user.Password);
}