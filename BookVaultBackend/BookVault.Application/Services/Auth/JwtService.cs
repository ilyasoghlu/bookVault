using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookVault.Application.Interfaces.Auth;
using BookVault.Application.Models.Auth;
using BookVault.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace BookVault.Application.Services.Auth;

public class JwtService : IJwtService
{
	private readonly JwtSettings _jwtSettings;

	public JwtService(JwtSettings jwtSettings)
	{
		_jwtSettings = jwtSettings;
	}

	public string GenerateToken(User user)
	{
		var claims = new List<Claim>
		{
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new(JwtRegisteredClaimNames.Email, user.Email),
			new(ClaimTypes.Name, user.Username),
			new(ClaimTypes.NameIdentifier, user.Id.ToString())
		};

		var key = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

		var credentials = new SigningCredentials(
			key,
			SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _jwtSettings.Issuer,
			audience: _jwtSettings.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(
				_jwtSettings.ExpirationMinutes),
			signingCredentials: credentials);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
