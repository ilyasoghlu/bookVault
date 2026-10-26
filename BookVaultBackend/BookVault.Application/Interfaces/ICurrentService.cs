namespace BookVault.Application.Interfaces.Services;

public interface ICurrentUserService
{
	Guid? UserId { get; }
	string? Username { get; }
}