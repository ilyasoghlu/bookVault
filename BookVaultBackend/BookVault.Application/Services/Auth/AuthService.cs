using BookVault.Application.DTOs.Auth;
using BookVault.Application.Exceptions;
using BookVault.Application.Interfaces.Auth;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Interfaces.Services;
using BookVault.Domain.Entities;

namespace BookVault.Application.Services.Auth;

public class AuthService : IAuthService
{
	private readonly IUserRepository _userRepository;
	private readonly IPasswordService _passwordService;
	private readonly IJwtService _jwtService;
	private readonly IUserRuleRepository _userRuleRepository;

	public AuthService(
		IUserRepository userRepository,
		IUserRuleRepository userRuleRepository,
		IPasswordService passwordService,
		IJwtService jwtService)
	{
		_userRepository = userRepository;
		_userRuleRepository = userRuleRepository;
		_passwordService = passwordService;
		_jwtService = jwtService;
	}

	public async Task<Guid> RegisterAsync(RegisterRequest request)
	{
		var email = request.Email.Trim().ToLowerInvariant();
		var username = request.UserName.Trim();

		var existingEmail =
			await _userRepository.GetByEmailAsync(email);

		if (existingEmail is not null)
		{
			throw new ConflictException(
				"A user with this email already exists.");
		}

		var existingUsername =
			await _userRepository.GetByUsernameAsync(username);

		if (existingUsername is not null)
		{
			throw new ConflictException(
				"A user with this username already exists.");
		}

		var now = DateTime.SpecifyKind(
			DateTime.UtcNow,
			DateTimeKind.Unspecified);


		var user = new User
		{
			Firstname = request.FirstName.Trim(),
			Lastname = request.LastName.Trim(),
			Email = email,
			Username = username,
			Isactive = true,
			Isdeleted = false,
			Createdat = now
		};

		user.Passwordhash = _passwordService.HashPassword(
			user,
			request.Password);

		await _userRepository.AddAsync(user);
		await _userRepository.SaveChangesAsync();

		return user.Id;
	}

	public async Task<string> LoginAsync(LoginRequest request)
	{
		var email = request.Email.Trim().ToLowerInvariant();

		var user = await _userRepository.GetByEmailAsync(email);

		if (user is null)
		{
			throw new UnauthorizedAccessException(
				"Invalid email or password.");
		}

		if (user.Isdeleted || !user.Isactive)
		{
			throw new UnauthorizedAccessException(
				"User account is inactive.");
		}

		var passwordValid = _passwordService.VerifyPassword(
			user,
			request.Password,
			user.Passwordhash);

		if (!passwordValid)
		{
			throw new UnauthorizedAccessException(
				"Invalid email or password.");
		}

		return _jwtService.GenerateToken(user);
	}

	public async Task<List<string>> GetUserPermissionsAsync(
	Guid userId)
	{
		return await _userRuleRepository
			.GetUserPermissionsAsync(userId);
	}
}

