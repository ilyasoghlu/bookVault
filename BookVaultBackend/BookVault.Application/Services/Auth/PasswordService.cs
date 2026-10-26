using BookVault.Application.Interfaces.Auth;
using BookVault.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BookVault.Application.Services.Auth;

public class PasswordService : IPasswordService
{
	private readonly IPasswordHasher<User> _passwordHasher;

	public PasswordService(IPasswordHasher<User> passwordHasher)
	{
		_passwordHasher = passwordHasher;
	}

	public string HashPassword(User user, string password)
	{
		return _passwordHasher.HashPassword(user, password);
	}

	public bool VerifyPassword(
		User user,
		string password,
		string passwordHash)
	{
		var result = _passwordHasher.VerifyHashedPassword(
			user,
			passwordHash,
			password);

		return result == PasswordVerificationResult.Success ||
			   result == PasswordVerificationResult.SuccessRehashNeeded;
	}
}
