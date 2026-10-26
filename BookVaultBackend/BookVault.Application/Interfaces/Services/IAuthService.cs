using BookVault.Application.DTOs.Auth;

namespace BookVault.Application.Interfaces.Services;

public interface IAuthService
{
	Task<Guid> RegisterAsync(RegisterRequest request);

	Task<string> LoginAsync(LoginRequest request);
	Task<List<string>> GetUserPermissionsAsync(Guid userId);
}
