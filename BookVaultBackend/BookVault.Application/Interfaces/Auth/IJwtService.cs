using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Auth;

public interface IJwtService
{
	string GenerateToken(User user);
}
