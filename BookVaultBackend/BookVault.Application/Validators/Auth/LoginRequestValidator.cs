using BookVault.Application.DTOs.Auth;
using FluentValidation;

namespace BookVault.Application.Validators.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
	public LoginRequestValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress();

		RuleFor(x => x.Password)
			.NotEmpty();
	}
}
