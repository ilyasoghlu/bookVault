using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Auth;

public interface IPasswordService
{
	string HashPassword(User user, string password);

	bool VerifyPassword(
		User user,
		string password,
		string passwordHash);
}
