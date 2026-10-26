using BookVault.Application.DTOs.Auth;
using FluentValidation;

namespace BookVault.Application.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
	public RegisterRequestValidator()
	{
		RuleFor(x => x.FirstName)
			.NotEmpty()
			.MaximumLength(30);

		RuleFor(x => x.LastName)
			.NotEmpty()
			.MaximumLength(30);

		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress()
			.MaximumLength(255);

		RuleFor(x => x.UserName)
			.NotEmpty()
			.MaximumLength(30);

		RuleFor(x => x.Password)
			.NotEmpty()
			.MinimumLength(6);
	}
}
