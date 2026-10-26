using BookVault.Application.DTOs.Reader.Requests;
using FluentValidation;

namespace BookVault.Application.Validators.Reader;

public class CreateReaderValidator : AbstractValidator<CreateReaderDto>
{
	public CreateReaderValidator()
	{
		RuleFor(x => x.Firstname)
			.NotEmpty()
			.MaximumLength(30);

		RuleFor(x => x.Lastname)
			.NotEmpty()
			.MaximumLength(30);

		RuleFor(x => x.Email)
			.NotEmpty()
			.EmailAddress()
			.MaximumLength(30);

		RuleFor(x => x.Username)
			.NotEmpty()
			.MaximumLength(50);

		RuleFor(x => x.Password)
			.NotEmpty()
			.MinimumLength(6);

		RuleFor(x => x.Phone)
			.NotEmpty()
			.MaximumLength(20);

		RuleFor(x => x.Dateofbirth)
			.LessThanOrEqualTo(
				DateOnly.FromDateTime(DateTime.UtcNow))
			.WithMessage("Date of birth cannot be in the future.");

		RuleFor(x => x.Expirationdate)
			.GreaterThan(
				DateOnly.FromDateTime(DateTime.UtcNow))
			.WithMessage("Expiration date must be in the future.");
	}
}