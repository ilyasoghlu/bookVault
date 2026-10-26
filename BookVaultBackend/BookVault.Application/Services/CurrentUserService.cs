using System.Security.Claims;
using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace BookVault.Application.Services;

public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CurrentUserService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public Guid? UserId
	{
		get
		{
			var value = _httpContextAccessor.HttpContext?
				.User
				.FindFirstValue(ClaimTypes.NameIdentifier);

			return Guid.TryParse(value, out var userId)
				? userId
				: null;
		}
	}

	public string? Username
	{
		get
		{
			return _httpContextAccessor.HttpContext?
				.User
				.FindFirstValue(ClaimTypes.Name);
		}
	}
}