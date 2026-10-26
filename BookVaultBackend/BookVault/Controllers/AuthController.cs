using BookVault.Application.DTOs.Auth;
using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVault.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;

	public AuthController(IAuthService authService)
	{
		_authService = authService;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register(
		[FromBody] RegisterRequest request)
	{
		await _authService.RegisterAsync(request);

		return StatusCode(
			StatusCodes.Status201Created,
			new
			{
				message = "User registered successfully."
			});
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login(
		[FromBody] LoginRequest request)
	{
		var token = await _authService.LoginAsync(request);

		return Ok(new
		{
			token
		});
	}


	[Authorize]
	[HttpGet("me")]
	public async Task<IActionResult> Me()
	{
		var userIdClaim = User.FindFirst(
			System.Security.Claims.ClaimTypes.NameIdentifier);

		if (userIdClaim is null)
			return Unauthorized();

		if (!Guid.TryParse(userIdClaim.Value, out var userId))
			return Unauthorized();

		var permissions =
			await _authService.GetUserPermissionsAsync(userId);

		return Ok(new
		{
			id = userId,
			username = User.Identity?.Name,
			permissions
		});
	}
}

